package handler

import (
	"log"
	"net/http"
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
