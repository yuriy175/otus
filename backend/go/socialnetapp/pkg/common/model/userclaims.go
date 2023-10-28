package model

import "errors"

// UserClaims represents model for a user claim
type UserClaims struct {
	UserId string
	//Role string
}

// Valid checks if claim is valid
func (c *UserClaims) Valid() error {
	if c.UserId != "" {
		return nil
	}

	return errors.New("wrong UserClaims")
}
