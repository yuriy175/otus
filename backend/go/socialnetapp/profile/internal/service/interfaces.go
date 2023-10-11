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

type FriendService interface {
	UpsertFriend(_ context.Context, userId uint, friendId uint) error
	DeleteFriend(_ context.Context, userId uint, friendId uint) error
}

type PostService interface {
	CreatePost(_ context.Context, userId uint, text string) error
	UpdatePost(_ context.Context, userId uint, postId uint, text string) error
	DeletePost(_ context.Context, userId uint, postId uint) error
	GetPost(_ context.Context, postId uint) (*model.Post, error)
	FeedPosts(_ context.Context, userId uint, offset uint, limit uint) ([]model.Post, error)
}
