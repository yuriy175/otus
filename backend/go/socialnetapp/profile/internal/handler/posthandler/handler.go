package posthandler

import (
	"log"
	"net/http"
	"strconv"

	"github.com/gorilla/mux"
	"socialnerworkapp.com/profile/internal/handler"
	"socialnerworkapp.com/profile/internal/handler/handlerutils"
	"socialnerworkapp.com/profile/internal/service"
)

type postHandlerImp struct {
	authService service.AuthService
	service     service.PostService
}

func NewPostHandler(authService service.AuthService, service service.PostService) handler.PostHandler {
	return &postHandlerImp{
		authService: authService,
		service:     service}
}

// CreatePost godoc
// @Summary Add user's post
// @Tags         posts
// @Security BearerAuth
// @Accept  json
// @Produce  json
// @Param trxt body string true "Text"
// @Success 200
// @Failure      400
// @Failure      404
// @Failure      500
// @Router /post/create [post]
func (h *postHandlerImp) CreatePost(w http.ResponseWriter, req *http.Request) {
	ctx := req.Context()

	userId, err := handlerutils.CheckAuthorizationAndGetUserId(w, req, h.authService)
	if err != nil {
		return
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
	vars := mux.Vars(req)
	friendId, err := strconv.Atoi(vars["user_id"])
	if err != nil {
		log.Printf("Friend handler error: %v\n", err)
		w.WriteHeader(http.StatusBadRequest)
	}
	token, err := handlerutils.GetAuthorizationToken(w, req)
	if err != nil {
		return
	}
	userId, err := handlerutils.GetAuthorizedUserId(w, req, token, h.authService)
	if err != nil {
		return
	}

	err = h.service.DeleteFriend(ctx, userId, uint(friendId))

	if err != nil {
		log.Printf("Friend handler error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
	}
}
