package countersservice

import (
	"context"
	"encoding/json"
	"log"

	"socialnerworkapp.com/counters/internal/repository"
	"socialnerworkapp.com/counters/internal/service"
	"socialnerworkapp.com/pkg/mq"
	mqtypes "socialnerworkapp.com/pkg/mq/types"
)

type countersServiceImp struct {
	repository repository.CountersRepository
	mqSender   mq.MqSender
	mqReceiver mq.MqReceiver
}

func NewCountersService(
	repository repository.CountersRepository,
	mqSender mq.MqSender,
	mqReceiver mq.MqReceiver) service.CountersService {

	srv := &countersServiceImp{repository: repository, mqSender: mqSender, mqReceiver: mqReceiver}
	mqReceiver.CreateUnreadDialogMessagesCountReceiver(func(data []byte) {
		message := &mqtypes.UnreadCountMessage{}
		if err := json.Unmarshal(data, message); err != nil {
			log.Println(err)
			return
		}

		if message.MessageType == mq.UpdateUnreadDialogMessages {
			count := len(message.UnreadMessageIds)
			ctx := context.Background()
			if !message.IsIncrement {
				count = -count
			}
			_, err := srv.repository.UpdateUnReadCounterByUserId(ctx, message.UserId, count)
			if err != nil {
				message.MessageType = mq.UpdateUnreadDialogMessagesCompensate
				bytes, err := json.Marshal(message)
				if err != nil {
					return
				}
				srv.mqSender.SendUnreadDialogMessageIdsFailed(ctx, bytes)
			}
		}
	})

	return srv
}

func (s *countersServiceImp) UpdateUnReadCounterByUserId(ctx context.Context, userId uint, delta int) (int, error) {
	return s.repository.UpdateUnReadCounterByUserId(ctx, userId, delta)
}

func (s *countersServiceImp) GetUnReadCounterByUserId(ctx context.Context, userId uint) (uint, error) {
	return s.repository.GetUnReadCounterByUserId(ctx, userId)
}
