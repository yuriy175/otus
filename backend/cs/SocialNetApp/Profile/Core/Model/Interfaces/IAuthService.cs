using SocialNetApp.Core.Model;

namespace Profile.Core.Model.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(uint userId ,string password, CancellationToken cancellationToken);
    }
}
