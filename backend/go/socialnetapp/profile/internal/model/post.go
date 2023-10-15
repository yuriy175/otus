package model

import "time"

// Post represents model for a post
type Post struct {
	ID       uint       `json:"id"`
	AuthorId uint       `json:"authorId"`
	Message  string     `json:"message"`
	Created  *time.Time `json:"created"`
}
