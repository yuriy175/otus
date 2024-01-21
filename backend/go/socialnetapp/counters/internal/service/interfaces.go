package service

import (
	"context"
)

type CountersService interface {
	UpdateUnReadCounterByUserId(_ context.Context, userId uint, delta int) (int, error)
	GetUnReadCounterByUserId(_ context.Context, userId uint) (uint, error)
}
