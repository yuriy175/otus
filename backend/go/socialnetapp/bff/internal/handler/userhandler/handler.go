package userhandler

import (
	"encoding/json"
	"log"
	"net/http"

	"socialnerworkapp.com/bff/internal/handler"
	"socialnerworkapp.com/bff/internal/service"
	commonhandler "socialnerworkapp.com/pkg/common/handler"
)

type userHandlerImp struct {
	service service.UserService
}

func NewUserHandler(service service.UserService) handler.UserHandler {
	return &userHandlerImp{service: service}
}

// Login godoc
// @Summary Login user
// @Tags         users
// @Accept  json
// @Param properties body dto.LoginUserDto true "Login properties"
// @Success 200 {object} dto.LoggedinUserDto
// @Failure      400
// @Failure      500
// @Router /login [post]
func (h *userHandlerImp) Login(w http.ResponseWriter, req *http.Request) {
	payload := make(map[string]interface{})
	err := json.NewDecoder(req.Body).Decode(&payload)
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}
	ctx := req.Context()
	p, ok := commonhandler.CheckParam(w, payload, "id", "Auth repository error: id\n")
	if !ok {
		return
	}
	id := uint(p.(float64))

	p, ok = commonhandler.CheckParam(w, payload, "password", "Auth repository error: password\n")
	if !ok {
		return
	}
	password := p.(string)
	userDto, err := h.service.Login(ctx, id, password)
	if err != nil {
		log.Printf("Auth service error: %v\n", err)
		w.WriteHeader(http.StatusBadRequest)
		return
	}
	if err := json.NewEncoder(w).Encode(userDto); err != nil {
		log.Printf("Auth service error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
	}
}
