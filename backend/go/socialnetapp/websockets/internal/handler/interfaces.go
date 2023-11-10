package handler

import (
	"net/http"
)

type WebsocketsHandler interface {
	SendPosts(w http.ResponseWriter, req *http.Request)
}
