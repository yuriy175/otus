package middleware

import "net/http"

func HeadersMiddleware(next http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		w.Header().Add("Server-Language", "Golang")
		next.ServeHTTP(w, r)
	})
}
