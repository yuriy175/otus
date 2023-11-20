package service

import (
	"context"

	"socialnerworkapp.com/posts/internal/model"
)

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
