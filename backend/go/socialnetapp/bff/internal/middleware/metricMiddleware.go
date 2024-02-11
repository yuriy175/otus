package middleware

import (
	"net/http"
	"strings"
	"sync"
	"time"

	"github.com/prometheus/client_golang/prometheus"
	"github.com/prometheus/client_golang/prometheus/promauto"
)

var mtx sync.RWMutex
var counters = make(map[string]prometheus.Counter)
var histograms = make(map[string]prometheus.Histogram)

func MetricMiddleware(next http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		mtx.Lock()
		defer mtx.Unlock()
		start := time.Now()
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

		key = key + "_hist"
		histogram, ok := histograms[key]
		if !ok {
			histogram = promauto.NewHistogram(
				prometheus.HistogramOpts{
					Name:    key,
					Buckets: prometheus.LinearBuckets(0, 3, 100),
				})
			histograms[key] = histogram
		}
		histogram.Observe(float64(time.Since(start).Milliseconds()))
	})
}
