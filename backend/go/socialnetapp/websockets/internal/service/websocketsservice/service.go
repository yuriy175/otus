package websocketsservice

import (
	"context"
	"log"
	"sync"

	"github.com/gorilla/websocket"
	"socialnerworkapp.com/pkg/mq"
	"socialnerworkapp.com/websockets/internal/repository"
	"socialnerworkapp.com/websockets/internal/service"
)

type websocketsServiceImp struct {
	repository repository.FriendRepository
	mqReceiver mq.MqReceiver

	mtx        sync.RWMutex
	websockets map[uint][]*websocket.Conn
}

func NewWebsocketsService(
	repository repository.FriendRepository,
	mqReceiver mq.MqReceiver) service.WebsocketsService {
	return &websocketsServiceImp{
		repository: repository,
		mqReceiver: mqReceiver,
		websockets: make(map[uint][]*websocket.Conn)}
}

func (s *websocketsServiceImp) OnUserConnected(ctx context.Context, conn *websocket.Conn, userId uint) {
	go func() {
		s.mqReceiver.CreateReceiver(ctx)
		friends, _ := s.repository.GetFriendIdsAsync(ctx, uint(userId))
		for _, friendId := range friends {
			s.mtx.Lock()
			conns, ok := s.websockets[friendId]
			if !ok {
				conns = make([]*websocket.Conn, 1)
				conns[0] = conn
			} else {
				conns = append(conns, conn)
			}
			s.websockets[friendId] = conns
			s.mtx.Unlock()
			s.mqReceiver.ReceivePosts(ctx, friendId, func(friendId uint, post string) {
				websockets := s.websockets[friendId]
				for _, ws := range websockets {
					s.mtx.Lock()
					if err := ws.WriteMessage(websocket.TextMessage, []byte(post)); err != nil {
						log.Println(err)
					}

					s.mtx.Unlock()
				}
			})
		}
	}()
}
