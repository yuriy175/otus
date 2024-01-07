package dto

// UserMessagesDto represents dto for a user dialog messages
type UserMessagesDto struct {
	User     *UserDto      `json:"user"`
	Messages []*MessageDto `json:"messages"`
}
