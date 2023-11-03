package main

import (
	"log"
	"net/http"
	"os"

	"github.com/joho/godotenv"

	_ "socialnerworkapp.com/docs"
	"socialnerworkapp.com/pkg/mq"
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

	// TODO: use fasthttp
	mqReceiver := mq.NewMqReceiver()
	friendRepo := friendrepository.NewFriendRepository()

	wsSrv := websocketsservice.NewWebsocketsService(friendRepo, mqReceiver)
	wsSrv.Start()
	if err := http.ListenAndServe(":80", nil); err != nil {
		panic(err)
	}
}
