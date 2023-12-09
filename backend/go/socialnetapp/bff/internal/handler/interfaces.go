package handler

import (
	"net/http"
)

type UserHandler interface {
	Login(w http.ResponseWriter, req *http.Request)
}
