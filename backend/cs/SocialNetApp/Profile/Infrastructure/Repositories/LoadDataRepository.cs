using Dapper;
using Profile.Infrastructure.Repositories.Interfaces;
using SocialNetApp.API.Daos;
using SocialNetApp.Core.Model;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Profile.Infrastructure.Repositories
{
    public class LoadDataRepository : ILoadDataRepository
    {
        private DataContext _context;

        public LoadDataRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<int> LoadCitiesAsync(IEnumerable<string> cityNames)
        {
            using var connection = _context.CreateConnection();
            var seed = new StringBuilder("INSERT INTO public.cities(name) VALUES");
            var sql = cityNames.Aggregate(seed, (s, c) => s.Append($" ('{c}'),"), s => s.Remove(s.Length-1,1).ToString());
            
            return await connection.ExecuteAsync(sql);
        }

        public async Task<int> LoadUsersAsync(IEnumerable<NewUserDao> users)
        {
            using var connection = _context.CreateConnection();
            //too slow case
            /*
            var sql = "INSERT INTO public.users(name, surname, age, sex, info, password_hash, city_id) " +
            "VALUES (@Name, @Surname, @Age, @Sex, @Info,crypt(@Password, gen_salt('md5'))," +
            "(SELECT id FROM public.cities WHERE name = @City));";
            
            var rowsAffected = await connection.ExecuteAsync(sql, users);
            */
            var seed = new StringBuilder("INSERT INTO public.users(name, surname, age, password_hash, city_id) VALUES");
            var sql = users.Aggregate(seed, 
                (s, u) => s.Append($" ('{u.Name}', '{u.Surname}', {u.Age}, crypt('{u.Password}', gen_salt('md5')), (SELECT id FROM public.cities WHERE name = '{u.City}')),"), s => s.Remove(s.Length - 1, 1).ToString());


            return await connection.ExecuteAsync(sql);
        }
    }
}
