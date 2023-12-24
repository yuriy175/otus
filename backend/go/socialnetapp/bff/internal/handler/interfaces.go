package handler

import (
	"net/http"
)

type UserHandler interface {
	Login(w http.ResponseWriter, req *http.Request)
	RegisterUser(w http.ResponseWriter, req *http.Request)
	FindUser(w http.ResponseWriter, req *http.Request)
}

type FriendHandler interface {
	AddFriend(w http.ResponseWriter, req *http.Request)
	DeleteFriend(w http.ResponseWriter, req *http.Request)
	GetFriends(w http.ResponseWriter, req *http.Request)
}

type PostHandler interface {
	CreatePost(w http.ResponseWriter, req *http.Request)
	FeedPosts(w http.ResponseWriter, req *http.Request)
}
