using SocialNetApp.Core.Model;

namespace Profile.Core.Model.Interfaces
{
    public interface ILoadDataService
    {
        Task<int> LoadCitiesAsync();
        Task<int> LoadUsersAsync();
    }
}
