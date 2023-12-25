package dialoghandler

import (
	"socialnerworkapp.com/bff/internal/handler"
	"socialnerworkapp.com/bff/internal/service"
)

type dialogHandlerImp struct {
	service service.FriendService
}

func NewDialogHandler(service service.FriendService) handler.DialogHandler {
	return &dialogHandlerImp{service: service}
}
