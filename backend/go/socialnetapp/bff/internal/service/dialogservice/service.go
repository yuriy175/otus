package dialogservice

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

type dialogServiceImp struct {
	grpcDialogUrl  string
	grpcProfileUrl string
}

func NewDialogService() service.DialogsService {
	grpcDialogUrl, _ := os.LookupEnv("GRPC_DIALOGS")
	grpcProfileUrl, _ := os.LookupEnv("GRPC_PROFILE")
	return &dialogServiceImp{grpcDialogUrl: grpcDialogUrl, grpcProfileUrl: grpcProfileUrl}
}

// GetFriends implements service.FriendService.
func (s *dialogServiceImp) GetDialogBuddies(ctx context.Context, userId uint) ([]dto.UserDto, error) {
	conn, err := grpc.Dial(s.grpcDialogUrl, grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		log.Fatalf("fail to dial: %v", err)
	}
	defer conn.Close()
	dialogClient := gen.NewDialogClient(conn)
	getRequest := &gen.GetBuddyIdsRequest{Id: uint32(userId)}
	dialogReply, err := dialogClient.GetBuddyIds(ctx, getRequest)
	if err != nil {
		return nil, err
	}

	userConn, err := grpc.Dial(s.grpcProfileUrl, grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		log.Fatalf("fail to dial: %v", err)
	}
	defer userConn.Close()
	userClient := gen.NewUsersClient(userConn)
	buddies := make([]dto.UserDto, len(dialogReply.Ids))
	for ind, friendId := range dialogReply.Ids {
		userRequest := &gen.GetUserByIdRequest{Id: uint32(friendId)}
		user, err := userClient.GetUserById(ctx, userRequest)
		if err != nil {
			return nil, err
		}

		userDto := service.ConvertToUserDto(user)
		buddies[ind] = *userDto
	}

	return buddies, err
}
