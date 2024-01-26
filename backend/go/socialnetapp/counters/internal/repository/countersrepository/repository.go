package countersrepository

import (
	"context"
	"database/sql"
	"os"

	_ "github.com/lib/pq"
	"socialnerworkapp.com/counters/internal/repository"
)

type countersRepositoryImp struct{}

func NewCountersRepository() repository.CountersRepository {
	return &countersRepositoryImp{}
}

func (r *countersRepositoryImp) UpdateUnReadCounterByUserId(_ context.Context, userId uint, delta int) (int, error) {
	db, err := r.connectSql("POSTGRESQL_CONNECTION")
	if err != nil {
		return 0, err
	}
	defer db.Close()

	result, err := db.Exec(
		"INSERT INTO counters (user_id, unread_count) "+
			"VALUES($1, $2) "+
			"ON CONFLICT (user_id) "+
			"DO "+
			"UPDATE SET unread_count = ( "+
			"SELECT unread_count + $2  "+
			"FROM counters "+
			"WHERE user_id = $1 "+
			")",
		userId, delta)
	if err != nil {
		return 0, err
	}
	val, err := result.RowsAffected()

	return int(val), err
}

func (r *countersRepositoryImp) GetUnReadCounterByUserId(_ context.Context, userId uint) (uint, error) {
	db, err := r.connectSql("POSTGRESQL_READ_CONNECTION")
	if err != nil {
		return 0, err
	}
	defer db.Close()
	rows, err := db.Query(
		"SELECT unread_count FROM public.counters WHERE user_id = $1",
		userId)
	if err != nil {
		return 0, err
	}
	defer rows.Close()

	for rows.Next() {
		count := 0
		err := rows.Scan(&count)

		return uint(count), err
	}
	return 0, nil
}

func (r *countersRepositoryImp) connectSql(dataSourceName string) (*sql.DB, error) {
	return sql.Open("postgres", os.Getenv(dataSourceName))
}
