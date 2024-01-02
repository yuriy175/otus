package dialogsservice

import (
	"context"
	"encoding/json"

	"socialnerworkapp.com/dialogs/internal/model"
	"socialnerworkapp.com/dialogs/internal/repository"
	"socialnerworkapp.com/dialogs/internal/service"
	"socialnerworkapp.com/pkg/mq"
)

type dialogsServiceImp struct {
	repository repository.DialogsRepository
	mqSender   mq.MqSender
}

func NewDialogsService(
	repository repository.DialogsRepository,
	mqSender mq.MqSender) service.DialogsService {
	return &dialogsServiceImp{repository: repository, mqSender: mqSender}
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
	err = s.mqSender.SendDialogMessage(ctx, bytes)
	return message, err
}

func (s *dialogsServiceImp) GetMessages(ctx context.Context, userId1 uint, userId2 uint) ([]model.Message, error) {
	return s.repository.GetMessages(ctx, userId1, userId2)
}

// GetBuddyIds implements service.DialogsService.
func (s *dialogsServiceImp) GetBuddyIds(ctx context.Context, userId uint) ([]uint, error) {
	return s.repository.GetBuddyIds(ctx, userId)
}
