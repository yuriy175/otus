package userhandler

import (
	"encoding/json"
	"log"
	"net/http"

	"socialnerworkapp.com/bff/internal/handler"
	"socialnerworkapp.com/bff/internal/handler/dto"
	"socialnerworkapp.com/bff/internal/service"
	commonhandler "socialnerworkapp.com/pkg/common/handler"
	"socialnerworkapp.com/pkg/trace"
)

type userHandlerImp struct {
	service service.UserService
	tracer  trace.OtelTracer
}

func NewUserHandler(
	tracer trace.OtelTracer,
	service service.UserService) handler.UserHandler {
	return &userHandlerImp{
		service: service,
		tracer:  tracer,
	}
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
	ctx := req.Context()
	tracer := h.tracer.CreateTracer(req.RequestURI)
	ctx, endSpan := h.tracer.StartSpan(ctx, tracer, "Login")
	defer endSpan()

	payload := make(map[string]interface{})
	err := json.NewDecoder(req.Body).Decode(&payload)
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

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
	userDto, err := h.service.Login(ctx, tracer, id, password)
	if err != nil {
		log.Printf("Auth service error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
		return
	}
	if err := json.NewEncoder(w).Encode(userDto); err != nil {
		log.Printf("Auth service error: %v\n", err)
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

	tracer := h.tracer.CreateTracer(req.RequestURI)
	ctx, endSpan := h.tracer.StartSpan(ctx, tracer, "RegisterUser")
	defer endSpan()

	payload := make(map[string]interface{})
	err := json.NewDecoder(req.Body).Decode(&payload)
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	p, ok := commonhandler.CheckParam(w, payload, "name", "User handler error: name\n")
	if !ok {
		return
	}
	name := p.(string)

	p, ok = commonhandler.CheckParam(w, payload, "surname", "User handler error: surname\n")
	if !ok {
		return
	}
	surname := p.(string)

	p, ok = commonhandler.CheckParam(w, payload, "password", "User handler error: password\n")
	if !ok {
		return
	}
	password := p.(string)

	user := &dto.NewUserDto{
		Name:     name,
		Surname:  surname,
		Password: password,
	}

	pAge, ok := payload["age"].(float64)
	if ok {
		ui := uint8(pAge)
		user.Age = &ui
	}

	pCity, ok := payload["city"].(string)
	if ok {
		user.City = &pCity
	}

	pInfo, ok := payload["info"].(string)
	if ok {
		user.Info = &pInfo
	}

	userDto, err := h.service.PutUser(ctx, tracer, user, password)
	if err != nil {
		log.Printf("User handler error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
		return
	}

	if err := json.NewEncoder(w).Encode(userDto); err != nil {
		log.Printf("User handler error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
	}
}

// RegisterUser godoc
// @Summary Find user
// @Tags         users
// @Accept  json
// @Param properties query dto.SearchUserDto true "Search user properties"
// @Success 200 {array} dto.UserPostsDto
// @Failure      400
// @Failure      404
// @Failure      500
// @Router /user/search [get]
func (h *userHandlerImp) FindUser(w http.ResponseWriter, req *http.Request) {
	ctx := req.Context()
	tracer := h.tracer.CreateTracer(req.RequestURI)
	ctx, endSpan := h.tracer.StartSpan(ctx, tracer, "FindUser")
	defer endSpan()

	vars := req.URL.Query()
	name := vars.Get("Name")
	if name == "" {
		log.Printf("User handler error: empty name")
		w.WriteHeader(http.StatusBadRequest)
	}
	surname := vars.Get("Surname")
	if surname == "" {
		log.Printf("User handler error: empty surname")
		w.WriteHeader(http.StatusBadRequest)
	}
	users, err := h.service.GetUsersByName(ctx, tracer, name, surname)
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
