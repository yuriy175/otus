package main

import (
	"log"
	"net"
	"net/http"
	"os"

	"google.golang.org/grpc"
	"socialnerworkapp.com/profile/internal/handler/authhandler"
	"socialnerworkapp.com/profile/internal/handler/userhandler"
	"socialnerworkapp.com/profile/internal/repository/userrepository"
	"socialnerworkapp.com/profile/internal/service/authservice"
	"socialnerworkapp.com/profile/internal/service/userservice"

	"github.com/gorilla/mux"
	"github.com/joho/godotenv"

	httpSwagger "github.com/swaggo/http-swagger"
	_ "socialnerworkapp.com/docs"
	gen "socialnerworkapp.com/pkg/proto/gen"
	profilegrpc "socialnerworkapp.com/profile/internal/grpc"

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
	log.Println(os.LookupEnv("POSTGRESQL_CONNECTION"))
	log.Println(os.LookupEnv("REDIS_HOST"))
	grpcPort, _ := os.LookupEnv("GRPC_PORT")
	log.Println(grpcPort)
	restPort, _ := os.LookupEnv("REST_PORT")
	log.Println(restPort)

	// TODO: use fasthttp
	repo := userrepository.NewUserRepository()
	userSrv := userservice.NewUserService(repo)
	userHandler := userhandler.NewUserHandler(userSrv)
	authSrv := authservice.NewAuthService(repo)
	authHandler := authhandler.NewAuthHandler(authSrv)

	go func() {
		//lis, err := net.Listen("tcp", fmt.Sprintf("localhost:%d", 55267))
		lis, err := net.Listen("tcp", "localhost:"+grpcPort)
		if err != nil {
			log.Fatalf("failed to listen: %v", err)
		}
		var opts []grpc.ServerOption
		grpcServer := grpc.NewServer(opts...)
		gen.RegisterAuthServer(grpcServer, profilegrpc.NewGrpcAuthtHandler(authSrv))
		gen.RegisterUsersServer(grpcServer, profilegrpc.NewGrpcUserHandler(userSrv))
		if err = grpcServer.Serve(lis); err != nil {
			panic(err)
		}
	}()

	router := mux.NewRouter()
	router.HandleFunc("/users", userHandler.GetUsers)
	router.HandleFunc("/user/get/{id}", userHandler.GetUserById)
	router.HandleFunc("/user/register", userHandler.RegisterUser)
	router.HandleFunc("/user/search", userHandler.FindUser)
	router.HandleFunc("/login", authHandler.Login)

	router.PathPrefix("/swagger").Handler(httpSwagger.WrapHandler)
	http.Handle("/", router)

	handler := cors.Default().Handler(router)
	if err := http.ListenAndServe(":"+restPort, handler); err != nil {
		panic(err)
	}
}
