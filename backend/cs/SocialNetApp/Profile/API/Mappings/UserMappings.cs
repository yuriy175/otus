using SocialNetApp.Core.Model;
using AutoMapper;
using SocialNetApp.API.Dtos;

namespace Profile.API.Mappings
{
    public class UserMappings : AutoMapper.Profile
    {
        public UserMappings()
        {
            // NewUserDto -> User
            CreateMap<NewUserDto, User>();
        }
    }
}
