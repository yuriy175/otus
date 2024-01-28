package redis

import (
	"context"
	"fmt"
	"os"

	"github.com/go-redis/redis"
	"socialnerworkapp.com/dialogs/internal/cache"
)

type redisServiceImp struct {
	client *redis.Client
}

func NewRedisService() cache.CacheService {
	host, _ := os.LookupEnv("REDIS_HOST")
	client := redis.NewClient(&redis.Options{
		Addr:     host,
		Password: "",
		DB:       0,
	})

	pong, err := client.Ping().Result()
	fmt.Println(pong, err)

	return &redisServiceImp{client: client}
}

func (s *redisServiceImp) GetUserWebSocketAddress(_ context.Context, userId uint) (string, error) {
	userKey := s.getKey(userId)

	cmd := s.client.Get(userKey)
	return cmd.String(), cmd.Err()
}

func (s *redisServiceImp) getKey(userId uint) string {
	return fmt.Sprintf("user:%d_ws", userId)
}
