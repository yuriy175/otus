package mq

import (
	"context"
)

type MqSender interface {
	SendPost(_ context.Context, userId uint, post string) error
	SendDialogMessage(_ context.Context, data []byte) error
}

type MqReceiver interface {
	CreateReceiver(_ context.Context) error
	ReceivePosts(_ context.Context, userId uint, action func(friendId uint, post string)) error
	CloseReceiver(_ context.Context) error
	CreateDialogReceiver(action func(data []byte)) error
}
