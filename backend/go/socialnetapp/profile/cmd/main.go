package main

import (
	"log"
	"net/http"
	"os"

	"socialnerworkapp.com/profile/internal/cache/redis"
	"socialnerworkapp.com/profile/internal/handler/authhandler"
	"socialnerworkapp.com/profile/internal/handler/friendhandler"
	"socialnerworkapp.com/profile/internal/handler/posthandler"
	"socialnerworkapp.com/profile/internal/handler/userhandler"
	"socialnerworkapp.com/profile/internal/repository/friendrepository"
	"socialnerworkapp.com/profile/internal/repository/postrepository"
	"socialnerworkapp.com/profile/internal/repository/userrepository"
	"socialnerworkapp.com/profile/internal/service/authservice"
	"socialnerworkapp.com/profile/internal/service/friendservice"
	"socialnerworkapp.com/profile/internal/service/postservice"
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
// @securityDefinitions.apikey BearerAuth
// @in header
// @name Authorization
// @BasePath /
func main() {
	log.Println("Started Go!")
	log.Println(os.LookupEnv("POSTGRESQL_CONNECTION"))
	log.Println(os.LookupEnv("REDIS_HOST"))

	// TODO: use fasthttp
	repo := userrepository.NewUserRepository()
	userSrv := userservice.NewUserService(repo)
	userHandler := userhandler.NewUserHandler(userSrv)
	authSrv := authservice.NewAuthService(repo)
	authHandler := authhandler.NewAuthHandler(authSrv)

	friendRepo := friendrepository.NewFriendRepository()
	friendSrv := friendservice.NewFriendService(friendRepo)
	friendHandler := friendhandler.NewFriendHandler(authSrv, friendSrv)

	cacheSrv := redis.NewRedisService()

	postRepo := postrepository.NewPostRepository()
	postSrv := postservice.NewPostService(postRepo, friendRepo, cacheSrv)
	postHandler := posthandler.NewPostHandler(authSrv, postSrv)

	router := mux.NewRouter()
	router.HandleFunc("/users", userHandler.GetUsers)
	router.HandleFunc("/user/get/{id}", userHandler.GetUserById)
	router.HandleFunc("/user/register", userHandler.RegisterUser)
	router.HandleFunc("/user/search", userHandler.FindUser)
	router.HandleFunc("/login", authHandler.Login)
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
