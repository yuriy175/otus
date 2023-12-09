package dto

// UserDto represents dto for a user
type UserDto struct {
	ID      uint    `json:"id"`
	Name    string  `json:"name"`
	Surname string  `json:"surname"`
	Age     *uint8  `json:"age"`
	Sex     *string `json:"sex"`
	City    *string `json:"city"`
	Info    *string `json:"info"`
}
