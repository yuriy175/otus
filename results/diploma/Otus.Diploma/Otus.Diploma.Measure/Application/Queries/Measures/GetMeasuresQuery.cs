using Model = Measure.Core.Model;

namespace Measure.Application.Queries.Measures
{
    /*public class GetMeasuresQuery : IRequest<Model.Measure>
    {
        public uint UserId { get; init; }
        public uint PostId { get; init; }
        public class GetMeasuresQueryHandler : IRequestHandler<GetMeasuresQuery, Post>
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
    }*/
}
