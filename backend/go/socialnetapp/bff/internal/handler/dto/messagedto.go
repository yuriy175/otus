package dto

import "time"

// MessageDto represents dto for a dialog message
type MessageDto struct {
	ID       uint       `json:"id"`
	AuthorId uint       `json:"authorId"`
	Message  string     `json:"message"`
	Created  *time.Time `json:"created"`
}
