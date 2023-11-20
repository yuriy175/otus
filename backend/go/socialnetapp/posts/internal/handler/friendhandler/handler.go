package friendhandler

import (
	"log"
	"net/http"
	"strconv"

	"github.com/gorilla/mux"
	commonhandler "socialnerworkapp.com/pkg/common/handler"
	"socialnerworkapp.com/posts/internal/handler"
	"socialnerworkapp.com/posts/internal/service"
)

type friendHandlerImp struct {
	service service.FriendService
}

func NewFriendHandler(service service.FriendService) handler.FriendHandler {
	return &friendHandlerImp{
		service: service}
}

// AddFriend godoc
// @Summary Add user's friend
// @Tags         friends
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
	token, err := commonhandler.GetAuthorizationToken(w, req)
	if err != nil {
		return
	}
	userId, err := commonhandler.GetAuthorizedUserId(w, req, token)
	if err != nil {
		return
	}

	err = h.service.UpsertFriend(ctx, userId, uint(friendId))

	if err != nil {
		log.Printf("Friend handler error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
	}
}

// DeleteFriend godoc
// @Summary Delete user's friend
// @Tags         friends
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
	token, err := commonhandler.GetAuthorizationToken(w, req)
	if err != nil {
		return
	}
	userId, err := commonhandler.GetAuthorizedUserId(w, req, token)
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
