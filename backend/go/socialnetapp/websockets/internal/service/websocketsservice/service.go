package websocketsservice

import (
	"fmt"
	"log"
	"net/http"
	"strconv"
	"sync"
	"time"

	"github.com/gorilla/websocket"
	commonservice "socialnerworkapp.com/pkg/common/service"
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

func (s *websocketsServiceImp) Start() {

	var upgrader = websocket.Upgrader{
		ReadBufferSize:  1024,
		WriteBufferSize: 1024,
	}

	http.HandleFunc("/post/feed", func(w http.ResponseWriter, r *http.Request) {
		upgrader.CheckOrigin = func(r *http.Request) bool { return true }
		websocket, err := upgrader.Upgrade(w, r, nil)
		if err != nil {
			log.Println(err)
			return
		}
		log.Println("Websocket Connected!")
		tokens, ok := r.URL.Query()["token"]

		if !ok {
			log.Println("Url Param 'token' is missing")
			return
		}
		token := commonservice.GetAuthorizedUserId(tokens[0])
		if token == "" {
			return
		}

		userId, _ := strconv.Atoi(token)

		ctx := r.Context()

		s.mqReceiver.CreateReceiver(ctx)
		go func() {
			friends, _ := s.repository.GetFriendIdsAsync(ctx, uint(userId))
			for _, v := range friends {
				s.mqReceiver.ReceivePosts(ctx, v, func(friendId uint, post string) {
					log.Println(post)
				})
			}
			s.listen(websocket, uint(userId))
		}()
	})
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
