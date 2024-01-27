package grpc

import (
	"context"

	"socialnerworkapp.com/counters/internal/service"
	gen "socialnerworkapp.com/pkg/proto/gen"
)

type GrpcCountersHandlerImp struct {
	gen.UnimplementedCounterServer
	service service.CountersService
}

func (*GrpcCountersHandlerImp) mustEmbedUnimplementedCounterServer() {
	panic("unimplemented")
}

func NewGrpcCountersHandler(service service.CountersService) *GrpcCountersHandlerImp {
	return &GrpcCountersHandlerImp{service: service}
}

func (h *GrpcCountersHandlerImp) GetUnreadCount(ctx context.Context, r *gen.GetUnreadCountRequest) (*gen.GetUnreadCountReply, error) {
	count, err := h.service.GetUnReadCounterByUserId(ctx, uint(r.UserId))

	return &gen.GetUnreadCountReply{Count: uint32(count)}, err
}
