FROM golang:alpine AS builder

ENV GOOS linux
WORKDIR /build

COPY backend/go/socialnetapp/. .

RUN CGO_ENABLED=0 go build -o bff bff/cmd/main.go

FROM golang:alpine AS final

WORKDIR /build

COPY --from=builder /build/bff /build/bff

CMD ["/build/bff/main"]