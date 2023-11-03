package repository

import (
	"context"
)

type FriendRepository interface {
	GetFriendIdsAsync(_ context.Context, userId uint) ([]uint, error)
}
