package middleware

import (
	"net/http"
	"strings"
	"sync"

	"github.com/prometheus/client_golang/prometheus"
	"github.com/prometheus/client_golang/prometheus/promauto"
)

var mtx sync.RWMutex
var counters = make(map[string]prometheus.Counter)

func MetricMiddleware(next http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		mtx.Lock()
		defer mtx.Unlock()
		key := "bff_go_" + strings.Replace(r.RequestURI, "/", "", -1)
		counter, ok := counters[key]
		if !ok {
			counter = promauto.NewCounter(prometheus.CounterOpts{
				Name: key,
			})
			counters[key] = counter
		}
		counter.Inc()
		next.ServeHTTP(w, r)
	})
}
