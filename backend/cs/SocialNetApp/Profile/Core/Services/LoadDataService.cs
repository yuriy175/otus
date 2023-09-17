using AutoMapper;
using Profile.Core.Model.Interfaces;
using Profile.Infrastructure.Repositories.Interfaces;
using SocialNetApp.API.Daos;
using SocialNetApp.Core.Model;
using System.Collections.Concurrent;

namespace Profile.Core.Services
{
    public class LoadDataService : ILoadDataService
    {
        private const string DataFolder = @"..\..\..\..\csvdata";
        private const string CitiesFile = @"cities_raw.csv";
        private const string UsersFile = @"people.csv";
        private const char UserPropsSeparator = ',';
        private const char UserNamesSeparator = ' ';
        private const int ChunkSize = 2000;

        private readonly object _locker = new object();
        private readonly ILoadDataRepository _loadDataRepository;

        public LoadDataService(ILoadDataRepository loadDataRepository)
        {
            _loadDataRepository = loadDataRepository;
        }

        public async Task<int> LoadCitiesAsync()
        {
            var cities = await File.ReadAllLinesAsync(Path.Combine(DataFolder,CitiesFile));
            return await LoadEntitiesAsync(cities.Length, cities, e => _loadDataRepository.LoadCitiesAsync(e));
        }

        public async Task<int> LoadUsersAsync()
        {
            var lines = await File.ReadAllLinesAsync(Path.Combine(DataFolder, UsersFile));
            var users = lines
                .Select(u =>
                {
                    var props = u.Split(UserPropsSeparator);
                    var names = props[0].Split(UserNamesSeparator);
                    return new NewUserDao
                    {
                        Name = names[1],
                        Surname = names[0],
                        Age = Convert.ToByte(props[1]),
                        City = props[2],
                        Password = names[0]
                    };
                });
            return await LoadEntitiesAsync(lines.Length, users, e => _loadDataRepository.LoadUsersAsync(e));
        }

        private async Task<int> LoadEntitiesAsync<T>(int count, IEnumerable<T> entities, Func<IEnumerable<T>, Task<int>> loader)
        {
            var totalCount = 0;
            var rows = 0;
            while (totalCount < count)
            {
                var currentCount = totalCount;
                totalCount += ChunkSize;
                await Task.Run(async () =>
                {
                    var rowsAffected = await loader(entities.Skip(currentCount).Take(ChunkSize));

                    lock (_locker)
                    {
                        rows += rowsAffected;
                    }
                });
            }

            return rows;
        }
    }
}
