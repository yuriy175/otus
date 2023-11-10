package friendrepository

import (
	"context"
	"database/sql"
	"os"

	_ "github.com/lib/pq"
	"socialnerworkapp.com/websockets/internal/repository"
)

type friendRepositoryImp struct{}

func NewFriendRepository() repository.FriendRepository {
	return &friendRepositoryImp{}
}

func (r *friendRepositoryImp) GetFriendIdsAsync(_ context.Context, userId uint) ([]uint, error) {
	db, err := r.connectSql()
	if err != nil {
		return nil, err
	}
	defer db.Close()
	rows, err := db.Query(
		"SELECT friend_id "+
			"FROM public.friends "+
			"WHERE user_id = $1 and \"isDeleted\" = false", userId)
	if err != nil {
		return nil, err
	}
	defer rows.Close()
	friendIds := make([]uint, 0)

	for rows.Next() {
		var p uint
		err := rows.Scan(&p)
		if err != nil {
			return nil, err
		}
		friendIds = append(friendIds, p)
	}
	return friendIds, nil
}

func (r *friendRepositoryImp) connectSql() (*sql.DB, error) {
	return sql.Open("postgres", os.Getenv("POSTGRESQL_CONNECTION"))
}
