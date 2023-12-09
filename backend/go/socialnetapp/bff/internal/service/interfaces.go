package service

import (
	"context"

	"socialnerworkapp.com/bff/internal/handler/dto"
)

type UserService interface {
	Login(_ context.Context, userId uint, password string) (*dto.LoggedinUserDto, error)
}
