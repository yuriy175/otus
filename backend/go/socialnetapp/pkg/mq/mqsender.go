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
		postChannelName, // name
		"topic",         // type
		false,           // no durable
		false,           // auto-deleted
		false,           // internal
		false,           // no-wait
		nil,             // arguments
	)
	if err != nil {
		return err
	}

	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	routingKey := strconv.Itoa(int(userId))
	err = ch.PublishWithContext(ctx,
		postChannelName, // exchange
		routingKey,      // routing key
		false,           // mandatory
		false,           // immediate
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

func (s *mqSenderImp) SendDialogMessage(ctx context.Context, data []byte) error {
	return s.sendDirectMessage(ctx, dialogWebsockQueueName, data)
}

func (s *mqSenderImp) SendUnreadDialogMessageIds(ctx context.Context, data []byte) error {
	return s.sendDirectMessage(ctx, counterQueueName, data)
}

func (s *mqSenderImp) SendUnreadDialogMessageIdsFailed(ctx context.Context, data []byte) error {
	return s.sendDirectMessage(ctx, counterDialogQueueName, data)
}

func (s *mqSenderImp) sendDirectMessage(_ context.Context, queueName string, data []byte) error {
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

	q, err := ch.QueueDeclare(
		queueName, // name
		false,     // durable
		false,     // delete when unused
		false,     // exclusive
		false,     // no-wait
		nil,       // arguments
	)
	if err != nil {
		return err
	}

	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	err = ch.PublishWithContext(ctx,
		"",     // exchange
		q.Name, // routing key
		false,  // mandatory
		false,  // immediate
		amqp.Publishing{
			ContentType: "text/plain",
			Body:        data,
		})
	if err != nil {
		return err
	}

	return nil
}
