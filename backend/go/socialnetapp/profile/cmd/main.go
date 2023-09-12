package main

import (
	"log"
	"net/http"
	"os"

	"socialnerworkapp.com/profile/internal/handler/authhandler"
	"socialnerworkapp.com/profile/internal/handler/userhandler"
	"socialnerworkapp.com/profile/internal/repository/userrepository"
	"socialnerworkapp.com/profile/internal/service/authservice"
	"socialnerworkapp.com/profile/internal/service/userservice"

	"github.com/gorilla/mux"
	"github.com/joho/godotenv"

	httpSwagger "github.com/swaggo/http-swagger"
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
// @BasePath /
func main() {
	log.Println("Started Go!")
	log.Println(os.LookupEnv("POSTGRESQL_CONNECTION"))
	log.Println(os.LookupEnv("GOPATH"))

	// TODO: use fasthttp
	repo := userrepository.NewUserRepository()
	userSrv := userservice.NewUserService(repo)
	userHandler := userhandler.NewUserHandler(userSrv)
	authSrv := authservice.NewAuthService(repo)
	authHandler := authhandler.NewAuthHandler(authSrv)

	router := mux.NewRouter()
	router.HandleFunc("/users", userHandler.GetUsers)
	router.HandleFunc("/user/get/{id}", userHandler.GetUserById)
	router.HandleFunc("/user/register", userHandler.RegisterUser)
	router.HandleFunc("/login", authHandler.Login)
	router.PathPrefix("/swagger").Handler(httpSwagger.WrapHandler)
	http.Handle("/", router)

	handler := cors.Default().Handler(router)
	if err := http.ListenAndServe(":80", handler); err != nil {
		panic(err)
	}
}
