package cache

import (
	"context"

	"socialnerworkapp.com/profile/internal/model"
)

type CacheService interface {
	AddPost(_ context.Context, userId uint, post *model.Post) error
	GetPosts(_ context.Context, userId uint, offset uint, limit uint) ([]model.Post, error)
	WarmupCache(_ context.Context, userId uint, posts []model.Post) error
}
