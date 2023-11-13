package redis

import (
	"context"
	"encoding/json"
	"fmt"
	"os"
	"strconv"
	"time"

	"github.com/go-redis/redis"
	"socialnerworkapp.com/profile/internal/cache"
	"socialnerworkapp.com/profile/internal/model"
)

type redisServiceImp struct {
	client     *redis.Client
	itemsCount int
}

func NewRedisService() cache.CacheService {
	host, _ := os.LookupEnv("REDIS_HOST")
	value, _ := os.LookupEnv("CACHE_ITEMS_COUNT")
	itemsCount, _ := strconv.Atoi(value)
	client := redis.NewClient(&redis.Options{
		Addr:     host,
		Password: "",
		DB:       0,
	})

	pong, err := client.Ping().Result()
	fmt.Println(pong, err)

	return &redisServiceImp{client: client, itemsCount: itemsCount}
}

// AddPost implements cache.CacheService.
func (s *redisServiceImp) AddPost(_ context.Context, userId uint, post *model.Post) error {
	userKey := s.getKey(userId)
	value, _ := json.Marshal(post)

	_, err := s.client.Do("fcall", "addpost", 1, userKey, value, int64(s.itemsCount)-1).Result()
	return err

	/*cmd := s.client.LPush(userKey, value)
	if cmd.Err() != nil {
		return cmd.Err()
	}

	status := s.client.LTrim(userKey, 0, int64(s.itemsCount)-1)
	return status.Err()*/
}

// GetPosts implements cache.CacheService.
func (s *redisServiceImp) GetPosts(_ context.Context, userId uint, offset uint, limit uint) ([]model.Post, error) {
	userKey := s.getKey(userId)
	/*values, err := s.client.LRange(userKey, int64(offset), int64(limit)).Result()
	if err != nil {
		return nil, err
	}*/
	result, err := s.client.Do("fcall", "getposts", 1, userKey, offset, limit).Result()
	if err != nil {
		return nil, err
	}
	values, ok := result.([]interface{})
	if !ok {
		return nil, nil
	}
	posts := make([]model.Post, 0)
	for _, v := range values {
		post := model.Post{}
		err = json.Unmarshal([]byte(v.(string)), &post)
		posts = append(posts, post)
	}

	return posts, nil
}

// WarmupCache implements cache.CacheService.
func (s *redisServiceImp) WarmupCache(_ context.Context, userId uint, posts []model.Post) error {
	userKey := s.getKey(userId)
	duration, _ := time.ParseDuration("4h")
	ok := s.client.Expire(userKey, duration)
	if ok.Err() != nil {
		return ok.Err()
	}

	values := make([]interface{}, 4+len(posts))
	values[0] = "fcall"
	values[1] = "warmupposts"
	values[2] = 1
	values[3] = userKey
	for i, v := range posts {
		value, _ := json.Marshal(v)
		values[i+4] = string(value)
	}
	_, err := s.client.Do(values).Result()
	return err

	//cmd := s.client.RPush(userKey, values)
	//return cmd.Err()
}

func (s *redisServiceImp) getKey(userId uint) string {
	return fmt.Sprintf("user:%d", userId)
}
