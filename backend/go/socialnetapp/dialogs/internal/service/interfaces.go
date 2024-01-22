package service

import (
	"context"

	"socialnerworkapp.com/dialogs/internal/model"
)

type DialogsService interface {
	CreateMessage(_ context.Context, authorId uint, userId uint, text string) (*model.Message, error)
	GetMessages(_ context.Context, userId1 uint, userId2 uint) ([]model.Message, error)
	GetBuddyIds(_ context.Context, userId uint) ([]uint, error)
	SetUnreadMessagesFromUser(_ context.Context, authorId uint, userId uint) (int, error)
}
