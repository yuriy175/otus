package dto

import "time"

// MessageDto represents dto for a dialog message
type MessageDto struct {
	ID       uint       `json:"id"`
	AuthorId uint       `json:"authorId"`
	UserId   uint       `json:"userId"`
	Message  string     `json:"message"`
	Created  *time.Time `json:"created"`
}
