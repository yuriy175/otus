package dto

import "time"

// Post represents dto for a post
type PostDto struct {
	ID       uint       `json:"id"`
	AuthorId uint       `json:"authorId"`
	Message  string     `json:"message"`
	Created  *time.Time `json:"created"`
}
