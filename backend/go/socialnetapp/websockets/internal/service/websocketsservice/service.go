package websocketsservice

import (
	"context"
	"fmt"
	"log"
	"sync"
	"time"

	"github.com/gorilla/websocket"
	"socialnerworkapp.com/pkg/mq"
	"socialnerworkapp.com/websockets/internal/repository"
	"socialnerworkapp.com/websockets/internal/service"
)

type websocketsServiceImp struct {
	repository repository.FriendRepository
	mqReceiver mq.MqReceiver

	mtx sync.RWMutex
}

func NewWebsocketsService(
	repository repository.FriendRepository,
	mqReceiver mq.MqReceiver) service.WebsocketsService {
	return &websocketsServiceImp{
		repository: repository,
		mqReceiver: mqReceiver}
}

func (s *websocketsServiceImp) OnUserConnected(ctx context.Context, conn *websocket.Conn, userId uint) {
	go func() {
		s.mqReceiver.CreateReceiver(ctx)
		friends, _ := s.repository.GetFriendIdsAsync(ctx, uint(userId))
		for _, v := range friends {
			s.mqReceiver.ReceivePosts(ctx, v, func(friendId uint, post string) {
				log.Println(post)
			})
		}
		s.listen(conn, uint(userId))
	}()
}

func (s *websocketsServiceImp) listen(conn *websocket.Conn, userId uint) {
	for {
		// read a message
		messageType, messageContent, err := conn.ReadMessage()
		timeReceive := time.Now()
		if err != nil {
			log.Println(err)
			return
		}

		// print out that message
		fmt.Println(string(messageContent))

		// reponse message
		messageResponse := fmt.Sprintf("Your message is: %s. Time received : %v from %v !!!", messageContent, timeReceive, userId)

		if err := conn.WriteMessage(messageType, []byte(messageResponse)); err != nil {
			log.Println(err)
			return
		}

	}
}
