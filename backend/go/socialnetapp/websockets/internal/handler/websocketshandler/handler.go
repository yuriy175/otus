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
	feedPostsService service.WebsocketsService
	diaogsService    service.WebsocketsService
	upgrader         websocket.Upgrader
}

func NewWebsocketsHandler(
	feedPostsService service.WebsocketsService,
	diaogsService service.WebsocketsService) handler.WebsocketsHandler {
	upgrader := websocket.Upgrader{
		ReadBufferSize:  1024,
		WriteBufferSize: 1024,
	}
	upgrader.CheckOrigin = func(r *http.Request) bool { return true }

	return &wsHandlerImp{
		feedPostsService: feedPostsService,
		diaogsService:    diaogsService,
		upgrader:         upgrader,
	}
}

// SendPosts implements handler.WebsocketsHandler.
func (h *wsHandlerImp) SendPosts(w http.ResponseWriter, req *http.Request) {
	websocket, userId, err := h.preHandle(w, req)
	if err != nil {
		return
	}

	ctx := req.Context()
	h.feedPostsService.OnUserConnected(ctx, websocket, uint(userId))
}

func (h *wsHandlerImp) SendDialogMessages(w http.ResponseWriter, req *http.Request) {
	websocket, userId, err := h.preHandle(w, req)
	if err != nil {
		return
	}

	ctx := req.Context()
	h.diaogsService.OnUserConnected(ctx, websocket, uint(userId))
}

func (h *wsHandlerImp) preHandle(w http.ResponseWriter, req *http.Request) (*websocket.Conn, int, error) {
	websocket, err := h.upgrader.Upgrade(w, req, nil)
	if err != nil {
		log.Println(err)
		return nil, 0, err
	}
	log.Println("Websocket Connected!")
	tokens, ok := req.URL.Query()["token"]

	if !ok {
		log.Println("Url Param 'token' is missing")
		return nil, 0, err
	}
	token := commonservice.GetAuthorizedUserId(tokens[0])
	//protocols := strings.Split(req.Header.Get("Sec-WebSocket-Protocol"), ", ")
	//token := commonservice.GetAuthorizedUserId(protocols[1])
	if token == "" {
		return nil, 0, err
	}

	userId, _ := strconv.Atoi(token)

	return websocket, userId, nil
}
