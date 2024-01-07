package grpc

import (
	"context"

	"google.golang.org/protobuf/types/known/wrapperspb"
	commonmodel "socialnerworkapp.com/pkg/common/model"
	gen "socialnerworkapp.com/pkg/proto/gen"
	"socialnerworkapp.com/profile/internal/model"
	"socialnerworkapp.com/profile/internal/service"
)

type GrpcUserHandlerImp struct {
	gen.UnimplementedUsersServer
	service service.UserService
}

func (*GrpcUserHandlerImp) mustEmbedUnimplementedUsersServer() {
	panic("unimplemented")
}

func NewGrpcUserHandler(service service.UserService) *GrpcUserHandlerImp {
	return &GrpcUserHandlerImp{service: service}
}

func (h *GrpcUserHandlerImp) GetUserById(ctx context.Context, r *gen.GetUserByIdRequest) (*gen.UserReply, error) {
	user, err := h.service.GetUserById(ctx, uint(r.Id))
	if _, ok := err.(commonmodel.NotFoundError); ok {
		return nil, err
	}
	if err != nil {
		return nil, err
	}
	reply := &gen.UserReply{
		Id:      uint32(user.ID),
		Name:    user.Name,
		Surname: user.Surname,
		//Age:     user.Age,
		//Sex:     &wrapperspb.StringValue{Value: user.Sex},
		//Info:    user.Info
	}
	if user.City != nil {
		reply.City = &wrapperspb.StringValue{Value: *user.City}
	}
	return reply, nil
}

func (h *GrpcUserHandlerImp) AddUser(ctx context.Context, r *gen.AddUserRequest) (*gen.AddUserReply, error) {
	user := &model.User{
		Name:    r.User.Name,
		Surname: r.User.Surname,
	}
	if r.User.City != nil {
		user.City = &r.User.City.Value
	}
	err := h.service.PutUser(ctx, user, r.Password)
	if _, ok := err.(commonmodel.NotFoundError); ok {
		return nil, err
	}
	if err != nil {
		return nil, err
	}
	reply := &gen.AddUserReply{
		Id: uint32(user.ID),
	}
	return reply, nil
}

func (h *GrpcUserHandlerImp) GetUsersByName(r *gen.GetUsersByNameRequest, stream gen.Users_GetUsersByNameServer) error {
	users, err := h.service.GetUsersByName(context.Background(), r.Name, r.Surname)
	if err != nil {
		return err
	}
	for _, user := range users {
		reply := &gen.UserReply{
			Id:      uint32(user.ID),
			Name:    user.Name,
			Surname: user.Surname,
			//Age:     user.Age,
			//Sex:     &wrapperspb.StringValue{Value: user.Sex},
			//Info:    user.Info
		}
		if user.City != nil {
			reply.City = &wrapperspb.StringValue{Value: *user.City}
		}
		if err := stream.Send(reply); err != nil {
			return err
		}
	}

	return nil
}
