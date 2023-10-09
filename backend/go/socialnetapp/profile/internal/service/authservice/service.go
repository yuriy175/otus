package authservice

import (
	"context"
	"os"
	"strconv"

	"github.com/dgrijalva/jwt-go"
	_ "github.com/google/uuid"
	"socialnerworkapp.com/profile/internal/model"
	"socialnerworkapp.com/profile/internal/repository"
	"socialnerworkapp.com/profile/internal/service"
)

type authServiceImp struct {
	repository repository.UserRepository

	// jwt token secret
	jwtSecret string
}

func NewAuthService(repository repository.UserRepository) service.AuthService {
	return &authServiceImp{repository: repository, jwtSecret: os.Getenv("SECURITY_KEY")}
}

func (s *authServiceImp) Login(ctx context.Context, userId uint, password string) (string, error) {
	exists, err := s.repository.CheckUser(ctx, userId, password)
	if err != nil {
		return "", err
	}

	if !exists {
		return "", model.NotFoundError{}
	}

	token := s.createToken(userId)

	// not clear what should be returned
	return token, nil //uuid.NewString(), nil
}

func (s *authServiceImp) createToken(userId uint) string {
	claims := &model.UserClaims{UserId: strconv.Itoa(int(userId))}
	token := jwt.NewWithClaims(jwt.GetSigningMethod("HS256"), claims)
	tokenString, _ := token.SignedString([]byte(s.jwtSecret))

	return tokenString
}

func (s *authServiceImp) GetAuthorizedUserId(tokenString string) string {
	claims := &model.UserClaims{}
	token, err := jwt.ParseWithClaims(tokenString, claims, func(token *jwt.Token) (interface{}, error) {
		return []byte(s.jwtSecret), nil
	})

	if token == nil || err != nil {
		return ""
	}

	return claims.UserId
}
