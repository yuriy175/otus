package authhandler

import (
	"encoding/json"
	"log"
	"net/http"

	commonhandler "socialnerworkapp.com/pkg/common/handler"
	"socialnerworkapp.com/profile/internal/handler"
	"socialnerworkapp.com/profile/internal/service"
)

type authHandlerImp struct {
	service service.AuthService
}

func NewAuthHandler(service service.AuthService) handler.AuthHandler {
	return &authHandlerImp{service: service}
}

// Login godoc
// @Summary Login user
// @Tags         auth
// @Accept  json
// @Param properties body dto.LoginDto true "Login properties"
// @Success 200 {object} string
// @Failure      400
// @Failure      500
// @Router /login [post]
func (h *authHandlerImp) Login(w http.ResponseWriter, req *http.Request) {
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

	token, err := h.service.Login(ctx, id, password)
	if err != nil {
		log.Printf("Auth service error: %v\n", err)
		w.WriteHeader(http.StatusBadRequest)
		return
	}
	u := struct {
		Token string `json:"token"`
	}{Token: token}
	if err := json.NewEncoder(w).Encode(u); err != nil {
		log.Printf("Auth service error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
	}
}
