package posthandler

import (
	"encoding/json"
	"io"
	"log"
	"net/http"
	"strconv"

	"socialnerworkapp.com/bff/internal/handler"
	"socialnerworkapp.com/bff/internal/service"

	commonhandler "socialnerworkapp.com/pkg/common/handler"
	"socialnerworkapp.com/pkg/trace"
)

type postHandlerImp struct {
	service service.PostService
	tracer  trace.OtelTracer
}

func NewPostHandler(tracer trace.OtelTracer, service service.PostService) handler.PostHandler {
	return &postHandlerImp{service: service, tracer: tracer}
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
