package counterhandler

import (
	"encoding/json"
	"log"
	"net/http"

	"socialnerworkapp.com/bff/internal/handler"
	"socialnerworkapp.com/bff/internal/service"
	commonhandler "socialnerworkapp.com/pkg/common/handler"
	"socialnerworkapp.com/pkg/trace"
)

type countersHandlerImp struct {
	service service.CounterService
	tracer  trace.OtelTracer
}

func NewCounterHandler(tracer trace.OtelTracer, service service.CounterService) handler.CountersHandler {
	return &countersHandlerImp{service: service, tracer: tracer}
}

// GetUnReadCounterByUserId godoc
// @Summary Get unread dialog messages count by user
// @Tags        counters
// @Security BearerAuth
// @Accept  json
// @Produce  json
// @Success 200 {uint} uint
// @Failure      500
// @Router /unread/count
func (h *countersHandlerImp) GetUnReadCounterByUserId(w http.ResponseWriter, req *http.Request) {

	ctx := req.Context()
	tracer := h.tracer.CreateTracer(req.RequestURI)
	ctx, endSpan := h.tracer.StartSpan(ctx, tracer, "GetUnReadCounterByUserId")
	defer endSpan()

	userId, err := commonhandler.CheckAuthorizationAndGetUserId(w, req)
	if err != nil {
		return
	}

	users, err := h.service.GetUnReadCounterByUserId(ctx, tracer, userId)
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
