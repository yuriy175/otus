FROM golang:alpine AS builder

ENV GOOS linux
WORKDIR /build

COPY backend/go/socialnetapp/. .

RUN CGO_ENABLED=0 go build -o posts posts/cmd/main.go

FROM golang:alpine AS final

WORKDIR /build

COPY --from=builder /build/posts /build/posts

CMD ["/build/posts/main"]