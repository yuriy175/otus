package repository

import (
	"context"

	"socialnerworkapp.com/posts/internal/model"
)

type FriendRepository interface {
	UpsertFriend(_ context.Context, userId uint, friendId uint) error
	DeleteFriend(_ context.Context, userId uint, friendId uint) error
	GetSubscriberIds(_ context.Context, userId uint) ([]uint, error)
	GetFriendIds(_ context.Context, userId uint) ([]uint, error)
}

type PostRepository interface {
	AddPost(_ context.Context, userId uint, text string) (*model.Post, error)
	UpdatePost(_ context.Context, userId uint, postId uint, text string) error
	DeletePost(_ context.Context, userId uint, postId uint) error
	GetPost(_ context.Context, postId uint) (*model.Post, error)
	GetPosts(_ context.Context, userId uint, offset uint, limit uint) ([]model.Post, error)
	GetLatestFriendsPosts(_ context.Context, userId uint, limit uint) ([]model.Post, error)
}
