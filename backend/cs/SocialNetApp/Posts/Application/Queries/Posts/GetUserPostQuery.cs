using MediatR;
using Posts.Infrastructure.Repositories.Interfaces;
using SocialNetApp.Core.Model;

namespace Posts.Application.Queries.Posts
{
    public class GetUserPostQuery : IRequest<Post>
    {
        public uint UserId { get; init; }
        public uint PostId { get; init; }
        public class GetUserPostQueryHandler : IRequestHandler<GetUserPostQuery, Post>
        {
            private readonly IPostsRepository _postsRepository;
            public GetUserPostQueryHandler(IPostsRepository postsRepository)
            {
                _postsRepository = postsRepository;
            }
            public Task<Post> Handle(GetUserPostQuery query, CancellationToken cancellationToken)
            {
                return _postsRepository.GetPostAsync(query.UserId, query.PostId, cancellationToken);
            }
        }
    }
}
