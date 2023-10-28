package model

import "time"

// Message represents model for a dialog message
type Message struct {
	UserID1  uint       `json:"userId1"`
	UserID2  uint       `json:"userId2"`
	AuthorId uint       `json:"authorId"`
	Text     string     `json:"message"`
	Created  *time.Time `json:"created"`
}
