package cache

import (
	"context"
)

type CacheService interface {
	GetUserWebSocketAddress(_ context.Context, userId uint) (string, error)
}
