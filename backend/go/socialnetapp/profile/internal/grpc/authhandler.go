package grpc

import (
	"context"

	gen "socialnerworkapp.com/pkg/proto/gen"
	"socialnerworkapp.com/profile/internal/service"
)

type GrpcAuthHandlerImp struct {
	gen.UnimplementedAuthServer
	service service.AuthService
}

func (*GrpcAuthHandlerImp) mustEmbedUnimplementedAuthServer() {
	panic("unimplemented")
}

func NewGrpcAuthtHandler(service service.AuthService) *GrpcAuthHandlerImp {
	return &GrpcAuthHandlerImp{service: service}
}

func (h *GrpcAuthHandlerImp) Login(ctx context.Context, r *gen.LoginRequest) (*gen.LoginReply, error) {
	token, err := h.service.Login(ctx, uint(r.Id), r.Password)
	if err != nil {
		return nil, err
	}
	reply := &gen.LoginReply{Token: token}
	return reply, nil
}
