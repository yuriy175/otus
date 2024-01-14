package dialoghandler

import (
	"encoding/json"
	"io"
	"log"
	"net/http"
	"strconv"

	"github.com/gorilla/mux"
	"socialnerworkapp.com/bff/internal/handler"
	"socialnerworkapp.com/bff/internal/service"
	commonhandler "socialnerworkapp.com/pkg/common/handler"
	"socialnerworkapp.com/pkg/trace"
)

type dialogsHandlerImp struct {
	service service.DialogsService
	tracer  trace.OtelTracer
}

func NewDialogHandler(tracer trace.OtelTracer, service service.DialogsService) handler.DialogsHandler {
	return &dialogsHandlerImp{service: service, tracer: tracer}
}

// GetDialogByUserId godoc
// @Summary Get dialog with user
// @Tags        dialogs
// @Security BearerAuth
// @Accept  json
// @Produce  json
// @Param user_id path string true "User id"
// @Success 200 {array} model.Message
// @Failure      500
// @Router /dialog/{user_id}/list [get]
func (h *dialogsHandlerImp) GetDialogByUserId(w http.ResponseWriter, req *http.Request) {

	ctx := req.Context()
	tracer := h.tracer.CreateTracer(req.RequestURI)
	ctx, endSpan := h.tracer.StartSpan(ctx, tracer, "GetDialogByUserId")
	defer endSpan()

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

	users, err := h.service.GetMessages(ctx, tracer, authorId, uint(userId))
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
// @Tags        dialogs
// @Security BearerAuth
// @Accept  json
// @Produce  json
// @Param user_id path string true "User id"
// @Param text body string true "Text"
// @Success 200
// @Failure      400
// @Failure      404
// @Failure      500
// @Router /dialog/{user_id}/send [post]
func (h *dialogsHandlerImp) SendMessageToUser(w http.ResponseWriter, req *http.Request) {
	ctx := req.Context()
	tracer := h.tracer.CreateTracer(req.RequestURI)
	ctx, endSpan := h.tracer.StartSpan(ctx, tracer, "SendMessageToUser")
	defer endSpan()

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

	m, err := h.service.CreateMessage(ctx, tracer, authorId, uint(userId), text)
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
	}

	if err := json.NewEncoder(w).Encode(m); err != nil {
		log.Printf("Dialogs handler error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
	}
}

// GetDialogBuddies godoc
// @Summary Get buddies
// @Tags         friends
// @Security BearerAuth
// @Accept  json
// @Produce  json
// @Success 200 {array} dto.UserDto
// @Failure      400
// @Failure      404
// @Failure      500
// @Router /buddies [get]
func (h *dialogsHandlerImp) GetDialogBuddies(w http.ResponseWriter, req *http.Request) {
	ctx := req.Context()
	tracer := h.tracer.CreateTracer(req.RequestURI)
	ctx, endSpan := h.tracer.StartSpan(ctx, tracer, "GetDialogBuddies")
	defer endSpan()

	userId, err := commonhandler.CheckAuthorizationAndGetUserId(w, req)
	if err != nil {
		return
	}

	buddies, err := h.service.GetDialogBuddies(ctx, tracer, userId)

	if err != nil {
		log.Printf("Dialogs handler error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
		return
	}
	if err := json.NewEncoder(w).Encode(buddies); err != nil {
		log.Printf("Dialogs handler error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
	}
}
