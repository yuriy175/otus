package handler

import (
	"net/http"
)

type UserHandler interface {
	GetUsers(w http.ResponseWriter, req *http.Request)
	GetUserById(w http.ResponseWriter, req *http.Request)
	RegisterUser(w http.ResponseWriter, req *http.Request)
}

type AuthHandler interface {
	Login(w http.ResponseWriter, req *http.Request)
}
