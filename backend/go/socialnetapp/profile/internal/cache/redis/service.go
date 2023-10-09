package redis

import (
	"context"

	"socialnerworkapp.com/profile/internal/cache"
	"socialnerworkapp.com/profile/internal/model"
)

type redisServiceImp struct {
}

func NewRedisService() cache.CacheService {
	return &redisServiceImp{}
}

// AddPost implements cache.CacheService.
func (*redisServiceImp) AddPost(_ context.Context, userId uint, post *model.Post) error {
	return nil
}

// GetPosts implements cache.CacheService.
func (*redisServiceImp) GetPosts(_ context.Context, userId uint, offset uint, limit uint) ([]model.Post, error) {
	return []model.Post{}, nil
}

// WarmupCache implements cache.CacheService.
func (*redisServiceImp) WarmupCache(_ context.Context, userId uint, posts []model.Post) error {
	return nil
}
