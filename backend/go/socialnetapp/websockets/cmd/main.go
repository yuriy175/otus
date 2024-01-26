package main

import (
	"log"
	"net/http"
	"os"

	"github.com/joho/godotenv"

	_ "socialnerworkapp.com/docs"
	"socialnerworkapp.com/pkg/mq"
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
	restPort, _ := os.LookupEnv("REST_PORT")
	log.Println(restPort)

	mqReceiver := mq.NewMqReceiver()
	mqSender := mq.NewMqSender()

	friendRepo := friendrepository.NewFriendRepository()

	wsFeedPostsSrv := websocketsservice.NewFeedPostsWebsocketsService(friendRepo, mqReceiver)
	wsDialogsSrv := websocketsservice.NewDialogssWebsocketsService(friendRepo, mqReceiver, mqSender)
	wsHandler := websocketshandler.NewWebsocketsHandler(wsFeedPostsSrv, wsDialogsSrv)
	http.HandleFunc("/post/feed", wsHandler.SendPosts)
	http.HandleFunc("/dialogs", wsHandler.SendDialogMessages)

	if err := http.ListenAndServe(":"+restPort, nil); err != nil {
		panic(err)
	}
}
