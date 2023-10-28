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
		"INSERT INTO public.dialogs(user_id_1, user_id_2, author_id, message) "+
			"VALUES ($1, $2, $1, $3) "+
			"RETURNING user_id_1, user_id_2, author_id, message, created",
		authorId, userId, text)

	message := &model.Message{}

	for rows.Next() {
		err := rows.Scan(&message.UserID1, &message.UserID2, &message.AuthorId, &message.Text, &message.Created)
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
		"SELECT user_id_1 as userId1, user_id_2 as userId2, author_id as authorId, message as text, created "+
			"FROM public.dialogs "+
			"where user_id_1 = $1 and user_id_2 = $2 "+
			"UNION ALL "+
			"SELECT user_id_1 as userId1, user_id_2 as userId2, author_id as authorId, message as text, created "+
			"FROM public.dialogs "+
			"where user_id_1 = $2 and user_id_2 = $1;",
		userId1, userId2)
	if err != nil {
		return nil, err
	}
	defer rows.Close()
	messages := []model.Message{}

	for rows.Next() {
		m := model.Message{}
		err := rows.Scan(&m.UserID1, &m.UserID2, &m.AuthorId, &m.Text, &m.Created)
		if err != nil {
			return nil, err
		}
		messages = append(messages, m)
	}
	return messages, nil
}

func (r *dialogsRepositoryImp) connectSql() (*sql.DB, error) {
	return sql.Open("postgres", os.Getenv("POSTGRESQL_CONNECTION_DB"))
}
