package friendrepository

import (
	"context"
	"database/sql"
	"os"

	_ "github.com/lib/pq"
	"socialnerworkapp.com/posts/internal/repository"
)

type friendRepositoryImp struct{}

func NewFriendRepository() repository.FriendRepository {
	return &friendRepositoryImp{}
}

func (r *friendRepositoryImp) UpsertFriend(_ context.Context, userId uint, friendId uint) error {
	db, err := r.connectSql()
	if err != nil {
		return err
	}
	defer db.Close()

	_, err = db.Exec(
		"INSERT INTO public.friends(user_id, friend_id, \"isDeleted\") VALUES($1, $2, false) "+
			"ON CONFLICT(user_id, friend_id) DO UPDATE SET \"isDeleted\" = false",
		userId, friendId)
	return err
}

func (r *friendRepositoryImp) DeleteFriend(_ context.Context, userId uint, friendId uint) error {
	db, err := r.connectSql()
	if err != nil {
		return err
	}
	defer db.Close()

	_, err = db.Exec(
		"UPDATE public.friends SET \"isDeleted\"=true "+
			"WHERE user_id = $1 and friend_id = $2",
		userId, friendId)
	return err
}

func (r *friendRepositoryImp) GetSubscriberIds(_ context.Context, userId uint) ([]uint, error) {
	db, err := r.connectSql()
	if err != nil {
		return nil, err
	}
	defer db.Close()
	rows, err := db.Query(
		"SELECT user_id "+
			"FROM public.friends "+
			"WHERE friend_id = $1 and \"isDeleted\" = false", userId)
	if err != nil {
		return nil, err
	}
	defer rows.Close()
	subscriberIds := make([]uint, 0)

	for rows.Next() {
		var p uint
		err := rows.Scan(&p)
		if err != nil {
			return nil, err
		}
		subscriberIds = append(subscriberIds, p)
	}
	return subscriberIds, nil
}

func (r *friendRepositoryImp) connectSql() (*sql.DB, error) {
	return sql.Open("postgres", os.Getenv("POSTGRESQL_CONNECTION"))
}
