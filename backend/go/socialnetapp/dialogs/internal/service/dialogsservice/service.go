package dialogsservice

import (
	"context"

	"socialnerworkapp.com/dialogs/internal/model"
	"socialnerworkapp.com/dialogs/internal/repository"
	"socialnerworkapp.com/dialogs/internal/service"
)

type dialogsServiceImp struct {
	repository repository.DialogsRepository
}

func NewDialogsService(repository repository.DialogsRepository) service.DialogsService {
	return &dialogsServiceImp{repository: repository}
}

func (s *dialogsServiceImp) CreateMessage(ctx context.Context, authorId uint, userId uint, text string) (*model.Message, error) {
	return s.repository.CreateMessage(ctx, authorId, userId, text)
}

func (s *dialogsServiceImp) GetMessages(ctx context.Context, userId1 uint, userId2 uint) ([]model.Message, error) {
	return s.repository.GetMessages(ctx, userId1, userId2)
}
