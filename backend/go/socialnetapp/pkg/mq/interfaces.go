package mq

import (
	"context"
)

type MqSender interface {
	SendPost(_ context.Context, userId uint, post string) error
	SendNewDialogMessage(_ context.Context, queuePostfix string, data []byte) error
	SendUnreadDialogMessageIds(_ context.Context, data []byte) error
	SendUnreadDialogMessageIdsFailed(_ context.Context, data []byte) error
}

type MqReceiver interface {
	CreateReceiver(_ context.Context) error
	ReceivePosts(_ context.Context, userId uint, action func(friendId uint, post string)) error
	CloseReceiver(_ context.Context) error
	NewDialogMessageReceiver(queuePostfix string, action func(data []byte)) error
	CreateUnreadDialogMessagesCountReceiver(action func(data []byte)) error
	CreateUnreadDialogMessagesCountFailedReceiver(action func(data []byte)) error
}
