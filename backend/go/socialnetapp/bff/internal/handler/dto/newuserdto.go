package dto

// NewUserDto represents dto for a newly registered user
type NewUserDto struct {
	Name     string  `json:"name"`
	Surname  string  `json:"surname"`
	Password string  `json:"password"`
	Age      *uint8  `json:"age"`
	Sex      *string `json:"sex"`
	City     *string `json:"city"`
	Info     *string `json:"info"`
}
