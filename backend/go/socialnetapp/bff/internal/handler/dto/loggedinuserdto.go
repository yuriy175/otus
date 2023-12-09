package dto

// LoggedinUserDto represents dto for a loggedin user
type LoggedinUserDto struct {
	User  *UserDto `json:"user"`
	Token string   `json:"token"`
}
