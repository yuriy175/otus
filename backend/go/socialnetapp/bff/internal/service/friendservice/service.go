package friendservice

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

type friendServiceImp struct {
	grpcFriendUrl  string
	grpcProfileUrl string
}

func NewFriendService() service.FriendService {
	grpcFriendUrl, _ := os.LookupEnv("GRPC_POSTS")
	grpcProfileUrl, _ := os.LookupEnv("GRPC_PROFILE")
	return &friendServiceImp{grpcFriendUrl: grpcFriendUrl, grpcProfileUrl: grpcProfileUrl}
}

// AddFriend implements service.FriendService.
func (s *friendServiceImp) AddFriend(ctx context.Context, userId uint, friendId uint) (*dto.UserDto, error) {
	conn, err := grpc.Dial(s.grpcFriendUrl, grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		log.Fatalf("fail to dial: %v", err)
	}
	defer conn.Close()
	friendClient := gen.NewFriendClient(conn)
	addRequest := &gen.AddFriendRequest{UserId: uint32(userId), FriendId: uint32(friendId)}
	_, err = friendClient.AddFriend(ctx, addRequest)
	if err != nil {
		return nil, err
	}

	userConn, err := grpc.Dial(s.grpcProfileUrl, grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		log.Fatalf("fail to dial: %v", err)
	}
	defer userConn.Close()
	userClient := gen.NewUsersClient(userConn)
	userRequest := &gen.GetUserByIdRequest{Id: uint32(friendId)}
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
	return userDto, err
}

// DeleteFriend implements service.FriendService.
func (s *friendServiceImp) DeleteFriend(ctx context.Context, userId uint, friendId uint) error {
	conn, err := grpc.Dial(s.grpcFriendUrl, grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		log.Fatalf("fail to dial: %v", err)
	}
	defer conn.Close()
	friendClient := gen.NewFriendClient(conn)
	deleteRequest := &gen.DeleteFriendRequest{UserId: uint32(userId), FriendId: uint32(friendId)}
	_, err = friendClient.DeleteFriend(ctx, deleteRequest)
	return err
}

// GetFriends implements service.FriendService.
func (s *friendServiceImp) GetFriends(ctx context.Context, userId uint) ([]dto.UserDto, error) {
	conn, err := grpc.Dial(s.grpcFriendUrl, grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		log.Fatalf("fail to dial: %v", err)
	}
	defer conn.Close()
	friendClient := gen.NewFriendClient(conn)
	getRequest := &gen.GetFriendIdsRequest{Id: uint32(userId)}
	friendsReply, err := friendClient.GetFriendIds(ctx, getRequest)
	if err != nil {
		return nil, err
	}

	userConn, err := grpc.Dial(s.grpcProfileUrl, grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		log.Fatalf("fail to dial: %v", err)
	}
	defer userConn.Close()
	userClient := gen.NewUsersClient(userConn)
	friends := make([]dto.UserDto, len(friendsReply.Ids))
	for ind, friendId := range friendsReply.Ids {
		userRequest := &gen.GetUserByIdRequest{Id: uint32(friendId)}
		user, err := userClient.GetUserById(ctx, userRequest)
		if err != nil {
			return nil, err
		}

		userDto := dto.UserDto{
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
		friends[ind] = userDto
	}

	return friends, err
}
