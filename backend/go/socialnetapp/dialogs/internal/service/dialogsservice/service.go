package dialogsservice

import (
	"context"
	"encoding/json"
	"log"

	"socialnerworkapp.com/dialogs/internal/cache"
	"socialnerworkapp.com/dialogs/internal/model"
	"socialnerworkapp.com/dialogs/internal/repository"
	"socialnerworkapp.com/dialogs/internal/service"
	"socialnerworkapp.com/pkg/mq"
	mqtypes "socialnerworkapp.com/pkg/mq/types"
)

type dialogsServiceImp struct {
	repository repository.DialogsRepository
	mqSender   mq.MqSender
	mqReceiver mq.MqReceiver
	cacheSrv   cache.CacheService
}

func NewDialogsService(
	repository repository.DialogsRepository,
	mqSender mq.MqSender,
	mqReceiver mq.MqReceiver,
	cacheSrv cache.CacheService) service.DialogsService {
	srv := &dialogsServiceImp{
		repository: repository,
		mqSender:   mqSender,
		mqReceiver: mqReceiver,
		cacheSrv:   cacheSrv,
	}
	mqReceiver.CreateUnreadDialogMessagesCountFailedReceiver(func(data []byte) {
		message := &mqtypes.UnreadCountMessage{}
		if err := json.Unmarshal(data, message); err != nil {
			log.Println(err)
			return
		}

		if message.MessageType == mq.UpdateUnreadDialogMessagesCompensate {
			ctx := context.Background()
			_, _ = srv.repository.SetUnreadDialogMessages(ctx, message.UnreadMessageIds)
		}
	})
	return srv
}

func (s *dialogsServiceImp) CreateMessage(ctx context.Context, authorId uint, userId uint, text string) (*model.Message, error) {
	message, err := s.repository.CreateMessage(ctx, authorId, userId, text)
	if err != nil {
		return nil, err
	}

	bytes, err := json.Marshal(message)
	if err != nil {
		return nil, err
	}

	wsAddress, err := s.cacheSrv.GetUserWebSocketAddress(ctx, userId)
	if err != nil {
		// increment unread counter
		s.sendUnreadDialogMessageIds(ctx, &mqtypes.UnreadCountMessage{
			MessageHeader:    mqtypes.MessageHeader{MessageType: mq.UpdateUnreadDialogMessages},
			UserId:           userId,
			IsIncrement:      true,
			UnreadMessageIds: []int{int(message.Id)},
		})
	} else {
		err = s.mqSender.SendNewDialogMessage(ctx, wsAddress, bytes)
	}
	return message, err
}

func (s *dialogsServiceImp) GetMessages(ctx context.Context, userId1 uint, userId2 uint) ([]model.Message, error) {
	return s.repository.GetMessages(ctx, userId1, userId2)
}

// GetBuddyIds implements service.DialogsService.
func (s *dialogsServiceImp) GetBuddyIds(ctx context.Context, userId uint) ([]uint, error) {
	return s.repository.GetBuddyIds(ctx, userId)
}

func (s *dialogsServiceImp) SetUnreadMessagesFromUser(ctx context.Context, authorId uint, userId uint) (int, error) {
	unreadMsgIds, err := s.repository.SetReadDialogMessagesFromUser(ctx, authorId, userId)
	if err != nil {
		return 0, err
	}

	count := len(unreadMsgIds)
	if count > 0 {
		s.sendUnreadDialogMessageIds(ctx, &mqtypes.UnreadCountMessage{
			MessageHeader:    mqtypes.MessageHeader{MessageType: mq.UpdateUnreadDialogMessages},
			UserId:           userId,
			IsIncrement:      false,
			UnreadMessageIds: unreadMsgIds,
		})
	}

	return count, err
}

func (s *dialogsServiceImp) sendUnreadDialogMessageIds(ctx context.Context, message *mqtypes.UnreadCountMessage) error {
	bytes, err := json.Marshal(message)
	if err != nil {
		return err
	}

	s.mqSender.SendUnreadDialogMessageIds(ctx, bytes)

	return nil
}
