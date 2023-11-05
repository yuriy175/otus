package mq

import (
	"context"
	"log"
	"os"
	"strconv"
	"time"

	amqp "github.com/rabbitmq/amqp091-go"
)

type mqSenderImp struct {
}

func NewMqSender() MqSender {
	return &mqSenderImp{}
}

func (s *mqSenderImp) SendPost(_ context.Context, userId uint, post string) error {
	conn, err := amqp.Dial(os.Getenv("RABBITMQ_CONNECTION"))
	if err != nil {
		return err
	}
	defer conn.Close()

	ch, err := conn.Channel()
	if err != nil {
		return err
	}
	defer ch.Close()

	err = ch.ExchangeDeclare(
		channelName, // name
		"topic",     // type
		false,       // no durable
		false,       // auto-deleted
		false,       // internal
		false,       // no-wait
		nil,         // arguments
	)
	if err != nil {
		return err
	}

	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	routingKey := strconv.Itoa(int(userId))
	err = ch.PublishWithContext(ctx,
		channelName, // exchange
		routingKey,  // routing key
		false,       // mandatory
		false,       // immediate
		amqp.Publishing{
			ContentType: "text/plain",
			Body:        []byte(post),
		})

	if err != nil {
		return err
	}

	log.Printf(" [x] Sent %s", post)
	return nil
}
