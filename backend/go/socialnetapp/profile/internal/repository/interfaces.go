package repository

import (
	"context"

	"socialnerworkapp.com/profile/internal/model"
)

type UserRepository interface {
	GetUsers(_ context.Context) ([]model.User, error)
	GetUserById(_ context.Context, userId uint) (*model.User, error)
	PutUser(_ context.Context, user *model.User, hash model.PasswordType) error
	CheckUser(_ context.Context, userId uint, hash model.PasswordType) (bool, error)
	GetUsersByName(_ context.Context, name string, surname string) ([]model.User, error)
}
