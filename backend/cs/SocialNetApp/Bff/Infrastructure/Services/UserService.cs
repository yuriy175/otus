﻿using Auths;
using AutoMapper;
using Bff.API.Dtos;
using Bff.Infrastructure.gRpc.Services.Interfaces;
using Bff.Infrastructure.Services.Interfaces;
using Grpc.Core;
using Grpc.Net.Client;
using ProfileGrpc;
using static ProfileGrpc.Users;

namespace Bff.Infrastructure.gRpc.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IGrpcChannelsProvider _channelsProvider;

        public UserService(IMapper mapper, IGrpcChannelsProvider channelsProvider)
        {
            _mapper = mapper;
            _channelsProvider = channelsProvider;
        }

        public async Task<IEnumerable<UserDto>> GetUsersByNameAsync(string name, string surname)
        {
            var client = new UsersClient(_channelsProvider.GetUsersChannel());
            using var call = client.GetUsersByName(new GetUsersByNameRequest
            {
                Name = name,
                Surname = surname
            });
            var users = new List<UserDto>();
            while (await call.ResponseStream.MoveNext())
            {
                users.Add(_mapper.Map<UserDto>(call.ResponseStream.Current));
            }

            return users;
        }

        public async Task<LoggedinUserDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken)
        {
            //var options = new GrpcChannelOptions();
            //using var channel = GrpcChannel.ForAddress(_grpcUrl, options);
            //using var authChannel = GrpcChannel.ForAddress(_grpcUrl, options);
            var client = new UsersClient(_channelsProvider.GetUsersChannel());
            var auth = new Auth.AuthClient(_channelsProvider.GetAuthChannel());

            var token = await auth.LoginAsync(new LoginRequest { Id = dto.Id, Password = dto.Password });
            var user = await client.GetUserByIdAsync(new GetUserByIdRequest { Id = dto.Id });
            var userDto = _mapper.Map<UserDto>(user);
            //    new UserDto
            //{
            //    City = user.City,
            //    Id = user.Id,
            //    Info = user.Info,
            //    Name = user.Name,
            //    Sex = user.Sex,
            //    Surname = user.Surname,
            //    Age = user.Age.HasValue ? (byte)user.Age.Value : null as byte?,
            //};

            return new LoggedinUserDto { User = userDto,
                Token = token.Token,
            };
        }

        public async Task<UserDto> PutUserAsync(NewUserDto dto, string password)
        {
            var client = new UsersClient(_channelsProvider.GetUsersChannel());
            var reply = await client.AddUserAsync(new AddUserRequest { 
                User = new UserReply
            {
                City = dto.City,
                Info = dto.Info,
                Name = dto.Name,
                Sex = dto.Sex,
                Surname = dto.Surname,
                Age = dto.Age
            }, Password = password});

            return _mapper.Map<UserDto>(reply);
        }
    }
}
