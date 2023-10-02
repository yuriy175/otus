package model

import "errors"

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

type UserClaims struct {
	UserId string
	//Role string
}

// Valid checks if claim is valid
func (c *UserClaims) Valid() error {
	if c.UserId == "" {
		return nil
	}

	return errors.New("wrong UserClaims")
}
