package handler

import (
	"net/http"
)

type DialogsHandler interface {
	GetDialogByUserId(w http.ResponseWriter, req *http.Request)
	SendMessageToUser(w http.ResponseWriter, req *http.Request)
}
