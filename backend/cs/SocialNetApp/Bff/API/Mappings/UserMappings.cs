using Bff.API.Dtos;
using ProfileGrpc;

namespace Bff.API.Mappings
{
    public class UserMappings : AutoMapper.Profile
    {
        public UserMappings()
        {
            // UserReply -> UserDto
            CreateMap<UserReply, UserDto>();
        }
    }
}

// ToUserDto(UserReply user) => new UserDto
//{
//    City = user.City,
//    Id = user.Id,
//    Info = user.Info,
//    Name = user.Name,
//    Sex = user.Sex,
//    Surname = user.Surname,
//    Age = user.Age.HasValue ? (byte)user.Age.Value : null as byte?,
//};