package main

import (
	"log"
	"net/http"
	"os"

	"github.com/gorilla/mux"
	"github.com/joho/godotenv"

	httpSwagger "github.com/swaggo/http-swagger"
	"socialnerworkapp.com/dialogs/internal/handler/dialogshandler"
	"socialnerworkapp.com/dialogs/internal/repository/dialogsrepository"
	"socialnerworkapp.com/dialogs/internal/service/dialogsservice"

	_ "socialnerworkapp.com/docs"

	"github.com/rs/cors"
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
	log.Println(os.LookupEnv("POSTGRESQL_CONNECTION_DB"))
	log.Println(os.LookupEnv("REDIS_HOST"))

	// TODO: use fasthttp
	repo := dialogsrepository.NewDialogsRepository()
	dialogsSrv := dialogsservice.NewDialogsService(repo)
	dialogsHandler := dialogshandler.NewDialogsHandler(dialogsSrv)

	router := mux.NewRouter()
	router.HandleFunc("/dialog/{user_id}/list", dialogsHandler.GetDialogByUserId)
	router.HandleFunc("/dialog/{user_id}/send", dialogsHandler.SendMessageToUser)

	router.PathPrefix("/swagger").Handler(httpSwagger.WrapHandler)
	http.Handle("/", router)

	handler := cors.Default().Handler(router)
	if err := http.ListenAndServe(":80", handler); err != nil {
		panic(err)
	}
}
