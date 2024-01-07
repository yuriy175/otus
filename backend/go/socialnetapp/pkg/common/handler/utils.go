package handler

import (
	"log"
	"net/http"
	"strconv"
	"strings"

	commonmodel "socialnerworkapp.com/pkg/common/model"
	"socialnerworkapp.com/pkg/common/service"
)

func CheckParam(w http.ResponseWriter, payload map[string]interface{}, paramName string, errorMessage string) (interface{}, bool) {
	p, ok := payload[paramName]
	if !ok {
		log.Printf(errorMessage)
		w.WriteHeader((http.StatusBadRequest))
		return nil, ok
	}

	return p, ok
}

func GetAuthorizationToken(w http.ResponseWriter, r *http.Request) (string, error) {
	tokenHeader := r.Header.Get("Authorization") //Получение токена

	if tokenHeader == "" { //Токен отсутствует, возвращаем  403 http-код Unauthorized
		w.WriteHeader(http.StatusForbidden)
		w.Header().Add("Content-Type", "application/json")
		return "", commonmodel.NotAuthorizedError{}
	}

	return tokenHeader, nil
}

func GetAuthorizedUserId(
	w http.ResponseWriter,
	r *http.Request,
	token string) (uint, error) {
	token = strings.TrimPrefix(token, "Bearer ")
	userId := service.GetAuthorizedUserId(token)

	if userId == "" {
		w.WriteHeader(http.StatusForbidden)
		w.Header().Add("Content-Type", "application/json")
		return 0, commonmodel.NotAuthorizedError{}
	}
	val, _ := strconv.Atoi(userId)
	return uint(val), nil
}

func CheckAuthorizationAndGetUserId(
	w http.ResponseWriter,
	r *http.Request) (uint, error) {
	token, err := GetAuthorizationToken(w, r)
	if err != nil {
		return 0, err
	}
	userId, err := GetAuthorizedUserId(w, r, token)
	if err != nil {
		return 0, err
	}

	return userId, nil
}
