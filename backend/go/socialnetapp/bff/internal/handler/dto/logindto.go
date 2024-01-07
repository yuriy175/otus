package dto

// LoginUserDto represents dto for a user login
type LoginUserDto struct {
	ID       uint   `json:"id"`
	Password string `json:"password"`
}
