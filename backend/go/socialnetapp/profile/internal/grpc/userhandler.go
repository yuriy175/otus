package grpc

import (
	"context"

	"google.golang.org/protobuf/types/known/wrapperspb"
	commonmodel "socialnerworkapp.com/pkg/common/model"
	gen "socialnerworkapp.com/pkg/proto/gen"
	"socialnerworkapp.com/profile/internal/service"
)

type GrpcUserHandlerImp struct {
	gen.UnimplementedUsersServer
	service service.UserService
}

func (*GrpcUserHandlerImp) mustEmbedUnimplementedAuthServer() {
	panic("unimplemented")
}

func NewGrpcUserHandler(service service.UserService) *GrpcUserHandlerImp {
	return &GrpcUserHandlerImp{service: service}
}

func (h *GrpcUserHandlerImp) GetUserById(ctx context.Context, r *gen.GetUserByIdRequest) (*gen.GetUserByIdReply, error) {
	user, err := h.service.GetUserById(ctx, uint(r.Id))
	if _, ok := err.(commonmodel.NotFoundError); ok {
		return nil, err
	}
	if err != nil {
		return nil, err
	}
	reply := &gen.GetUserByIdReply{
		Id:      uint32(user.ID),
		Name:    user.Name,
		Surname: user.Surname,
		//Age:     user.Age,
		//Sex:     &wrapperspb.StringValue{Value: user.Sex},
		//City:     wrapperspb.StringValue{Value: user.City},
		//Info:    user.Info
	}
	if user.City != nil {
		reply.City = &wrapperspb.StringValue{Value: *user.City}
	}
	return reply, nil
}
