package main

import (
	"log"
	"net"
	"net/http"
	"os"

	"google.golang.org/grpc"
	"socialnerworkapp.com/pkg/mq"
	"socialnerworkapp.com/pkg/proto/gen"
	"socialnerworkapp.com/posts/internal/cache/redis"
	"socialnerworkapp.com/posts/internal/handler/friendhandler"
	"socialnerworkapp.com/posts/internal/handler/posthandler"
	"socialnerworkapp.com/posts/internal/repository/friendrepository"
	"socialnerworkapp.com/posts/internal/repository/postrepository"
	"socialnerworkapp.com/posts/internal/service/friendservice"
	"socialnerworkapp.com/posts/internal/service/postservice"

	"github.com/gorilla/mux"
	"github.com/joho/godotenv"

	httpSwagger "github.com/swaggo/http-swagger"
	_ "socialnerworkapp.com/docs"

	"github.com/rs/cors"
	postsgrpc "socialnerworkapp.com/posts/internal/grpc"
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
	friendRepo := friendrepository.NewFriendRepository()
	friendSrv := friendservice.NewFriendService(friendRepo)
	friendHandler := friendhandler.NewFriendHandler(friendSrv)

	cacheSrv := redis.NewRedisService()
	mqSender := mq.NewMqSender()

	postRepo := postrepository.NewPostRepository()
	postSrv := postservice.NewPostService(postRepo, friendRepo, cacheSrv, mqSender)
	postHandler := posthandler.NewPostHandler(postSrv)

	go func() {
		lis, err := net.Listen("tcp", "localhost:"+grpcPort)
		if err != nil {
			log.Fatalf("failed to listen: %v", err)
		}
		var opts []grpc.ServerOption
		grpcServer := grpc.NewServer(opts...)
		gen.RegisterFriendServer(grpcServer, postsgrpc.NewGrpcFriendsHandler(friendSrv))
		gen.RegisterPostServer(grpcServer, postsgrpc.NewGrpcPostsHandler(postSrv))
		if err = grpcServer.Serve(lis); err != nil {
			panic(err)
		}
	}()

	router := mux.NewRouter()
	router.HandleFunc("/friend/set/{user_id}", friendHandler.AddFriend)
	router.HandleFunc("/friend/delete/{user_id}", friendHandler.DeleteFriend)

	router.HandleFunc("/post/create", postHandler.CreatePost)
	router.HandleFunc("/post/update", postHandler.UpdatePost)
	router.HandleFunc("/post/get/{id}", postHandler.GetPost)
	router.HandleFunc("/post/delete/{id}", postHandler.DeletePost)
	router.HandleFunc("/post/feed", postHandler.FeedPosts)

	router.PathPrefix("/swagger").Handler(httpSwagger.WrapHandler)
	http.Handle("/", router)

	handler := cors.Default().Handler(router)
	if err := http.ListenAndServe(":80", handler); err != nil {
		panic(err)
	}
}
