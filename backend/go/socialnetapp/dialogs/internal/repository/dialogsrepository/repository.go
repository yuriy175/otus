package dialogsrepository

import (
	"context"
	"database/sql"
	"os"

	_ "github.com/lib/pq"
	"socialnerworkapp.com/dialogs/internal/model"
	"socialnerworkapp.com/dialogs/internal/repository"
	commonmodel "socialnerworkapp.com/pkg/common/model"
)

type dialogsRepositoryImp struct{}

func NewDialogsRepository() repository.DialogsRepository {
	return &dialogsRepositoryImp{}
}

func (r *dialogsRepositoryImp) CreateMessage(_ context.Context, authorId uint, userId uint, text string) (*model.Message, error) {
	db, err := r.connectSql()
	if err != nil {
		return nil, err
	}
	defer db.Close()

	rows, err := db.Query(
		"INSERT INTO public.dialogs(author_id, user_id, message) "+
			"VALUES ($1, $2, $3) "+
			"RETURNING author_id, user_id, message, created",
		authorId, userId, text)

	message := &model.Message{}

	for rows.Next() {
		err := rows.Scan(&message.UserID, &message.AuthorId, &message.Text, &message.Created)
		if err != nil {
			return nil, err
		}
		return message, nil
	}

	return nil, commonmodel.NotFoundError{}
}

func (r *dialogsRepositoryImp) GetMessages(_ context.Context, userId1 uint, userId2 uint) ([]model.Message, error) {
	db, err := r.connectSql()
	if err != nil {
		return nil, err
	}
	defer db.Close()
	rows, err := db.Query(
		"SELECT user_id as userId, author_id as authorId, message as text, created "+
			"FROM public.dialogs "+
			"where author_id = $1 and user_id = $2 "+
			"UNION ALL "+
			"SELECT user_id as userId1, author_id as authorId, message as text, created "+
			"FROM public.dialogs "+
			"where author_id = $2 and user_id = $1;",
		userId1, userId2)
	if err != nil {
		return nil, err
	}
	defer rows.Close()
	messages := []model.Message{}

	for rows.Next() {
		m := model.Message{}
		err := rows.Scan(&m.UserID, &m.AuthorId, &m.Text, &m.Created)
		if err != nil {
			return nil, err
		}
		messages = append(messages, m)
	}
	return messages, nil
}

func (r *dialogsRepositoryImp) GetBuddyIds(_ context.Context, userId uint) ([]uint, error) {
	db, err := r.connectSql()
	if err != nil {
		return nil, err
	}
	defer db.Close()
	// rows, err := db.Query(
	// 	"SELECT friend_id "+
	// 		"FROM public.friends "+
	// 		"WHERE user_id = $1 and \"isDeleted\" = false", userId)
	rows, err := db.Query(
		"SELECT DISTINCT * FROM "+
			"(SELECT author_id "+
			"FROM public.dialogs "+
			"WHERE user_id = $1 "+
			"UNION ALL "+
			"SELECT user_id "+
			"FROM public.dialogs "+
			"WHERE author_id = $1) x", userId)
	if err != nil {
		return nil, err
	}
	defer rows.Close()
	buddyIds := make([]uint, 0)

	for rows.Next() {
		var p uint
		err := rows.Scan(&p)
		if err != nil {
			return nil, err
		}
		buddyIds = append(buddyIds, p)
	}
	return buddyIds, nil
}

func (r *dialogsRepositoryImp) connectSql() (*sql.DB, error) {
	return sql.Open("postgres", os.Getenv("POSTGRESQL_CONNECTION_DB"))
}
