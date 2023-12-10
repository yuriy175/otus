package grpc

import (
	"context"

	"google.golang.org/protobuf/types/known/emptypb"
	"socialnerworkapp.com/pkg/common/helpers"
	gen "socialnerworkapp.com/pkg/proto/gen"
	"socialnerworkapp.com/posts/internal/service"
)

type GrpcFriendsHandlerImp struct {
	gen.UnimplementedFriendServer
	service service.FriendService
}

func (*GrpcFriendsHandlerImp) mustEmbedUnimplementedAuthServer() {
	panic("unimplemented")
}

func NewGrpcFriendsHandler(service service.FriendService) *GrpcFriendsHandlerImp {
	return &GrpcFriendsHandlerImp{service: service}
}

func (h *GrpcFriendsHandlerImp) GetFriendIds(ctx context.Context, r *gen.GetFriendIdsRequest) (*gen.GetFriendIdsReply, error) {
	friendIds, err := h.service.GetFriendIds(ctx, uint(r.Id))
	if err != nil {
		return nil, err
	}
	reply := &gen.GetFriendIdsReply{
		Ids: helpers.ConvertInts[uint32](friendIds),
	}
	return reply, nil
}

func (h *GrpcFriendsHandlerImp) AddFriend(ctx context.Context, r *gen.AddFriendRequest) (*emptypb.Empty, error) {
	err := h.service.UpsertFriend(ctx, uint(r.UserId), uint(r.FriendId))
	return &emptypb.Empty{}, err
}

func (h *GrpcFriendsHandlerImp) DeleteFriend(ctx context.Context, r *gen.DeleteFriendRequest) (*emptypb.Empty, error) {
	err := h.service.DeleteFriend(ctx, uint(r.UserId), uint(r.FriendId))
	return &emptypb.Empty{}, err
}
