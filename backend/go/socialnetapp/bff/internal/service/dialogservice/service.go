package dialogservice

import (
	"context"
	"log"
	"os"

	_ "github.com/google/uuid"
	oteltrace "go.opentelemetry.io/otel/trace"
	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials/insecure"
	"socialnerworkapp.com/bff/internal/handler/dto"
	"socialnerworkapp.com/bff/internal/service"
	"socialnerworkapp.com/pkg/proto/gen"
	"socialnerworkapp.com/pkg/trace"
)

type dialogServiceImp struct {
	grpcDialogUrl  string
	grpcProfileUrl string
	tracer         trace.OtelTracer
}

func NewDialogService(tracer trace.OtelTracer) service.DialogsService {
	grpcDialogUrl, _ := os.LookupEnv("GRPC_DIALOGS")
	grpcProfileUrl, _ := os.LookupEnv("GRPC_PROFILE")
	return &dialogServiceImp{
		grpcDialogUrl:  grpcDialogUrl,
		grpcProfileUrl: grpcProfileUrl,
		tracer:         tracer}
}

// GetFriends implements service.FriendService.
func (s *dialogServiceImp) GetDialogBuddies(ctx context.Context, tracer oteltrace.Tracer, userId uint) ([]dto.UserDto, error) {
	conn, err := grpc.Dial(s.grpcDialogUrl, grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		log.Fatalf("fail to dial: %v", err)
	}
	defer conn.Close()
	ctx, endSpan := s.tracer.StartSpan(ctx, tracer, "dialogClient.GetBuddyIds")

	dialogClient := gen.NewDialogClient(conn)
	getRequest := &gen.GetBuddyIdsRequest{Id: uint32(userId)}
	dialogReply, err := dialogClient.GetBuddyIds(ctx, getRequest)
	endSpan()
	if err != nil {
		return nil, err
	}

	userConn, err := grpc.Dial(s.grpcProfileUrl, grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		log.Fatalf("fail to dial: %v", err)
	}
	defer userConn.Close()

	ctx, endSpan = s.tracer.StartSpan(ctx, tracer, "userClient.GetUserById")
	defer endSpan()
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

// CreateMessage implements service.DialogsService.
func (s *dialogServiceImp) CreateMessage(ctx context.Context, tracer oteltrace.Tracer, authorId uint, userId uint, text string) (*dto.MessageDto, error) {
	conn, err := grpc.Dial(s.grpcDialogUrl, grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		log.Fatalf("fail to dial: %v", err)
	}
	defer conn.Close()
	ctx, endSpan := s.tracer.StartSpan(ctx, tracer, "dialogClient.CreateMessage")
	defer endSpan()
	dialogClient := gen.NewDialogClient(conn)
	getRequest := &gen.CreateMessageRequest{
		AuthorId: uint32(authorId),
		UserId:   uint32(userId),
		Text:     text,
	}
	reply, err := dialogClient.CreateMessage(ctx, getRequest)
	if err != nil {
		return nil, err
	}

	return s.convertToMessageDto(reply), nil
}

// GetMessages implements service.DialogsService.
func (s *dialogServiceImp) GetMessages(ctx context.Context, tracer oteltrace.Tracer, authorId uint, userId uint) (*dto.UserMessagesDto, error) {
	conn, err := grpc.Dial(s.grpcDialogUrl, grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		log.Fatalf("fail to dial: %v", err)
	}
	defer conn.Close()
	ctx, endSpan := s.tracer.StartSpan(ctx, tracer, "dialogClient.GetMessages")

	dialogClient := gen.NewDialogClient(conn)
	getRequest := &gen.GetMessagesRequest{
		UserId:   uint32(userId),
		AuthorId: uint32(authorId)}
	dialogReply, err := dialogClient.GetMessages(ctx, getRequest)
	go dialogClient.SetUnreadMessagesFromUser(ctx,
		&gen.SetUnreadMessagesFromUserRequest{
			AuthorId: uint32(userId),
			UserId:   uint32(authorId)})

	endSpan()
	if err != nil {
		return nil, err
	}

	userConn, err := grpc.Dial(s.grpcProfileUrl, grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		return nil, err
	}
	defer userConn.Close()
	ctx, endSpan = s.tracer.StartSpan(ctx, tracer, "userClient.GetUserById")
	userClient := gen.NewUsersClient(userConn)
	userRequest := &gen.GetUserByIdRequest{Id: uint32(userId)}
	user, err := userClient.GetUserById(ctx, userRequest)
	endSpan()
	if err != nil {
		return nil, err
	}

	messages := make([]*dto.MessageDto, len(dialogReply.Messages))
	for ind, message := range dialogReply.Messages {
		messageDto := s.convertToMessageDto(message)
		messages[ind] = messageDto
	}

	return &dto.UserMessagesDto{
		User:     service.ConvertToUserDto(user),
		Messages: messages,
	}, err
}

func (s *dialogServiceImp) convertToMessageDto(message *gen.MessageReply) *dto.MessageDto {
	messageDto := &dto.MessageDto{
		ID:       uint(message.Id),
		UserId:   uint(message.UserId),
		AuthorId: uint(message.AuthorId),
		Message:  message.Text,
		Created:  service.ConvertToTime(message.Created),
	}

	return messageDto
}
