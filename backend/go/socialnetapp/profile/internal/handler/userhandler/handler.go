package userhandler

import (
	"encoding/json"
	"log"
	"net/http"
	"strconv"

	"github.com/gorilla/mux"
	"socialnerworkapp.com/profile/internal/handler"
	"socialnerworkapp.com/profile/internal/model"
	"socialnerworkapp.com/profile/internal/service"
)

type userHandlerImp struct {
	service service.UserService
}

func NewUserHandler(service service.UserService) handler.UserHandler {
	return &userHandlerImp{service: service}
}

// GetUsers godoc
// @Summary Get all users
// @Tags         users
// @Accept  json
// @Produce  json
// @Success 200 {array} model.User
// @Failure      500
// @Router /users [get]
func (h *userHandlerImp) GetUsers(w http.ResponseWriter, req *http.Request) {
	ctx := req.Context()
	users, err := h.service.GetUsers(ctx)
	if err != nil {
		log.Printf("User repository error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
		return
	}
	if err := json.NewEncoder(w).Encode(users); err != nil {
		log.Printf("User repository error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
	}
}

// GetUserById godoc
// @Summary Get user by id
// @Tags         users
// @Accept  json
// @Produce  json
// @Param id path string true "User id"
// @Success 200 {object} model.User
// @Failure      400
// @Failure      404
// @Failure      500
// @Router /user/get/{id} [get]
func (h *userHandlerImp) GetUserById(w http.ResponseWriter, req *http.Request) {
	ctx := req.Context()
	vars := mux.Vars(req)
	id, err := strconv.Atoi(vars["id"])
	if err != nil {
		log.Printf("User handler error: %v\n", err)
		w.WriteHeader(http.StatusBadRequest)
	}
	user, err := h.service.GetUserById(ctx, uint(id))
	if _, ok := err.(model.NotFoundError); ok {
		log.Printf("User handler error: %v\n", err)
		w.WriteHeader(http.StatusNotFound)
		return
	}
	if err != nil {
		log.Printf("User handler error: %v\n", err)
		w.WriteHeader(http.StatusBadRequest)
		return
	}
	if err := json.NewEncoder(w).Encode(user); err != nil {
		log.Printf("User handler error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
	}
}

// RegisterUser godoc
// @Summary Register new user
// @Tags         users
// @Accept  json
// @Param properties body dto.NewUserDto true "User properties"
// @Success 200 {object} string
// @Failure      400
// @Failure      404
// @Failure      500
// @Router /user/register [post]
func (h *userHandlerImp) RegisterUser(w http.ResponseWriter, req *http.Request) {
	ctx := req.Context()

	payload := make(map[string]interface{})
	err := json.NewDecoder(req.Body).Decode(&payload)
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	p, ok := handler.CheckParam(w, payload, "name", "User handler error: name\n")
	if !ok {
		return
	}
	name := p.(string)

	p, ok = handler.CheckParam(w, payload, "surname", "User handler error: surname\n")
	if !ok {
		return
	}
	surname := p.(string)

	p, ok = handler.CheckParam(w, payload, "password", "User handler error: password\n")
	if !ok {
		return
	}
	password := p.(string)

	user := &model.User{
		Name:    name,
		Surname: surname,
	}

	pAge, ok := payload["age"].(float64)
	if ok {
		ui := uint8(pAge)
		user.Age = &ui
	}

	pSex, ok := payload["sex"].(byte)
	if ok {
		user.Sex = &pSex
	}

	pCity, ok := payload["city"].(string)
	if ok {
		user.City = &pCity
	}

	pInfo, ok := payload["info"].(string)
	if ok {
		user.Info = &pInfo
	}

	err = h.service.PutUser(ctx, user, password)
	if err != nil {
		log.Printf("User handler error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
		return
	}
	u := struct {
		UserId uint `json:"user_id"`
	}{UserId: user.ID}
	if err := json.NewEncoder(w).Encode(u); err != nil {
		log.Printf("User handler error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
	}
}

// RegisterUser godoc
// @Summary Find user
// @Tags         users
// @Accept  json
// @Param properties query dto.SearchUserDto true "Search user properties"
// @Success 200 {array} model.User
// @Failure      400
// @Failure      404
// @Failure      500
// @Router /user/search [get]
func (h *userHandlerImp) FindUser(w http.ResponseWriter, req *http.Request) {
	ctx := req.Context()
	vars := req.URL.Query()
	name := vars.Get("first_name")
	if name == "" {
		log.Printf("User handler error: empty name")
		w.WriteHeader(http.StatusBadRequest)
	}
	surname := vars.Get("last_name")
	if surname == "" {
		log.Printf("User handler error: empty surname")
		w.WriteHeader(http.StatusBadRequest)
	}
	users, err := h.service.GetUsersByName(ctx, name, surname)
	if _, ok := err.(model.NotFoundError); ok {
		log.Printf("User handler error: %v\n", err)
		w.WriteHeader(http.StatusNotFound)
		return
	}
	if err != nil {
		log.Printf("User handler error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
		return
	}
	if err := json.NewEncoder(w).Encode(users); err != nil {
		log.Printf("User handler error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
	}
}
