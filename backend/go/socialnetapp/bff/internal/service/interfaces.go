package service

import (
	"context"

	"go.opentelemetry.io/otel/trace"
	"socialnerworkapp.com/bff/internal/handler/dto"
)

type UserService interface {
	Login(_ context.Context, tracer trace.Tracer, userId uint, password string) (*dto.LoggedinUserDto, error)
	GetUsersByName(_ context.Context, tracer trace.Tracer, name string, surname string) ([]dto.UserDto, error)
	PutUser(_ context.Context, tracer trace.Tracer, dto *dto.NewUserDto, password string) (*dto.UserDto, error)
}

type FriendService interface {
	AddFriend(_ context.Context, userId uint, friendId uint) (*dto.UserDto, error)
	DeleteFriend(_ context.Context, userId uint, friendId uint) error
	GetFriends(_ context.Context, userId uint) ([]dto.UserDto, error)
}

type PostService interface {
	CreatePost(_ context.Context, userId uint, text string) error
	FeedPosts(_ context.Context, userId uint, offset uint, limit uint) (*dto.UserPostsDto, error)
}

type DialogsService interface {
	CreateMessage(_ context.Context, tracer trace.Tracer, authorId uint, userId uint, text string) (*dto.MessageDto, error)
	GetMessages(_ context.Context, tracer trace.Tracer, authorId uint, userId uint) (*dto.UserMessagesDto, error)
	GetDialogBuddies(_ context.Context, tracer trace.Tracer, userId uint) ([]dto.UserDto, error)
}
