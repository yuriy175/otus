package websocketshandler

import (
	"log"
	"net/http"
	"strconv"

	"github.com/gorilla/websocket"
	commonservice "socialnerworkapp.com/pkg/common/service"
	"socialnerworkapp.com/websockets/internal/handler"
	"socialnerworkapp.com/websockets/internal/service"
)

type wsHandlerImp struct {
	service  service.WebsocketsService
	upgrader websocket.Upgrader
}

func NewWebsocketsHandler(service service.WebsocketsService) handler.WebsocketsHandler {
	upgrader := websocket.Upgrader{
		ReadBufferSize:  1024,
		WriteBufferSize: 1024,
	}
	upgrader.CheckOrigin = func(r *http.Request) bool { return true }

	return &wsHandlerImp{
		service:  service,
		upgrader: upgrader,
	}
}

// SendPosts implements handler.WebsocketsHandler.
func (h *wsHandlerImp) SendPosts(w http.ResponseWriter, req *http.Request) {
	websocket, err := h.upgrader.Upgrade(w, req, nil)
	if err != nil {
		log.Println(err)
		return
	}
	log.Println("Websocket Connected!")
	tokens, ok := req.URL.Query()["token"]

	if !ok {
		log.Println("Url Param 'token' is missing")
		return
	}
	token := commonservice.GetAuthorizedUserId(tokens[0])
	if token == "" {
		return
	}

	userId, _ := strconv.Atoi(token)

	ctx := req.Context()
	h.service.OnUserConnected(ctx, websocket, uint(userId))
}
