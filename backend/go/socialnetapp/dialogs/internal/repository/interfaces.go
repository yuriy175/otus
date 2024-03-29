package repository

import (
	"context"

	"socialnerworkapp.com/dialogs/internal/model"
)

type DialogsRepository interface {
	CreateMessage(_ context.Context, authorId uint, userId uint, text string) (*model.Message, error)
	GetMessages(_ context.Context, userId1 uint, userId2 uint) ([]model.Message, error)
	GetBuddyIds(_ context.Context, userId uint) ([]uint, error)
	SetReadDialogMessagesFromUser(_ context.Context, authorId uint, userId uint) ([]int, error)
	SetUnreadDialogMessages(_ context.Context, msgIds []int) (int, error)
}
