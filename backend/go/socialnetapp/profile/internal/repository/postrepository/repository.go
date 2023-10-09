package postrepository

import (
	"context"
	"database/sql"
	"os"

	_ "github.com/lib/pq"
	"socialnerworkapp.com/profile/internal/model"
	"socialnerworkapp.com/profile/internal/repository"
)

type postRepositoryImp struct{}

func NewPostRepository() repository.PostRepository {
	return &postRepositoryImp{}
}

func (r *postRepositoryImp) AddPost(_ context.Context, userId uint, text string) (*model.Post, error) {
	db, err := r.connectSql()
	if err != nil {
		return nil, err
	}
	defer db.Close()

	rows, err := db.Query(
		"INSERT INTO public.posts(user_id, message, \"isDeleted\") VALUES($1, $2, false) "+
			"RETURNING id, user_id as authorId, message, created",
		userId, text)

	post := &model.Post{}

	for rows.Next() {
		err := rows.Scan(&post.ID, &post.AuthorId, &post.Message, &post.Created)
		if err != nil {
			return nil, err
		}
		return post, nil
	}

	return nil, model.NotFoundError{}
}

func (r *postRepositoryImp) UpdatePost(_ context.Context, userId uint, postId uint, text string) error {
	db, err := r.connectSql()
	if err != nil {
		return err
	}
	defer db.Close()

	_, err = db.Exec(
		"UPDATE public.posts SET message=$2 "+
			"WHERE id = $1",
		postId, text)
	return err
}

func (r *postRepositoryImp) DeletePost(_ context.Context, userId uint, postId uint) error {
	db, err := r.connectSql()
	if err != nil {
		return err
	}
	defer db.Close()

	_, err = db.Exec(
		"UPDATE public.posts SET \"isDeleted\"=true "+
			"WHERE id = $1",
		postId)
	return err
}

func (r *postRepositoryImp) GetPost(_ context.Context, userId uint, postId uint) (*model.Post, error) {
	db, err := r.connectSql()
	if err != nil {
		return nil, err
	}
	defer db.Close()

	rows, err := db.Query(
		"SELECT id, user_id as authorId, message, created "+
			"FROM public.posts "+
			"WHERE id = $1 and \"isDeleted\"!=true",
		postId)

	post := &model.Post{}

	for rows.Next() {
		err := rows.Scan(&post.ID, &post.AuthorId, &post.Message, &post.Created)
		if err != nil {
			return nil, err
		}
		return post, nil
	}

	return nil, model.NotFoundError{}
}

func (r *postRepositoryImp) GetPosts(_ context.Context, userId uint, offset uint, limit uint) ([]model.Post, error) {
	db, err := r.connectSql()
	if err != nil {
		return nil, err
	}
	defer db.Close()
	rows, err := db.Query(
		"SELECT id, user_id as authorId, message, created "+
			"FROM public.posts "+
			"WHERE \"isDeleted\"!=true "+
			"LIMIT $2 OFFSET $1", offset, limit)
	if err != nil {
		return nil, err
	}
	defer rows.Close()
	posts := []model.Post{}

	for rows.Next() {
		p := model.Post{}
		err := rows.Scan(&p.ID, &p.AuthorId, &p.Message, &p.Created)
		if err != nil {
			return nil, err
		}
		posts = append(posts, p)
	}
	return posts, nil
}

func (r *postRepositoryImp) GetLatestFriendsPosts(_ context.Context, userId uint, limit uint) ([]model.Post, error) {
	db, err := r.connectSql()
	if err != nil {
		return nil, err
	}
	defer db.Close()
	rows, err := db.Query(
		"SELECT id, user_id as authorId, message, created "+
			"FROM public.posts p "+
			"WHERE user_id IN( "+
			"SELECT friend_id "+
			"FROM public.friends f "+
			"WHERE f.user_id = $1 and f.\"isDeleted\" = false "+
			") and p.\"isDeleted\" = false "+
			"ORDER BY id DESC LIMIT $2", userId, limit)
	if err != nil {
		return nil, err
	}
	defer rows.Close()
	posts := []model.Post{}

	for rows.Next() {
		p := model.Post{}
		err := rows.Scan(&p.ID, &p.AuthorId, &p.Message, &p.Created)
		if err != nil {
			return nil, err
		}
		posts = append(posts, p)
	}
	return posts, nil
}

func (r *postRepositoryImp) connectSql() (*sql.DB, error) {
	return sql.Open("postgres", os.Getenv("POSTGRESQL_CONNECTION"))
}
