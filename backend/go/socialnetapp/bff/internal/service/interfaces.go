package service

import (
	"context"

	"socialnerworkapp.com/bff/internal/handler/dto"
)

type UserService interface {
	Login(_ context.Context, userId uint, password string) (*dto.LoggedinUserDto, error)
	GetUsersByName(_ context.Context, name string, surname string) ([]dto.UserDto, error)
	PutUser(_ context.Context, dto *dto.NewUserDto, password string) (*dto.UserDto, error)
}

type FriendService interface {
	AddFriend(_ context.Context, userId uint, friendId uint) (*dto.UserDto, error)
	DeleteFriend(_ context.Context, userId uint, friendId uint) error
	GetFriends(_ context.Context, userId uint) ([]dto.UserDto, error)
}

type PostService interface {
	CreatePost(_ context.Context, userId uint, text string) error
	FeedPosts(_ context.Context, userId uint, offset uint, limit uint) ([]dto.PostDto, error)
}
