package repository

import (
	"context"
)

type CountersRepository interface {
	UpdateUnReadCounterByUserId(_ context.Context, userId uint, delta int) (int, error)
	GetUnReadCounterByUserId(_ context.Context, userId uint) (uint, error)
}
