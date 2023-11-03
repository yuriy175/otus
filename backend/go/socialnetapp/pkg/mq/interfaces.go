package mq

import (
	"context"
)

type MqSender interface {
	SendPost(_ context.Context, userId uint, post string) error
}

type MqReceiver interface {
	CreateReceiver(_ context.Context) error
	ReceivePosts(_ context.Context, userId uint, action func(friendId uint, post string)) error
}
