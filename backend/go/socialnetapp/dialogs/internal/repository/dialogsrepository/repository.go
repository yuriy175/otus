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
	db, err := r.connectSql("POSTGRESQL_CONNECTION_DB")
	if err != nil {
		return nil, err
	}
	defer db.Close()

	rows, err := db.Query(
		"INSERT INTO public.dialogs(author_id, user_id, message) "+
			"VALUES ($1, $2, $3) "+
			"RETURNING id, author_id, user_id, message, created",
		authorId, userId, text)

	message := &model.Message{}

	for rows.Next() {
		err := rows.Scan(&message.AuthorId, &message.UserID, &message.Text, &message.Created)
		if err != nil {
			return nil, err
		}
		return message, nil
	}

	return nil, commonmodel.NotFoundError{}
}

func (r *dialogsRepositoryImp) GetMessages(_ context.Context, userId1 uint, userId2 uint) ([]model.Message, error) {
	db, err := r.connectSql("POSTGRESQL_READ_CONNECTION")
	if err != nil {
		return nil, err
	}
	defer db.Close()
	rows, err := db.Query(
		"SELECT id, user_id as userId, author_id as authorId, message as text, created "+
			"FROM public.dialogs "+
			"where author_id = $1 and user_id = $2 "+
			"UNION ALL "+
			"SELECT id, user_id as userId1, author_id as authorId, message as text, created "+
			"FROM public.dialogs "+
			"where author_id = $2 and user_id = $1 "+
			"ORDER BY created DESC;",
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
	db, err := r.connectSql("POSTGRESQL_READ_CONNECTION")
	if err != nil {
		return nil, err
	}
	defer db.Close()

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

	ind := 0
	for rows.Next() {
		var p uint
		err := rows.Scan(&p)
		if err != nil {
			return nil, err
		}
		buddyIds[ind] = p
		ind += 1
	}
	return buddyIds, nil
}

func (r *dialogsRepositoryImp) SetReadDialogMessagesFromUser(_ context.Context, authorId uint, userId uint) ([]int, error) {
	db, err := r.connectSql("POSTGRESQL_CONNECTION_DB")
	if err != nil {
		return nil, err
	}
	defer db.Close()

	rows, err := db.Query(
		"UPDATE public.dialogs "+
			"SET \"isRead\" = true "+
			"WHERE author_id = $1 and user_id = $2 and \"isRead\" = false "+
			" RETURNING id",
		authorId, userId)
	if err != nil {
		return nil, err
	}
	defer rows.Close()
	ids := make([]int, 0)
	ind := 0
	for rows.Next() {
		var p int
		err := rows.Scan(&p)
		if err != nil {
			return nil, err
		}
		ids[ind] = p
		ind += 1
	}
	return ids, nil
}

func (r *dialogsRepositoryImp) SetUnreadDialogMessages(_ context.Context, msgIds []int) (int, error) {
	db, err := r.connectSql("POSTGRESQL_CONNECTION_DB")
	if err != nil {
		return 0, err
	}
	defer db.Close()

	result, err := db.Exec(
		"UPDATE public.dialogs "+
			"SET \"isRead\" = false "+
			"WHERE id = ANY($1)",
		msgIds)

	if err != nil {
		return 0, err
	}
	val, err := result.RowsAffected()

	return int(val), err
}

func (r *dialogsRepositoryImp) connectSql(dataSourceName string) (*sql.DB, error) {
	return sql.Open("postgres", os.Getenv(dataSourceName))
}
