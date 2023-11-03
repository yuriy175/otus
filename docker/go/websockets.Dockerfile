FROM golang:alpine AS builder

ENV GOOS linux
WORKDIR /build

COPY backend/go/socialnetapp/. .

RUN CGO_ENABLED=0 go build -o websockets websockets/cmd/main.go

FROM golang:alpine AS final

WORKDIR /build

COPY --from=builder /build/websockets /build/websockets

CMD ["/build/websockets/main"]