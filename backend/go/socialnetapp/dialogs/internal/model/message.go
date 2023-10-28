package model

import "time"

// Message represents model for a dialog message
type Message struct {
	AuthorId uint       `json:"authorId"`
	UserID   uint       `json:"userId"`
	Text     string     `json:"message"`
	Created  *time.Time `json:"created"`
}
