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

type FriendRepository interface {
	UpsertFriend(_ context.Context, userId uint, friendId uint) error
	DeleteFriend(_ context.Context, userId uint, friendId uint) error
	GetSubscriberIds(_ context.Context, userId uint) ([]uint, error)
}

type PostRepository interface {
	AddPost(_ context.Context, userId uint, text string) (*model.Post, error)
	UpdatePost(_ context.Context, userId uint, postId uint, text string) error
	DeletePost(_ context.Context, userId uint, postId uint) error
	GetPost(_ context.Context, postId uint) (*model.Post, error)
	GetPosts(_ context.Context, userId uint, offset uint, limit uint) ([]model.Post, error)
	GetLatestFriendsPosts(_ context.Context, userId uint, limit uint) ([]model.Post, error)
}
