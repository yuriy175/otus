using Auths;
using AutoMapper;
using Bff.API.Dtos;
using Bff.Infrastructure.gRpc.Services.Interfaces;
using Bff.Infrastructure.Services.Interfaces;
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

namespace Bff.Infrastructure.gRpc.Services
{
    public class PostService : IPostService
    {
        private readonly IMapper _mapper;
        private readonly IGrpcChannelsProvider _channelsProvider;

        public PostService(IMapper mapper, IGrpcChannelsProvider channelsProvider)
        {
            _mapper = mapper;
            _channelsProvider = channelsProvider;
        }

        public async Task CreatePostAsync(uint userId, string text, CancellationToken cancellationToken)
        {
            var postClient = new PostClient(_channelsProvider.GetPostsChannel());
            await postClient.CreatePostAsync(new CreatePostRequest { UserId = userId, Text = text});
        }

        public async Task<UserPostsDto> FeedPostsAsync(uint userId, uint offset, uint limit, CancellationToken cancellationToken)
        {
            var userClient = new UsersClient(_channelsProvider.GetUsersChannel());
            var postClient = new PostClient(_channelsProvider.GetPostsChannel());
            
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

            var authors = new List< UserDto>{ };
            var authorIds = posts.Select(p => p.AuthorId).Distinct().ToList();
            foreach(var authorId in authorIds)
            {
                var user = await userClient.GetUserByIdAsync(new GetUserByIdRequest { Id = authorId });
                authors.Add(_mapper.Map<UserDto>(user));
            }

            return new UserPostsDto { Authors = authors, Posts = posts};
        }
    }
}
