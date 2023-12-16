using Auths;
using AutoMapper;
using Bff.API.Dtos;
using Bff.Infrastructure.gRpc.Services.Interfaces;
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

namespace Profile.Infrastructure.gRpc.Services
{
    public class DialogService : IDialogService
    {
        private readonly static string _grpcDialogsUrl = Environment.GetEnvironmentVariable("GRPC_DIALOGS");
        private readonly static string _grpcUsersUrl = Environment.GetEnvironmentVariable("GRPC_PROFILE");

        private readonly IMapper _mapper;

        private static readonly GrpcChannel _usersChannel = null;
        private static readonly GrpcChannel _dialogsChannel = null;

        static DialogService()
        {
            var options = new GrpcChannelOptions()
            {
                HttpHandler = new SocketsHttpHandler
                {
                    EnableMultipleHttp2Connections = true,
                }
            };
            _dialogsChannel = GrpcChannel.ForAddress(_grpcDialogsUrl, options);
            _usersChannel = GrpcChannel.ForAddress(_grpcUsersUrl, options);

            _dialogsChannel.ConnectAsync().Wait();
            _usersChannel.ConnectAsync().Wait();
        }

        public static Task WarmupChannels(){ return Task.CompletedTask; }


        public DialogService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<MessageDto> CreateMessageAsync(uint authorId, uint userId, string text, CancellationToken cancellationToken)
        {
            var dialogClient = new DialogClient(_dialogsChannel);
            var reply = await dialogClient.CreateMessageAsync(new CreateMessageRequest { AuthorId = authorId, UserId = userId, Text = text});

            return new MessageDto
            {
                AuthorId = reply.AuthorId,
                Id = reply.UserId,
                Message = reply.Text,
            };
        }

        public async Task<UserMessagesDto> GetMessagesAsync(uint authorId, uint userId, CancellationToken cancellationToken)
        {
            var userClient = new UsersClient(_usersChannel);
            var dialogClient = new DialogClient(_dialogsChannel);
            var user = await userClient.GetUserByIdAsync(new GetUserByIdRequest { Id = userId });

            var reply = dialogClient.GetMessages(new GetMessagesRequest { AuthorId = authorId, UserId = userId }, cancellationToken: cancellationToken);
            
            return new UserMessagesDto { 
                User = _mapper.Map<UserDto>(user), 
                Messages = reply.Messages.Select(e => new MessageDto
                {
                    AuthorId = e.AuthorId,
                    Id = e.UserId,
                    Message = e.Text,
                })
            };
        }
    }
}
