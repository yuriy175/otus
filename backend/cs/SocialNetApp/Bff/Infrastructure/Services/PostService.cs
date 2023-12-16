using Auths;
using AutoMapper;
using Bff.API.Dtos;
using Bff.Infrastructure.gRpc.Services.Interfaces;
using FriendGrpc;
using Grpc.Core;
using Grpc.Net.Client;
using PostGrpc;
using ProfileGrpc;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Diagnostics;
using static FriendGrpc.Friend;
using static PostGrpc.Post;
using static ProfileGrpc.Users;
using static System.Net.Mime.MediaTypeNames;

namespace Profile.Infrastructure.gRpc.Services
{
    public class PostService : IPostService
    {
        private readonly static string _grpcPostsUrl = Environment.GetEnvironmentVariable("GRPC_POSTS");
        private readonly static string _grpcUsersUrl = Environment.GetEnvironmentVariable("GRPC_PROFILE");

        private readonly IMapper _mapper;

        private static readonly GrpcChannel _usersChannel = null;
        private static readonly GrpcChannel _postsChannel = null;

        static PostService()
        {
            var options = new GrpcChannelOptions()
            {
                HttpHandler = new SocketsHttpHandler
                {
                    EnableMultipleHttp2Connections = true,
                }
            };
            _postsChannel = GrpcChannel.ForAddress(_grpcPostsUrl, options);
            _usersChannel = GrpcChannel.ForAddress(_grpcUsersUrl, options);

            _postsChannel.ConnectAsync().Wait();
            _usersChannel.ConnectAsync().Wait();
        }

        public static Task WarmupChannels(){ return Task.CompletedTask; }

        public PostService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task CreatePostAsync(uint userId, string text, CancellationToken cancellationToken)
        {
            var postClient = new PostClient(_postsChannel);
            await postClient.CreatePostAsync(new CreatePostRequest { UserId = userId, Text = text});
        }

        public async Task<UserPostsDto> FeedPostsAsync(uint userId, uint offset, uint limit, CancellationToken cancellationToken)
        {
            var userClient = new UsersClient(_usersChannel);
            var postClient = new PostClient(_postsChannel);
            var user = await userClient.GetUserByIdAsync(new GetUserByIdRequest { Id = userId });
            
            using var call = postClient.FeedPosts(new FeedPostsRequest { UserId = userId, Offset = offset, Limit = limit}, cancellationToken: cancellationToken);
            var posts = new List<PostDto>();
            while (await call.ResponseStream.MoveNext())
            {
                var post = call.ResponseStream.Current;
                posts.Add(new PostDto
                {
                    AuthorId = post.AuthorId,
                    Id = post.UserId,
                    Message = post.Message,
                });
            }

            return new UserPostsDto { User = _mapper.Map<UserDto>(user), Posts = posts};
        }
    }
}
