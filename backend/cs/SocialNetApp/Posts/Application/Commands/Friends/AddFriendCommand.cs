using MediatR;
using Posts.Infrastructure.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Posts.Application.Commands.Friends
{
    public class AddFriendCommand : IRequest<int>
    {
        public uint UserId { get; init; }
        public uint FriendId { get; init; }
        public class AddFriendCommandHandler : IRequestHandler<AddFriendCommand, int>
        {
            private readonly IFriendsRepository _friendsRepository;
            public AddFriendCommandHandler(IFriendsRepository friendsRepository)
            {
                _friendsRepository = friendsRepository;
            }
            public Task<int> Handle(AddFriendCommand command, CancellationToken cancellationToken) =>
                _friendsRepository.UpsertFriendAsync(command.UserId, command.FriendId, cancellationToken);
        }
    }
}
