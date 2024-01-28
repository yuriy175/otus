package main

import (
	"log"
	"net/http"
	"os"

	"github.com/joho/godotenv"

	_ "socialnerworkapp.com/docs"
	"socialnerworkapp.com/pkg/mq"
	"socialnerworkapp.com/websockets/internal/cache/redis"
	"socialnerworkapp.com/websockets/internal/handler/websocketshandler"
	"socialnerworkapp.com/websockets/internal/repository/friendrepository"
	"socialnerworkapp.com/websockets/internal/service/websocketsservice"
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
	log.Println(os.LookupEnv("POSTGRESQL_CONNECTION"))
	log.Println(os.LookupEnv("RABBITMQ_CONNECTION"))
	log.Println(os.LookupEnv("REDIS_HOST"))
	restPort, _ := os.LookupEnv("REST_PORT")
	log.Println(restPort)
	log.Println(os.Hostname())

	mqReceiver := mq.NewMqReceiver()
	mqSender := mq.NewMqSender()
	cacheSrv := redis.NewRedisService()

	friendRepo := friendrepository.NewFriendRepository()

	wsFeedPostsSrv := websocketsservice.NewFeedPostsWebsocketsService(friendRepo, mqReceiver)
	wsDialogsSrv := websocketsservice.NewDialogssWebsocketsService(friendRepo, mqReceiver, mqSender, cacheSrv)
	wsHandler := websocketshandler.NewWebsocketsHandler(wsFeedPostsSrv, wsDialogsSrv)
	http.HandleFunc("/post/feed", wsHandler.SendPosts)
	http.HandleFunc("/dialogs", wsHandler.SendDialogMessages)

	if err := http.ListenAndServe(":"+restPort, nil); err != nil {
		panic(err)
	}
}
