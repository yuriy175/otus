package userrepository

import (
	"context"
	"database/sql"
	"os"

	_ "github.com/lib/pq"
	commonmodel "socialnerworkapp.com/pkg/common/model"
	"socialnerworkapp.com/profile/internal/model"
	"socialnerworkapp.com/profile/internal/repository"

	"github.com/jackc/pgx/v5"
)

type userRepositoryImp struct{}

func NewUserRepository() repository.UserRepository {
	return &userRepositoryImp{}
}

func (r *userRepositoryImp) GetUsers(_ context.Context) ([]model.User, error) {
	db, err := r.connectSql()
	if err != nil {
		return nil, err
	}
	defer db.Close()
	rows, err := db.Query("SELECT u.id, u.name, surname, age, sex, info, c.name " +
		"FROM public.users u " +
		"LEFT OUTER JOIN public.cities c on c.ID = u.city_id;")
	if err != nil {
		return nil, err
	}
	defer rows.Close()
	users := []model.User{}

	for rows.Next() {
		p := model.User{}
		err := rows.Scan(&p.ID, &p.Name, &p.Surname, &p.Age, &p.Sex, &p.Info, &p.City)
		if err != nil {
			return nil, err
		}
		users = append(users, p)
	}
	return users, nil
}

func (r *userRepositoryImp) GetUserById(ctx context.Context, userId uint) (*model.User, error) {
	/*conn, err := r.connectPgx(ctx)
	if err != nil {
		return nil, err
	}
	defer conn.Close(ctx)

	user := &model.User{}
	err = conn.QueryRow(ctx, "SELECT u.id, u.name, surname, age, sex, info, c.name "+
		"FROM public.users u "+
		"LEFT OUTER JOIN public.cities c on c.ID = u.city_id "+
		"WHERE u.id = $1 LIMIT 1;", userId).Scan(&user.ID, &user.Name, &user.Surname, &user.Age, &user.Sex, &user.Info, &user.City)
	if err != nil {
		return user, nil
	}

	return nil, err*/

	db, err := r.connectSql()
	defer db.Close()
	rows, err := db.Query("SELECT u.id, u.name, surname, age, sex, info, c.name "+
		"FROM public.users u "+
		"LEFT OUTER JOIN public.cities c on c.ID = u.city_id "+
		"WHERE u.id = $1 LIMIT 1;", userId)
	if err != nil {
		return nil, err
	}
	defer rows.Close()
	user := &model.User{}

	for rows.Next() {
		err := rows.Scan(&user.ID, &user.Name, &user.Surname, &user.Age, &user.Sex, &user.Info, &user.City)
		if err != nil {
			return nil, err
		}
		return user, nil
	}

	return nil, commonmodel.NotFoundError{}
}

func (r *userRepositoryImp) PutUser(_ context.Context, user *model.User, hash model.PasswordType) error {
	db, err := r.connectSql()
	if err != nil {
		return err
	}
	defer db.Close()

	// TODO: make one SAFE string here
	if user.City != nil {
		_, err := db.Exec("INSERT INTO public.cities(name)	VALUES ($1) ON CONFLICT DO NOTHING;", user.City)
		if err != nil {
			return err
		}
	}
	rows, err := db.Query(
		"INSERT INTO public.users(name, surname, age, sex, info, password_hash, city_id) "+
			"VALUES ($1, $2, $3, $4, $5,crypt($6, gen_salt('md5')),"+
			"(SELECT id FROM public.cities WHERE name = $7)) RETURNING id;",
		user.Name, user.Surname, user.Age, user.Sex, user.Info, hash, user.City)
	if err != nil {
		return err
	}
	defer rows.Close()

	for rows.Next() {
		err := rows.Scan(&user.ID)
		if err != nil {
			return err
		}
	}

	return nil
}

func (r *userRepositoryImp) CheckUser(_ context.Context, userId uint, hash model.PasswordType) (bool, error) {
	db, err := r.connectSql()
	if err != nil {
		return false, err
	}
	defer db.Close()

	rows, err := db.Query("SELECT (password_hash = crypt($1, password_hash)) AS password_match "+
		"FROM users WHERE id = $2 LIMIT 1", hash, userId)
	if err != nil {
		return false, err
	}
	defer rows.Close()
	exists := false

	for rows.Next() {
		err := rows.Scan(&exists)
		if err != nil {
			return false, err
		}
	}
	return exists, nil
}

func (r *userRepositoryImp) GetUsersByName(ctx context.Context, name string, surname string) ([]model.User, error) {
	db, err := r.connectSql()
	if err != nil {
		return nil, err
	}
	defer db.Close()
	rows, err := db.Query("SELECT u.id, u.name, surname, age, sex, info, c.name "+
		"FROM public.users u "+
		"LEFT OUTER JOIN public.cities c on c.ID = u.city_id "+
		"WHERE u.name LIKE $1 and surname LIKE $2 "+
		"ORDER BY id", name, surname)
	if err != nil {
		return nil, err
	}
	defer rows.Close()
	users := []model.User{}

	for rows.Next() {
		p := model.User{}
		err := rows.Scan(&p.ID, &p.Name, &p.Surname, &p.Age, &p.Sex, &p.Info, &p.City)
		if err != nil {
			return nil, err
		}
		users = append(users, p)
	}
	return users, nil
}

func (r *userRepositoryImp) connectSql() (*sql.DB, error) {
	return sql.Open("postgres", os.Getenv("POSTGRESQL_CONNECTION"))
}

func (r *userRepositoryImp) connectPgx(ctx context.Context) (*pgx.Conn, error) {
	return pgx.Connect(ctx, os.Getenv("POSTGRESQL_CONNECTION"))
}
