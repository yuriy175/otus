package posthandler

import (
	"encoding/json"
	"io"
	"log"
	"net/http"
	"strconv"

	"github.com/gorilla/mux"
	"socialnerworkapp.com/posts/internal/handler"
	"socialnerworkapp.com/posts/internal/service"

	commonhandler "socialnerworkapp.com/pkg/common/handler"
	commonmodel "socialnerworkapp.com/pkg/common/model"
)

type postHandlerImp struct {
	service service.PostService
}

func NewPostHandler(service service.PostService) handler.PostHandler {
	return &postHandlerImp{
		service: service}
}

// CreatePost godoc
// @Summary Add user's post
// @Tags         posts
// @Security BearerAuth
// @Accept  json
// @Produce  json
// @Param text body string true "Text"
// @Success 200
// @Failure      400
// @Failure      404
// @Failure      500
// @Router /post/create [post]
func (h *postHandlerImp) CreatePost(w http.ResponseWriter, req *http.Request) {
	ctx := req.Context()

	userId, err := commonhandler.CheckAuthorizationAndGetUserId(w, req)
	if err != nil {
		return
	}

	body, err := io.ReadAll(req.Body)
	text := string(body)
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	err = h.service.CreatePost(ctx, userId, text)
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
	}
}

// UpdatePost godoc
// @Summary Update user's post
// @Tags         posts
// @Security BearerAuth
// @Accept  json
// @Produce  json
// @Param properties body dto.UpdatePostDto true "Post properties"
// @Success 200
// @Failure      400
// @Failure      404
// @Failure      500
// @Router /post/update [put]
func (h *postHandlerImp) UpdatePost(w http.ResponseWriter, req *http.Request) {
	userId, err := commonhandler.CheckAuthorizationAndGetUserId(w, req)
	if err != nil {
		return
	}

	ctx := req.Context()

	payload := make(map[string]interface{})
	err = json.NewDecoder(req.Body).Decode(&payload)
	if err != nil {
		http.Error(w, err.Error(), http.StatusInternalServerError)
		return
	}

	p, ok := commonhandler.CheckParam(w, payload, "id", "Post handler error: name\n")
	if !ok {
		return
	}
	postId := uint(p.(float64))

	p, ok = commonhandler.CheckParam(w, payload, "text", "Post handler error: surname\n")
	if !ok {
		return
	}
	text := p.(string)

	err = h.service.UpdatePost(ctx, userId, postId, text)
	if err != nil {
		log.Printf("Post repository error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
		return
	}
}

// DeletePost godoc
// @Summary Delete user's post
// @Tags         posts
// @Security BearerAuth
// @Accept  json
// @Produce  json
// @Param id path string true "Post id"
// @Success 200
// @Failure      400
// @Failure      404
// @Failure      500
// @Router /post/delete/{id} [delete]
func (h *postHandlerImp) DeletePost(w http.ResponseWriter, req *http.Request) {
	ctx := req.Context()
	userId, err := commonhandler.CheckAuthorizationAndGetUserId(w, req)
	if err != nil {
		return
	}

	vars := mux.Vars(req)
	postId, err := strconv.Atoi(vars["id"])
	if err != nil {
		log.Printf("Post handler error: %v\n", err)
		w.WriteHeader(http.StatusBadRequest)
	}

	err = h.service.DeletePost(ctx, userId, uint(postId))

	if err != nil {
		log.Printf("Friend handler error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
	}
}

// GetPost godoc
// @Summary Get user's post
// @Tags         posts
// @Security BearerAuth
// @Accept  json
// @Produce  json
// @Param id path string true "Post id"
// @Success 200 {object} model.Post
// @Failure      400
// @Failure      404
// @Failure      500
// @Router /post/get/{id} [get]
func (h *postHandlerImp) GetPost(w http.ResponseWriter, req *http.Request) {
	ctx := req.Context()

	vars := mux.Vars(req)
	postId, err := strconv.Atoi(vars["id"])
	if err != nil {
		log.Printf("Post handler error: %v\n", err)
		w.WriteHeader(http.StatusBadRequest)
	}

	post, err := h.service.GetPost(ctx, uint(postId))

	if _, ok := err.(commonmodel.NotFoundError); ok {
		log.Printf("Post handler error: %v\n", err)
		w.WriteHeader(http.StatusNotFound)
		return
	}
	if err != nil {
		log.Printf("Post handler error: %v\n", err)
		w.WriteHeader(http.StatusBadRequest)
		return
	}
	if err := json.NewEncoder(w).Encode(post); err != nil {
		log.Printf("Post handler error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
	}
}

// FeedPosts godoc
// @Summary Feed friends post
// @Tags         posts
// @Security BearerAuth
// @Accept  json
// @Produce  json
// @Param properties query dto.FeedPostsDto true "Feed posts properties"
// @Success 200 {array} model.Post
// @Failure      400
// @Failure      404
// @Failure      500
// @Router /post/feed [get]
func (h *postHandlerImp) FeedPosts(w http.ResponseWriter, req *http.Request) {
	userId, err := commonhandler.CheckAuthorizationAndGetUserId(w, req)
	if err != nil {
		return
	}

	ctx := req.Context()

	vars := req.URL.Query()
	p := vars.Get("offset")
	if p == "" {
		log.Printf("User handler error: empty offset")
		w.WriteHeader(http.StatusBadRequest)
	}

	offset, err := strconv.Atoi(p)
	p = vars.Get("limit")
	if p == "" {
		log.Printf("User handler error: empty limit")
		w.WriteHeader(http.StatusBadRequest)
	}
	limit, err := strconv.Atoi(p)

	posts, err := h.service.FeedPosts(ctx, userId, uint(offset), uint(limit))

	if err != nil {
		log.Printf("Post handler error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
		return
	}
	if err := json.NewEncoder(w).Encode(posts); err != nil {
		log.Printf("Post handler error: %v\n", err)
		w.WriteHeader(http.StatusInternalServerError)
	}
}
