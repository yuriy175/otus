package websocketsservice

import (
	"context"
	"encoding/json"
	"log"
	"os"
	"strconv"
	"sync"

	"github.com/gorilla/websocket"
	"socialnerworkapp.com/pkg/mq"
	"socialnerworkapp.com/websockets/internal/cache"
	"socialnerworkapp.com/websockets/internal/model"
	"socialnerworkapp.com/websockets/internal/repository"
	"socialnerworkapp.com/websockets/internal/service"

	mqtypes "socialnerworkapp.com/pkg/mq/types"
)

type webSockImp struct {
	//uint? BuddyId, WebSocket Socket, Task? Receiver
	buddyId   uint
	websocket *websocket.Conn
}

type dlgWebsocketsServiceImp struct {
	repository repository.FriendRepository
	mqReceiver mq.MqReceiver
	mqSender   mq.MqSender
	cacheSrv   cache.CacheService

	mtx           sync.RWMutex
	websockets    map[uint]*webSockImp
	wsServicePort string
	wsServiceName string
}

func NewDialogssWebsocketsService(
	repository repository.FriendRepository,
	mqReceiver mq.MqReceiver,
	mqSender mq.MqSender,
	cacheSrv cache.CacheService) service.WebsocketsService {

	wsServicePort, _ := os.LookupEnv("REST_PORT")
	wsServiceName, _ := os.Hostname()
	srv := &dlgWebsocketsServiceImp{
		repository:    repository,
		mqReceiver:    mqReceiver,
		mqSender:      mqSender,
		cacheSrv:      cacheSrv,
		wsServiceName: wsServiceName,
		wsServicePort: wsServicePort,
		websockets:    make(map[uint]*webSockImp)}

	mqReceiver.NewDialogMessageReceiver(
		wsServiceName+"_"+wsServicePort,
		func(data []byte) {
			ctx := context.Background()
			message := &model.Message{}
			if err := json.Unmarshal(data, message); err != nil {
				log.Println(err)
				return
			}

			buddyId := message.UserId
			authorId := message.AuthorId
			srv.mtx.Lock()
			defer srv.mtx.Unlock()
			webSock, ok := srv.websockets[buddyId]

			unreadMessage := &mqtypes.UnreadCountMessage{
				MessageHeader:    mqtypes.MessageHeader{MessageType: mq.UpdateUnreadDialogMessages},
				UserId:           buddyId,
				IsIncrement:      true,
				UnreadMessageIds: []int{int(message.Id)},
			}
			bytes, err := json.Marshal(unreadMessage)
			if err != nil {
				return
			}

			if !ok || webSock == nil || webSock.websocket == nil {
				srv.mqSender.SendUnreadDialogMessageIds(ctx, bytes)
				return
			}

			ws := webSock.websocket
			if authorId == webSock.buddyId {
				if err := ws.WriteMessage(websocket.TextMessage, data); err != nil {
					log.Println(err)
				}
			} else {
				srv.mqSender.SendUnreadDialogMessageIds(ctx, bytes)
			}
		})

	return srv
}

func (s *dlgWebsocketsServiceImp) OnUserConnected(ctx context.Context, conn *websocket.Conn, userId uint) {
	go func() {
		s.mtx.Lock()
		defer s.mtx.Unlock()
		webSock := &webSockImp{
			buddyId:   0,
			websocket: conn,
		}
		go func(ws *websocket.Conn) {
			for {
				_, data, err := ws.ReadMessage()
				if _, ok := err.(*websocket.CloseError); ok {
					return
				}

				if err == nil {
					buddyId, err := strconv.Atoi(string(data))
					if err != nil {
						continue
					}
					webSock.buddyId = uint(buddyId)
				}
			}
		}(conn)

		s.websockets[userId] = webSock
		s.cacheSrv.UpsertUserWebSocket(ctx, userId, s.wsServiceName, s.wsServicePort)
	}()
}
