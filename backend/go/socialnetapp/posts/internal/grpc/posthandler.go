package grpc

import (
	"context"

	"google.golang.org/protobuf/types/known/emptypb"
	"google.golang.org/protobuf/types/known/timestamppb"
	gen "socialnerworkapp.com/pkg/proto/gen"
	"socialnerworkapp.com/posts/internal/service"
)

type GrpcPostsHandlerImp struct {
	gen.UnimplementedPostServer
	service service.PostService
}

func (*GrpcPostsHandlerImp) mustEmbedUnimplementedAuthServer() {
	panic("unimplemented")
}

func NewGrpcPostsHandler(service service.PostService) *GrpcPostsHandlerImp {
	return &GrpcPostsHandlerImp{service: service}
}

func (h *GrpcPostsHandlerImp) CreatePost(ctx context.Context, r *gen.CreatePostRequest) (*emptypb.Empty, error) {
	err := h.service.CreatePost(ctx, uint(r.UserId), r.Text)
	return &emptypb.Empty{}, err
}

func (h *GrpcPostsHandlerImp) FeedPosts(r *gen.FeedPostsRequest, stream gen.Post_FeedPostsServer) error {
	posts, err := h.service.FeedPosts(context.Background(), uint(r.UserId), uint(r.Offset), uint(r.Limit))
	if err != nil {
		return err
	}
	for _, post := range posts {
		created := post.Created
		timestamp := timestamppb.Timestamp{
			Seconds: created.Unix(),
			Nanos:   int32(created.Nanosecond()),
		}
		reply := gen.PostReply{
			UserId:   uint32(post.ID),
			AuthorId: uint32(post.AuthorId),
			Message:  post.Message,
			Created:  &timestamp}
		if err := stream.Send(&reply); err != nil {
			return err
		}
	}

	return nil
}
