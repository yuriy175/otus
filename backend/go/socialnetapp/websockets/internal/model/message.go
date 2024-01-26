package model

import "time"

// Message represents model for a dialog message
type Message struct {
	Id       uint       `json:"id"`
	AuthorId uint       `json:"authorId"`
	UserId   uint       `json:"userId"`
	Text     string     `json:"message"`
	Created  *time.Time `json:"created"`
}
