package handler

import (
	"net/http"
)

type UserHandler interface {
	GetUsers(w http.ResponseWriter, req *http.Request)
	GetUserById(w http.ResponseWriter, req *http.Request)
	RegisterUser(w http.ResponseWriter, req *http.Request)
	FindUser(w http.ResponseWriter, req *http.Request)
}

type AuthHandler interface {
	Login(w http.ResponseWriter, req *http.Request)
}

type FriendHandler interface {
	AddFriend(w http.ResponseWriter, req *http.Request)
	DeleteFriend(w http.ResponseWriter, req *http.Request)
}

type PostHandler interface {
	CreatePost(w http.ResponseWriter, req *http.Request)
	UpdatePost(w http.ResponseWriter, req *http.Request)
	GetPost(w http.ResponseWriter, req *http.Request)
	DeletePost(w http.ResponseWriter, req *http.Request)
	FeedPosts(w http.ResponseWriter, req *http.Request)
}
