package service

import (
	"os"

	"github.com/dgrijalva/jwt-go"
	_ "github.com/google/uuid"
	"socialnerworkapp.com/pkg/common/model"
)

func GetAuthorizedUserId(tokenString string) string {
	jwtSecret := os.Getenv("SECURITY_KEY")
	claims := &model.UserClaims{}
	token, err := jwt.ParseWithClaims(tokenString, claims, func(token *jwt.Token) (interface{}, error) {
		return []byte(jwtSecret), nil
	})

	if token == nil || err != nil {
		return ""
	}

	return claims.UserId
}
