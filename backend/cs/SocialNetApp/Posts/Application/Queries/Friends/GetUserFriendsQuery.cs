using StackExchange.Redis;
using MediatR;
using Posts.Infrastructure.Repositories.Interfaces;

namespace Posts.Application.Queries.Friends
{
    public class GetUserFriendsQuery : IRequest<IEnumerable<int>>
    {
        public uint UserId { get; init; }
        public class GetUserFriendsQueryHandler : IRequestHandler<GetUserFriendsQuery, IEnumerable<int>>
        {
            private readonly IFriendsRepository _friendsRepository;
            public GetUserFriendsQueryHandler(IFriendsRepository friendsRepository)
            {
                _friendsRepository = friendsRepository;
            }
            public Task<IEnumerable<int>> Handle(GetUserFriendsQuery query, CancellationToken cancellationToken)
            {
                return _friendsRepository.GetFriendIdsAsync(query.UserId, cancellationToken);
            }
        }
    }
}
