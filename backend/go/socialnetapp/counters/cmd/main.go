package main

import (
	"log"
	"net"
	"net/http"
	"os"

	"github.com/joho/godotenv"
	"google.golang.org/grpc"

	countersgrpc "socialnerworkapp.com/counters/internal/grpc"
	"socialnerworkapp.com/counters/internal/repository/countersrepository"
	"socialnerworkapp.com/counters/internal/service/countersservice"
	_ "socialnerworkapp.com/docs"
	"socialnerworkapp.com/pkg/mq"
	"socialnerworkapp.com/pkg/proto/gen"
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
	grpcPort, _ := os.LookupEnv("GRPC_PORT")
	log.Println(grpcPort)
	restPort, _ := os.LookupEnv("REST_PORT")
	log.Println(restPort)

	mqReceiver := mq.NewMqReceiver()
	mqSender := mq.NewMqSender()

	repo := countersrepository.NewCountersRepository()
	countersSrv := countersservice.NewCountersService(repo, mqSender, mqReceiver)
	//countersHandler := countershandler.(countersSrv)

	go func() {
		lis, err := net.Listen("tcp", ":"+grpcPort)
		if err != nil {
			log.Fatalf("failed to listen: %v", err)
		}
		var opts []grpc.ServerOption
		grpcServer := grpc.NewServer(opts...)
		gen.RegisterCounterServer(grpcServer, countersgrpc.NewGrpcCountersHandler(countersSrv))
		if err = grpcServer.Serve(lis); err != nil {
			panic(err)
		}
	}()
	if err := http.ListenAndServe(":"+restPort, nil); err != nil {
		panic(err)
	}
}
