package main

import (
	"log"
	"net"
	"net/http"
	"os"

	"github.com/gorilla/mux"
	"github.com/joho/godotenv"
	"google.golang.org/grpc"

	httpSwagger "github.com/swaggo/http-swagger"
	"socialnerworkapp.com/dialogs/internal/handler/dialogshandler"
	"socialnerworkapp.com/dialogs/internal/repository/dialogsrepository"
	"socialnerworkapp.com/dialogs/internal/service/dialogsservice"
	"socialnerworkapp.com/pkg/mq"
	"socialnerworkapp.com/pkg/proto/gen"

	_ "socialnerworkapp.com/docs"

	"github.com/rs/cors"
	dialogsgrpc "socialnerworkapp.com/dialogs/internal/grpc"
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
	grpcPort, _ := os.LookupEnv("GRPC_PORT")
	log.Println(grpcPort)
	restPort, _ := os.LookupEnv("REST_PORT")
	log.Println(restPort)

	// TODO: use fasthttp
	mqSender := mq.NewMqSender()

	repo := dialogsrepository.NewDialogsRepository()
	dialogsSrv := dialogsservice.NewDialogsService(repo, mqSender)
	dialogsHandler := dialogshandler.NewDialogsHandler(dialogsSrv)

	go func() {
		lis, err := net.Listen("tcp", "localhost:"+grpcPort)
		if err != nil {
			log.Fatalf("failed to listen: %v", err)
		}
		var opts []grpc.ServerOption
		grpcServer := grpc.NewServer(opts...)
		gen.RegisterDialogServer(grpcServer, dialogsgrpc.NewGrpcDialogsHandler(dialogsSrv))
		if err = grpcServer.Serve(lis); err != nil {
			panic(err)
		}
	}()

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
