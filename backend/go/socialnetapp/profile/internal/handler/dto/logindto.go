package dto

// LoginDto represents dto for a user login
type LoginDto struct {
	ID       uint   `json:"id"`
	Password string `json:"password"`
}
