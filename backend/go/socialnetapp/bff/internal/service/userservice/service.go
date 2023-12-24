package userservice

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

type userServiceImp struct {
	grpcProfileUrl string
}

func NewUserService() service.UserService {
	grpcProfileUrl, _ := os.LookupEnv("GRPC_PROFILE")
	return &userServiceImp{grpcProfileUrl: grpcProfileUrl}
}

func (s *userServiceImp) Login(ctx context.Context, userId uint, password string) (*dto.LoggedinUserDto, error) {
	conn, err := grpc.Dial(s.grpcProfileUrl, grpc.WithTransportCredentials(insecure.NewCredentials())) // , opts...)
	if err != nil {
		log.Fatalf("fail to dial: %v", err)
	}
	defer conn.Close()
	id := uint32(userId)
	authClient := gen.NewAuthClient(conn)
	loginRequest := &gen.LoginRequest{Id: id, Password: password}
	token, err := authClient.Login(ctx, loginRequest)
	if err != nil {
		return nil, err
	}

	userClient := gen.NewUsersClient(conn)
	userRequest := &gen.GetUserByIdRequest{Id: id}
	user, err := userClient.GetUserById(ctx, userRequest)
	if err != nil {
		return nil, err
	}

	userDto := service.ConvertToUserDto(user)
	return &dto.LoggedinUserDto{
		User:  userDto,
		Token: token.Token,
	}, err
}

// GetUsersByName implements service.UserService.
func (s *userServiceImp) GetUsersByName(ctx context.Context, name string, surname string) ([]dto.UserDto, error) {
	conn, err := grpc.Dial(s.grpcProfileUrl, grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		log.Fatalf("fail to dial: %v", err)
	}
	defer conn.Close()
	userClient := gen.NewUsersClient(conn)
	getUsersRequest := &gen.GetUsersByNameRequest{
		Name:    name,
		Surname: surname,
	}
	stream, err := userClient.GetUsersByName(ctx, getUsersRequest)
	if err != nil {
		return nil, err
	}
	users := make([]dto.UserDto, 0)
	for {
		reply, err := stream.Recv()
		if err == io.EOF {
			break
		}
		if err != nil {
			return nil, err
		}
		user := service.ConvertToUserDto(reply)
		users = append(users, *user)
	}

	return users, nil
}

// PutUser implements service.UserService.
func (s *userServiceImp) PutUser(ctx context.Context, u *dto.NewUserDto, password string) (*dto.UserDto, error) {
	conn, err := grpc.Dial(s.grpcProfileUrl, grpc.WithTransportCredentials(insecure.NewCredentials())) // , opts...)
	if err != nil {
		log.Fatalf("fail to dial: %v", err)
	}
	defer conn.Close()

	userClient := gen.NewUsersClient(conn)
	addUserRequest := &gen.AddUserRequest{
		Password: password,
	}
	reply, err := userClient.AddUser(ctx, addUserRequest)
	if err != nil {
		return nil, err
	}

	userDto := &dto.UserDto{
		ID: uint(reply.Id),
	}

	return userDto, nil
}
