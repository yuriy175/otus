FROM golang:alpine AS builder

ENV GOOS linux
WORKDIR /build

COPY backend/go/socialnetapp/. .

RUN CGO_ENABLED=0 go build -o counters counters/cmd/main.go

FROM golang:alpine AS final

WORKDIR /build

COPY --from=builder /build/counters /build/counters

CMD ["/build/counters/main"]