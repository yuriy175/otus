package cache

import (
	"context"
)

type CacheService interface {
	UpsertUserWebSocket(_ context.Context, userId uint, hostName string, port string) error
}
