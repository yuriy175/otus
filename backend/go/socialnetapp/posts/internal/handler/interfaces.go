package handler

import (
	"net/http"
)

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
