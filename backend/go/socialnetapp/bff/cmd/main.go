package main

import (
	"log"
	"net/http"
	"os"

	"socialnerworkapp.com/bff/internal/handler/counterhandler"
	"socialnerworkapp.com/bff/internal/handler/dialoghandler"
	"socialnerworkapp.com/bff/internal/handler/friendhandler"
	"socialnerworkapp.com/bff/internal/handler/posthandler"
	"socialnerworkapp.com/bff/internal/handler/userhandler"
	"socialnerworkapp.com/bff/internal/middleware"
	"socialnerworkapp.com/bff/internal/service/counterservice"
	"socialnerworkapp.com/bff/internal/service/dialogservice"
	"socialnerworkapp.com/bff/internal/service/friendservice"
	"socialnerworkapp.com/bff/internal/service/postservice"
	"socialnerworkapp.com/bff/internal/service/userservice"

	"github.com/gorilla/mux"
	"github.com/joho/godotenv"
	"github.com/prometheus/client_golang/prometheus/promhttp"

	httpSwagger "github.com/swaggo/http-swagger"
	_ "socialnerworkapp.com/docs"

	"github.com/rs/cors"
	trace "socialnerworkapp.com/pkg/trace"
)

func init() {
	// loads values from .env into the system
	if err := godotenv.Load(); err != nil {
		log.Print("No .env file found")
	}
}

// @title Social Net API
// @version 1.0
// @description This is a serice for managing users
// @securityDefinitions.apikey BearerAuth
// @in header
// @name Authorization
// @BasePath /
func main() {
	log.Println("Started Go!")
	grpcProfileUrl, _ := os.LookupEnv("GRPC_PROFILE")
	log.Println(grpcProfileUrl)
	log.Println(os.LookupEnv("GRPC_DIALOGS"))
	log.Println(os.LookupEnv("GRPC_POSTS"))
	log.Println(os.LookupEnv("GRPC_COUNTERS"))
	restPort, _ := os.LookupEnv("REST_PORT")
	log.Println(restPort)
	log.Println(os.LookupEnv("OTEL_EXPORTER_JAEGER_ENDPOINT"))

	tracer := trace.NewOtelTracer("Go Bff Service")
	userSrv := userservice.NewUserService(tracer)
	userHandler := userhandler.NewUserHandler(tracer, userSrv)

	friendSrv := friendservice.NewFriendService(tracer)
	friendHandler := friendhandler.NewFriendHandler(tracer, friendSrv)

	postSrv := postservice.NewPostService(tracer)
	postHandler := posthandler.NewPostHandler(tracer, postSrv)

	dialogSrv := dialogservice.NewDialogService(tracer)
	dialogHandler := dialoghandler.NewDialogHandler(tracer, dialogSrv)

	counterSrv := counterservice.NewCounterService(tracer)
	counterHandler := counterhandler.NewCounterHandler(tracer, counterSrv)

	router := mux.NewRouter()
	router.HandleFunc("/login", userHandler.Login)
	router.HandleFunc("/friend/set/{user_id}", friendHandler.AddFriend)
	router.HandleFunc("/friend/delete/{user_id}", friendHandler.DeleteFriend)
	router.HandleFunc("/friends", friendHandler.GetFriends)
	router.HandleFunc("/post/create", postHandler.CreatePost)
	router.HandleFunc("/post/feed", postHandler.FeedPosts)
	router.HandleFunc("/user/register", userHandler.RegisterUser)
	router.HandleFunc("/user/search", userHandler.FindUser)
	router.HandleFunc("/dialog/{user_id}/list", dialogHandler.GetDialogByUserId)
	router.HandleFunc("/dialog/{user_id}/send", dialogHandler.SendMessageToUser)
	router.HandleFunc("/buddies", dialogHandler.GetDialogBuddies)
	router.HandleFunc("/unread/count", counterHandler.GetUnReadCounterByUserId)
	router.PathPrefix("/metrics").Handler(promhttp.Handler())
	router.Use(middleware.MetricMiddleware)
	router.Use(middleware.HeadersMiddleware)
	router.PathPrefix("/swagger").Handler(httpSwagger.WrapHandler)
	http.Handle("/", router)

	handler := cors.Default().Handler(router)
	if err := http.ListenAndServe(":"+restPort, handler); err != nil {
		panic(err)
	}
}
