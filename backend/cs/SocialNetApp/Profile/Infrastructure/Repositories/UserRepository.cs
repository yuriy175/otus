using Dapper;
using Profile.Infrastructure.Repositories.Interfaces;
using SocialNetApp.Core.Model;

namespace Profile.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckUserAsync(uint userId, string password)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT (password_hash = crypt(@password, password_hash)) AS password_match " +
        "FROM users WHERE id = @userId LIMIT 1";
            return await connection.QuerySingleAsync<bool>(sql, new { password, userId = (int)userId });
        }

        public async Task<User> GetUserByIdAsync(uint userId)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT u.id, u.name, surname, age, sex, info, c.name " +
        "FROM public.users u " +
        "LEFT OUTER JOIN public.cities c on c.ID = u.city_id " +
        "WHERE u.id = @userId LIMIT 1;";
            return await connection.QuerySingleAsync<User>(sql, new{ userId = (int)userId });
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT u.id, u.name, surname, age, sex, info, c.name " +
        "FROM public.users u " +
        "LEFT OUTER JOIN public.cities c on c.ID = u.city_id;";
            return await connection.QueryAsync<User>(sql);
        }

        public async Task<int> PutUserAsync(User user, string password)
        {
            using var connection = _context.CreateConnection();
            var sql = "INSERT INTO public.cities(name)	VALUES (@City) ON CONFLICT DO NOTHING; " +
        "INSERT INTO public.users(name, surname, age, sex, info, password_hash, city_id) " +
            "VALUES (@Name, @Surname, @Age, @Sex, @Info,crypt(@password, gen_salt('md5'))," +
            "(SELECT id FROM public.cities WHERE name = @City)) RETURNING id;";
            return await connection.QuerySingleAsync<int>(sql,
                new { user.Name, user.Surname, user.Age, user.Sex, user.Info, password, user.City });
        }
    }
}
