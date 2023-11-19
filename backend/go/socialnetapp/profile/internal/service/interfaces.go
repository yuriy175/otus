package service

import (
	"context"

	"socialnerworkapp.com/profile/internal/model"
)

type UserService interface {
	GetUsers(_ context.Context) ([]model.User, error)
	GetUserById(_ context.Context, userId uint) (*model.User, error)
	PutUser(_ context.Context, user *model.User, password string) error
	GetUsersByName(_ context.Context, name string, surname string) ([]model.User, error)
}

type AuthService interface {
	Login(_ context.Context, userId uint, password string) (string, error)
	GetAuthorizedUserId(tokenString string) string
}
