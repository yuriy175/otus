package userservice

import (
	"context"

	"socialnerworkapp.com/profile/internal/model"
	"socialnerworkapp.com/profile/internal/repository"
	"socialnerworkapp.com/profile/internal/service"
)

type userServiceImp struct {
	repository repository.UserRepository
}

func NewUserService(repository repository.UserRepository) service.UserService {
	return &userServiceImp{repository: repository}
}

func (s *userServiceImp) GetUsers(ctx context.Context) ([]model.User, error) {
	return s.repository.GetUsers(ctx)
}

func (s *userServiceImp) GetUserById(ctx context.Context, userId uint) (*model.User, error) {
	return s.repository.GetUserById(ctx, userId)
}

func (s *userServiceImp) PutUser(ctx context.Context, user *model.User, password string) error {
	return s.repository.PutUser(ctx, user, password)
}
