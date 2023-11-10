package service

import (
	"context"

	"github.com/gorilla/websocket"
)

type WebsocketsService interface {
	OnUserConnected(ctx context.Context, conn *websocket.Conn, userId uint)
}
