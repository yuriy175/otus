package dto

// SearchUserDto represents dto for finding a user
type SearchUserDto struct {
	Name    string `json:"first_name"`
	Surname string `json:"last_name"`
}
