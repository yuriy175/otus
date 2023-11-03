package mq

import "context"

type mqReceiverImp struct {
}

func NewMqReceiver() MqReceiver {
	return &mqReceiverImp{}
}

func (s *mqReceiverImp) CreateReceiver(_ context.Context) error {
	return nil
}

func (s *mqReceiverImp) ReceivePosts(_ context.Context, userId uint, action func(friendId uint, post string)) error {
	return nil
}
