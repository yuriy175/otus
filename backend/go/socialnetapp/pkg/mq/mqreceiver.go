package mq

import (
	"context"
	"log"
	"os"
	"strconv"

	amqp "github.com/rabbitmq/amqp091-go"
)

type mqReceiverImp struct {
	conn    *amqp.Connection
	channel *amqp.Channel
	queue   amqp.Queue
}

func NewMqReceiver() MqReceiver {
	return &mqReceiverImp{}
}

func (s *mqReceiverImp) CreateReceiver(_ context.Context) error {
	conn, err := amqp.Dial(os.Getenv("RABBITMQ_CONNECTION"))
	if err != nil {
		return err
	}
	s.conn = conn

	ch, err := conn.Channel()
	if err != nil {
		return err
	}
	s.channel = ch

	err = ch.ExchangeDeclare(
		channelName, // name
		"topic",     // type
		false,       // durable
		false,       // auto-deleted
		false,       // internal
		false,       // no-wait
		nil,         // arguments
	)
	if err != nil {
		return err
	}

	q, err := ch.QueueDeclare(
		"",    // name
		false, // durable
		false, // delete when unused
		true,  // exclusive
		false, // no-wait
		nil,   // arguments
	)
	if err != nil {
		return err
	}
	s.queue = q

	return nil
}

func (s *mqReceiverImp) ReceivePosts(_ context.Context, userId uint, action func(friendId uint, post string)) error {
	routingKey := strconv.Itoa(int(userId))
	err := s.channel.QueueBind(
		s.queue.Name, // queue name
		routingKey,   // routing key
		channelName,  // exchange
		false,
		nil)
	if err != nil {
		return err
	}

	msgs, err := s.channel.Consume(
		s.queue.Name, // queue
		"",           // consumer
		true,         // auto ack
		false,        // exclusive
		false,        // no local
		false,        // no wait
		nil,          // args
	)
	if err != nil {
		return err
	}

	go func() {
		for d := range msgs {
			//log.Printf(" [x] %s", d.Body)
			routingKey, _ := strconv.Atoi(d.RoutingKey)
			action(uint(routingKey), string(d.Body))
			log.Printf(" [x] %s %s", d.RoutingKey, d.Body)
		}
	}()

	return nil
}

func (s *mqReceiverImp) CloseReceiver(_ context.Context) error {
	defer s.conn.Close()
	defer s.channel.Close()

	return nil
}
