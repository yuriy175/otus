package redis

import (
	"context"
	"fmt"
	"os"

	"github.com/go-redis/redis"
	"socialnerworkapp.com/websockets/internal/cache"
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

func (s *redisServiceImp) UpsertUserWebSocket(_ context.Context, userId uint, hostName string, port string) error {
	userKey := s.getKey(userId)

	cmd := s.client.Set(userKey, hostName+"_"+port, 0)
	return cmd.Err()
}

func (s *redisServiceImp) getKey(userId uint) string {
	return fmt.Sprintf("user:%d_ws", userId)
}
