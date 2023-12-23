using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using PostGrpc;
using Posts;
using Posts.Application.Commands.Posts;
using Posts.Application.Queries.Posts;
using System.Collections.Generic;
using static PostGrpc.Post;
using static System.Net.Mime.MediaTypeNames;

namespace Posts.Infrastructure.gRpc.Services
{
    public class PostService : PostBase
    {
        private readonly IMediator _mediator;

        public PostService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<Empty> CreatePost(CreatePostRequest request, ServerCallContext context)
        {
            await _mediator.Send(new AddPostCommand { UserId = request.UserId, Text = request.Text });
            return new Empty();
        }

        public override async Task FeedPosts(FeedPostsRequest request, IServerStreamWriter<PostReply> responseStream, ServerCallContext context)
        {
            var posts = await _mediator.Send(new FeedPostsQuery { UserId = request.UserId, Offset = request.Offset, Limit = request.Limit });
            foreach (var post in posts)
            {
                try
                {
                    await responseStream.WriteAsync(new PostReply
                    {
                        UserId = post.Id,
                        AuthorId = post.AuthorId,
                        Message = post.Message,
                        Created = post.Created.HasValue ? Timestamp.FromDateTime(
                            DateTime.SpecifyKind(post.Created.Value, DateTimeKind.Utc)) : new Timestamp(),
                    });
                }
                catch(Exception ex)
                {
                    var u = 0;
                }
            }
        }
    }
}
