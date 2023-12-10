package friendhandler

import (
	"encoding/json"
	"log"
	"net/http"
	"strconv"

	"github.com/gorilla/mux"
	"socialnerworkapp.com/bff/internal/handler"
	"socialnerworkapp.com/bff/internal/service"
	commonhandler "socialnerworkapp.com/pkg/common/handler"
)

type friendHandlerImp struct {
	service service.FriendService
}

func NewFriendHandler(service service.FriendService) handler.FriendHandler {
	return &friendHandlerImp{service: service}
}

// AddFriend godoc
// @Summary Add user's friend
// @Tags        friends
// @Security BearerAuth
// @Accept  json
// @Produce  json
// @Param user_id path string true "Friend id"
// @Success 200
// @Failure      400
// @Failure      404
// @Failure      500
// @Router /friend/set/{user_id} [put]
func (h *friendHandlerImp) AddFriend(w http.ResponseWriter, req *http.Request) {
	ctx := req.Context()
	vars := mux.Vars(req)
	friendId, err := strconv.Atoi(vars["user_id"])
	if err != nil {
		log.Printf("Friend handler error: %v\n", err)
		w.WriteHeader(http.StatusBadRequest)
	}
	userId, err := commonhandler.CheckAuthorizationAndGetUserId(w, req)
	if err != nil {
		return
	}

	userDto, err := h.service.AddFriend(ctx, userId, uint(friendId))

	if err != nil {
		log.Printf("Friend handler error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
	}
	if err := json.NewEncoder(w).Encode(userDto); err != nil {
		log.Printf("Friend service error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
	}
}

// DeleteFriend godoc
// @Summary Delete user's friend
// @Tags        friends
// @Security BearerAuth
// @Accept  json
// @Produce  json
// @Param user_id path string true "Friend id"
// @Success 200
// @Failure      400
// @Failure      404
// @Failure      500
// @Router /friend/delete/{user_id} [delete]
func (h *friendHandlerImp) DeleteFriend(w http.ResponseWriter, req *http.Request) {
	ctx := req.Context()
	userId, err := commonhandler.CheckAuthorizationAndGetUserId(w, req)
	if err != nil {
		return
	}

	vars := mux.Vars(req)
	friendId, err := strconv.Atoi(vars["user_id"])
	if err != nil {
		log.Printf("Friend handler error: %v\n", err)
		w.WriteHeader(http.StatusBadRequest)
	}

	err = h.service.DeleteFriend(ctx, userId, uint(friendId))

	if err != nil {
		log.Printf("Friend handler error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
	}
}

// GetFriends godoc
// @Summary Get friends post
// @Tags         posts
// @Security BearerAuth
// @Accept  json
// @Produce  json
// @Success 200 {array} dto.UserDto
// @Failure      400
// @Failure      404
// @Failure      500
// @Router /friends [get]
func (h *friendHandlerImp) GetFriends(w http.ResponseWriter, req *http.Request) {
	userId, err := commonhandler.CheckAuthorizationAndGetUserId(w, req)
	if err != nil {
		return
	}

	ctx := req.Context()
	friends, err := h.service.GetFriends(ctx, userId)

	if err != nil {
		log.Printf("Friend handler error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
		return
	}
	if err := json.NewEncoder(w).Encode(friends); err != nil {
		log.Printf("Friend handler error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
	}
}
