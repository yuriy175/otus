package postservice

import (
	"context"

	"socialnerworkapp.com/profile/internal/cache"
	"socialnerworkapp.com/profile/internal/model"
	"socialnerworkapp.com/profile/internal/repository"
	"socialnerworkapp.com/profile/internal/service"
)

type postServiceImp struct {
	repository       repository.PostRepository
	friendRepository repository.FriendRepository
	cacheService     cache.CacheService
}

func NewPostService(
	repository repository.PostRepository,
	friendRepository repository.FriendRepository,
	cacheService cache.CacheService) service.PostService {
	return &postServiceImp{
		repository:       repository,
		friendRepository: friendRepository,
		cacheService:     cacheService}
}

// CreatePost implements service.PostService.
func (s *postServiceImp) CreatePost(ctx context.Context, userId uint, text string) error {
	post, err := s.repository.AddPost(ctx, userId, text)
	if err != nil {
		return err
	}

	subscriberIds, err := s.friendRepository.GetSubscriberIds(ctx, userId)
	if err != nil {
		return err
	}

	for _, v := range subscriberIds {
		go s.cacheService.AddPost(ctx, v, post)
	}

	return nil
}

// DeletePost implements service.PostService.
func (s *postServiceImp) DeletePost(ctx context.Context, userId uint, postId uint) error {
	return s.repository.DeletePost(ctx, userId, postId)
}

// FeedPosts implements service.PostService.
func (s *postServiceImp) FeedPosts(ctx context.Context, userId uint, offset uint, limit uint) ([]model.Post, error) {
	posts, err := s.cacheService.GetPosts(ctx, userId, offset, limit)
	if err != nil {
		return nil, err
	}

	if len(posts) == 0 {
		posts, err = s.repository.GetLatestFriendsPosts(ctx, userId, limit)
		if err != nil {
			return nil, err
		}

		err = s.cacheService.WarmupCache(ctx, userId, posts)
		if err != nil {
			return nil, err
		}

		posts, err = s.cacheService.GetPosts(ctx, userId, offset, limit)
	}

	return posts, err
}

// GetPost implements service.PostService.
func (s *postServiceImp) GetPost(ctx context.Context, userId uint, postId uint) (*model.Post, error) {
	return s.repository.GetPost(ctx, userId, postId)
}

// UpdatePost implements service.PostService.
func (s *postServiceImp) UpdatePost(ctx context.Context, userId uint, postId uint, text string) error {
	return s.repository.UpdatePost(ctx, userId, postId, text)
}
