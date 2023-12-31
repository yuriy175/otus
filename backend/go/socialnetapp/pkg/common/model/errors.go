package model

type NotFoundError struct {
	Name string
}

func (e NotFoundError) Error() string { return e.Name + ": not found" }

type NotAuthorizedError struct {
	Name string
}

func (e NotAuthorizedError) Error() string { return e.Name + ": not authorized" }
