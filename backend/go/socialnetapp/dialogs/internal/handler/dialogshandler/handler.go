package dialogshandler

import (
	"encoding/json"
	"io"
	"log"
	"net/http"
	"strconv"

	"github.com/gorilla/mux"
	"socialnerworkapp.com/dialogs/internal/handler"
	"socialnerworkapp.com/dialogs/internal/service"
	commonhandler "socialnerworkapp.com/pkg/common/handler"
)

type dialogsHandlerImp struct {
	service service.DialogsService
}

func NewDialogsHandler(service service.DialogsService) handler.DialogsHandler {
	return &dialogsHandlerImp{service: service}
}

// GetDialogByUserId godoc
// @Summary Get dialog with user
// @Tags        obsolete dialogs
// @Security BearerAuth
// @Accept  json
// @Produce  json
// @Param user_id path string true "User id"
// @Success 200 {array} model.Message
// @Failure      500
// // @Router /dialog/{user_id}/list [get]
func (h *dialogsHandlerImp) GetDialogByUserId(w http.ResponseWriter, req *http.Request) {
	userId1, err := commonhandler.CheckAuthorizationAndGetUserId(w, req)
	if err != nil {
		return
	}

	ctx := req.Context()

	vars := mux.Vars(req)
	userId2, err := strconv.Atoi(vars["user_id"])
	if err != nil {
		log.Printf("Dialogs handler error: %v\n", err)
		w.WriteHeader(http.StatusBadRequest)
	}

	users, err := h.service.GetMessages(ctx, userId1, uint(userId2))
	if err != nil {
		log.Printf("Dialogs handler error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
		return
	}
	if err := json.NewEncoder(w).Encode(users); err != nil {
		log.Printf("Dialogs handler error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
	}
}

// SendMessageToUser godoc
// @Summary Send message to user
// @Tags        obsolete dialogs
// @Security BearerAuth
// @Accept  json
// @Produce  json
// @Param user_id path string true "User id"
// @Param text body string true "Text"
// @Success 200
// @Failure      400
// @Failure      404
// @Failure      500
// // @Router /dialog/{user_id}/send [post]
func (h *dialogsHandlerImp) SendMessageToUser(w http.ResponseWriter, req *http.Request) {
	ctx := req.Context()

	authorId, err := commonhandler.CheckAuthorizationAndGetUserId(w, req)
	if err != nil {
		return
	}

	vars := mux.Vars(req)
	userId, err := strconv.Atoi(vars["user_id"])
	if err != nil {
		log.Printf("Dialogs handler error: %v\n", err)
		w.WriteHeader(http.StatusBadRequest)
	}

	body, err := io.ReadAll(req.Body)
	text := string(body)
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	m, err := h.service.CreateMessage(ctx, authorId, uint(userId), text)
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
	}

	if err := json.NewEncoder(w).Encode(m); err != nil {
		log.Printf("Dialogs handler error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
	}
}
