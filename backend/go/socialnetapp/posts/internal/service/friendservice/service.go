package friendservice

import (
	"context"

	"socialnerworkapp.com/posts/internal/repository"
	"socialnerworkapp.com/posts/internal/service"
)

type friendServiceImp struct {
	repository repository.FriendRepository
}

func NewFriendService(repository repository.FriendRepository) service.FriendService {
	return &friendServiceImp{repository: repository}
}

func (s *friendServiceImp) UpsertFriend(ctx context.Context, userId uint, friendId uint) error {
	return s.repository.UpsertFriend(ctx, userId, friendId)
}

func (s *friendServiceImp) DeleteFriend(ctx context.Context, userId uint, friendId uint) error {
	return s.repository.DeleteFriend(ctx, userId, friendId)
}
