package main

import (
	"log"
	"net/http"
	"os"

	"socialnerworkapp.com/bff/internal/handler/userhandler"
	"socialnerworkapp.com/bff/internal/service/userservice"

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
	restPort, _ := os.LookupEnv("REST_PORT")
	log.Println(restPort)

	//ctx := context.Background()
	// conn, err := grpc.Dial(grpcProfileUrl, grpc.WithTransportCredentials(insecure.NewCredentials())) // , opts...)
	// if err != nil {
	// 	log.Fatalf("fail to dial: %v", err)
	// }
	// defer conn.Close()
	// var id uint32 = 1645801
	// authClient := gen.NewAuthClient(conn)
	// loginRequest := &gen.LoginRequest{Id: id, Password: "Абрамов"}
	// token, err := authClient.Login(ctx, loginRequest)

	// userClient := gen.NewUsersClient(conn)
	// userRequest := &gen.GetUserByIdRequest{Id: id}
	// user, err := userClient.GetUserById(ctx, userRequest)

	// _ = token
	// _ = user

	userSrv := userservice.NewUserService()
	userHandler := userhandler.NewUserHandler(userSrv)

	router := mux.NewRouter()
	router.HandleFunc("/login", userHandler.Login)

	router.PathPrefix("/swagger").Handler(httpSwagger.WrapHandler)
	http.Handle("/", router)

	handler := cors.Default().Handler(router)
	if err := http.ListenAndServe(":"+restPort, handler); err != nil {
		panic(err)
	}
}
