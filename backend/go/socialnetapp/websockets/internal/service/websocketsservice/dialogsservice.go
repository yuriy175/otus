package websocketsservice

import (
	"context"
	"encoding/json"
	"log"
	"sync"

	"github.com/gorilla/websocket"
	"socialnerworkapp.com/pkg/mq"
	"socialnerworkapp.com/websockets/internal/model"
	"socialnerworkapp.com/websockets/internal/repository"
	"socialnerworkapp.com/websockets/internal/service"
)

type dlgWebsocketsServiceImp struct {
	repository repository.FriendRepository
	mqReceiver mq.MqReceiver
	mqSender   mq.MqSender

	mtx        sync.RWMutex
	websockets map[uint]*websocket.Conn
}

func NewDialogssWebsocketsService(
	repository repository.FriendRepository,
	mqReceiver mq.MqReceiver,
	mqSender mq.MqSender) service.WebsocketsService {

	srv := &dlgWebsocketsServiceImp{
		repository: repository,
		mqReceiver: mqReceiver,
		mqSender:   mqSender,
		websockets: make(map[uint]*websocket.Conn)}
	mqReceiver.CreateDialogReceiver(func(data []byte) {
		message := &model.Message{}
		if err := json.Unmarshal(data, message); err != nil {
			log.Println(err)
			return
		}

		srv.mtx.Lock()
		ws := srv.websockets[message.UserID]
		if ws == nil {
			return
		}
		if err := ws.WriteMessage(websocket.TextMessage, data); err != nil {
			log.Println(err)
		}

		srv.mtx.Unlock()
	})

	return srv
}

func (s *dlgWebsocketsServiceImp) OnUserConnected(ctx context.Context, conn *websocket.Conn, userId uint) {
	go func() {
		s.mtx.Lock()
		s.websockets[userId] = conn
		s.mtx.Unlock()
	}()
}
