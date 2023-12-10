package postservice

import (
	"context"
	"io"
	"log"
	"os"

	_ "github.com/google/uuid"
	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials/insecure"
	"socialnerworkapp.com/bff/internal/handler/dto"
	"socialnerworkapp.com/bff/internal/service"
	"socialnerworkapp.com/pkg/proto/gen"
)

type postServiceImp struct {
	grpcPostUrl string
}

func NewPostService() service.PostService {
	grpcPostUrl, _ := os.LookupEnv("GRPC_POSTS")
	return &postServiceImp{grpcPostUrl: grpcPostUrl}
}

// CreatePost implements service.PostService.
func (s *postServiceImp) CreatePost(ctx context.Context, userId uint, text string) error {
	conn, err := grpc.Dial(s.grpcPostUrl, grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		log.Fatalf("fail to dial: %v", err)
	}
	defer conn.Close()
	postClient := gen.NewPostClient(conn)
	createRequest := &gen.CreatePostRequest{UserId: uint32(userId), Text: text}
	_, err = postClient.CreatePost(ctx, createRequest)
	return err
}

// FeedPosts implements service.PostService.
func (s *postServiceImp) FeedPosts(ctx context.Context, userId uint, offset uint, limit uint) ([]dto.PostDto, error) {
	conn, err := grpc.Dial(s.grpcPostUrl, grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		log.Fatalf("fail to dial: %v", err)
	}
	defer conn.Close()
	postClient := gen.NewPostClient(conn)
	feedRequest := &gen.FeedPostsRequest{UserId: uint32(userId), Offset: uint32(offset), Limit: uint32(limit)}
	stream, err := postClient.FeedPosts(ctx, feedRequest)
	if err != nil {
		return nil, err
	}
	posts := make([]dto.PostDto, 0)
	for {
		reply, err := stream.Recv()
		if err == io.EOF {
			break
		}
		if err != nil {
			return nil, err
		}
		post := dto.PostDto{
			ID:       uint(reply.UserId),
			AuthorId: uint(reply.AuthorId),
			Message:  reply.Message,
		}
		posts = append(posts, post)
	}

	return posts, nil
}
