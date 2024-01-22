package grpc

import (
	"context"

	"google.golang.org/protobuf/types/known/timestamppb"
	"socialnerworkapp.com/dialogs/internal/model"
	"socialnerworkapp.com/dialogs/internal/service"
	"socialnerworkapp.com/pkg/common/helpers"
	gen "socialnerworkapp.com/pkg/proto/gen"
)

type GrpcDialogsHandlerImp struct {
	gen.UnimplementedDialogServer
	service service.DialogsService
}

func (*GrpcDialogsHandlerImp) mustEmbedUnimplementedDialogServer() {
	panic("unimplemented")
}

func NewGrpcDialogsHandler(service service.DialogsService) *GrpcDialogsHandlerImp {
	return &GrpcDialogsHandlerImp{service: service}
}

func (h *GrpcDialogsHandlerImp) GetMessages(ctx context.Context, r *gen.GetMessagesRequest) (*gen.GetMessagesReply, error) {
	msgs, _ := h.service.GetMessages(ctx, uint(r.UserId), uint(r.AuthorId))
	out := make([]*gen.MessageReply, len(msgs))
	for i, msg := range msgs {
		out[i] = h.toMessageReply(&msg)
	}

	return &gen.GetMessagesReply{Messages: out}, nil
}

func (h *GrpcDialogsHandlerImp) CreateMessage(ctx context.Context, r *gen.CreateMessageRequest) (*gen.MessageReply, error) {
	msg, _ := h.service.CreateMessage(ctx, uint(r.AuthorId), uint(r.UserId), r.Text)
	return h.toMessageReply(msg), nil
}

func (h *GrpcDialogsHandlerImp) GetBuddyIds(ctx context.Context, r *gen.GetBuddyIdsRequest) (*gen.GetBuddyIdsReply, error) {
	buddyIds, err := h.service.GetBuddyIds(ctx, uint(r.Id))
	if err != nil {
		return nil, err
	}
	reply := &gen.GetBuddyIdsReply{
		Ids: helpers.ConvertInts[uint32](buddyIds),
	}
	return reply, nil
}

func (h *GrpcDialogsHandlerImp) SetUnreadMessagesFromUser(ctx context.Context, r *gen.SetUnreadMessagesFromUserRequest) (*gen.SetUnreadMessagesFromUserReply, error) {
	count, err := h.service.SetUnreadMessagesFromUser(ctx, uint(r.AuthorId), uint(r.UserId))
	return &gen.SetUnreadMessagesFromUserReply{
		Count: uint32(count),
	}, err
}

func (h *GrpcDialogsHandlerImp) toMessageReply(msg *model.Message) *gen.MessageReply {
	created := msg.Created
	timestamp := timestamppb.Timestamp{
		Seconds: created.Unix(),
		Nanos:   int32(created.Nanosecond()),
	}
	reply := &gen.MessageReply{
		Id:       uint32(msg.Id),
		UserId:   uint32(msg.UserID),
		AuthorId: uint32(msg.AuthorId),
		Text:     msg.Text,
		Created:  &timestamp}

	return reply
}
