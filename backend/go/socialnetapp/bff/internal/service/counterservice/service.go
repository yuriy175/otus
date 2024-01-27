package counterservice

import (
	"context"
	"log"
	"os"

	_ "github.com/google/uuid"
	oteltrace "go.opentelemetry.io/otel/trace"
	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials/insecure"
	"socialnerworkapp.com/bff/internal/service"
	"socialnerworkapp.com/pkg/proto/gen"
	"socialnerworkapp.com/pkg/trace"
)

type counterServiceImp struct {
	grpcCounterUrl string
	tracer         trace.OtelTracer
}

func NewCounterService(tracer trace.OtelTracer) service.CounterService {
	grpcCounterUrl, _ := os.LookupEnv("GRPC_COUNTERS")
	return &counterServiceImp{
		grpcCounterUrl: grpcCounterUrl,
		tracer:         tracer}
}

func (s *counterServiceImp) GetUnReadCounterByUserId(ctx context.Context, tracer oteltrace.Tracer, userId uint) (uint, error) {
	conn, err := grpc.Dial(s.grpcCounterUrl, grpc.WithTransportCredentials(insecure.NewCredentials()))
	if err != nil {
		log.Fatalf("fail to dial: %v", err)
	}
	defer conn.Close()
	ctx, endSpan := s.tracer.StartSpan(ctx, tracer, "counterClient.GetUnReadCounterByUserId")

	counterClient := gen.NewCounterClient(conn)
	getRequest := &gen.GetUnreadCountRequest{UserId: uint32(userId)}
	counterReply, err := counterClient.GetUnreadCount(ctx, getRequest)
	endSpan()
	if err != nil {
		return 0, err
	}

	return uint(counterReply.Count), err
}
