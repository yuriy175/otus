package model

// User represents model for a user
type User struct {
	ID      uint    `json:"id"`
	Name    string  `json:"name"`
	Surname string  `json:"surname"`
	Age     *uint8  `json:"age"`
	Sex     *byte   `json:"sex"`
	City    *string `json:"city"`
	Info    *string `json:"info"`
}
