package trace

import (
	"context"

	"go.opentelemetry.io/otel/trace"
)

type OtelTracer interface {
	CreateTracer(name string) trace.Tracer
	StartSpan(ctx context.Context, tracer trace.Tracer, spanName string) (context.Context, func())
}
