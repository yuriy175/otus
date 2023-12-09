package userservice

import (
	"context"
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
	loginRequest := &gen.LoginRequest{Id: id, Password: "Абрамов"}
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

	userDto := &dto.UserDto{
		ID:      uint(user.Id),
		Name:    user.Name,
		Surname: user.Surname,
		//Age:     user.Age.Value,
		//Sex:     user.Sex,
	}
	if user.City != nil {
		userDto.City = &user.City.Value
	}
	if user.Info != nil {
		userDto.Info = &user.Info.Value
	}
	return &dto.LoggedinUserDto{
		User:  userDto,
		Token: token.Token,
	}, err
}
