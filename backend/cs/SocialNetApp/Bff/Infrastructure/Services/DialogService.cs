using Auths;
using AutoMapper;
using Bff.API.Dtos;
using Bff.Infrastructure.gRpc.Services.Interfaces;
using Bff.Infrastructure.Services.Interfaces;
using DialogGrpc;
using FriendGrpc;
using Grpc.Net.Client;
using PostGrpc;
using ProfileGrpc;
using RabbitMQ.Client;
using System.Diagnostics;
using static DialogGrpc.Dialog;
using static FriendGrpc.Friend;
using static PostGrpc.Post;
using static ProfileGrpc.Users;

namespace Bff.Infrastructure.gRpc.Services
{
    public class DialogService : IDialogService
    {
        private readonly IMapper _mapper;
        private readonly IGrpcChannelsProvider _channelsProvider;

        public DialogService(IMapper mapper, IGrpcChannelsProvider channelsProvider)
        {
            _mapper = mapper;
            _channelsProvider = channelsProvider;
        }

        public async Task<MessageDto> CreateMessageAsync(uint authorId, uint userId, string text, CancellationToken cancellationToken)
        {
            var dialogClient = new DialogClient(_channelsProvider.GetDialogsChannel());
            var reply = await dialogClient.CreateMessageAsync(new CreateMessageRequest { AuthorId = authorId, UserId = userId, Text = text});

            return new MessageDto
            {
                AuthorId = reply.AuthorId,
                Id = reply.UserId,
                Message = reply.Text,
                Created = reply.Created.ToDateTime(),
            };
        }

        public async Task<UserMessagesDto> GetMessagesAsync(uint authorId, uint userId, CancellationToken cancellationToken)
        {
            var userClient = new UsersClient(_channelsProvider.GetUsersChannel());
            var dialogClient = new DialogClient(_channelsProvider.GetDialogsChannel());
            var user = await userClient.GetUserByIdAsync(new GetUserByIdRequest { Id = userId });

            var reply = dialogClient.GetMessages(new GetMessagesRequest { AuthorId = authorId, UserId = userId }, cancellationToken: cancellationToken);
            
            return new UserMessagesDto { 
                User = _mapper.Map<UserDto>(user), 
                Messages = reply.Messages.Select(e => new MessageDto
                {
                    AuthorId = e.AuthorId,
                    Id = e.UserId,
                    Message = e.Text,
                    Created = e.Created.ToDateTime(),
                })
            };
        }
    }
}
