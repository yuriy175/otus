package trace

import (
	"context"
	"os"

	"go.opentelemetry.io/otel"
	"go.opentelemetry.io/otel/exporters/otlp/otlptrace/otlptracehttp"
	"go.opentelemetry.io/otel/sdk/resource"

	//"go.opentelemetry.io/otel/sdk/trace"
	sdktrace "go.opentelemetry.io/otel/sdk/trace"
	semconv "go.opentelemetry.io/otel/semconv/v1.21.0"
	"go.opentelemetry.io/otel/trace"
)

type otelTracerImp struct {
	tracerProvider *sdktrace.TracerProvider
}

func NewOtelTracer(serviceName string) OtelTracer {
	tracer := &otelTracerImp{}
	tracer.initTracer(serviceName)
	return tracer
}

func (t *otelTracerImp) initTracer(serviceName string) error {
	ctx := context.Background()
	res, err := resource.New(ctx,
		resource.WithAttributes(
			// the service name used to display traces in backends
			semconv.ServiceName(serviceName),
		),
	)

	endpoint, _ := os.LookupEnv("OTEL_EXPORTER_JAEGER_ENDPOINT")

	traceExporter, err := otlptracehttp.New(ctx,
		otlptracehttp.WithInsecure(),
		otlptracehttp.WithEndpoint(endpoint),
	)
	if err != nil {
		return err
	}

	bsp := sdktrace.NewBatchSpanProcessor(traceExporter)
	tracerProvider := sdktrace.NewTracerProvider(
		sdktrace.WithSampler(sdktrace.AlwaysSample()),
		sdktrace.WithResource(res),
		sdktrace.WithSpanProcessor(bsp),
	)

	t.tracerProvider = tracerProvider
	// defer func() {
	// 	if err := tracerProvider.Shutdown(ctx); err != nil {
	// 		return err
	// 	}
	// }()
	otel.SetTracerProvider(tracerProvider)

	// tracer := otel.Tracer("test-tracer")
	// ctx, span := tracer.Start(
	// 	ctx,
	// 	"CollectorExporter-Example",
	// )
	// defer span.End()
	// for i := 0; i < 10; i++ {
	// 	_, iSpan := tracer.Start(ctx, fmt.Sprintf("Sample-%d", i))
	// 	log.Printf("Doing really hard work (%d / 10)\n", i+1)

	// 	<-time.After(time.Second)
	// 	iSpan.End()
	// }

	// log.Printf("Done!")
	return nil
}

// AddSpan implements OtelTracer.
func (t *otelTracerImp) StartSpan(ctx context.Context, tracer trace.Tracer, spanName string) (context.Context, func()) {
	ctx, span := tracer.Start(
		ctx,
		spanName,
	)
	return ctx, func() {
		span.End()
	}
	//defer span.End()
}

// CreateTracer implements OtelTracer.
func (t *otelTracerImp) CreateTracer(name string) trace.Tracer {
	return t.tracerProvider.Tracer(name)
}
