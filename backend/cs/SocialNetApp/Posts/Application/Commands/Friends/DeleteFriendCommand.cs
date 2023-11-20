using MediatR;
using Posts.Application.Commands.Posts;
using Posts.Infrastructure.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Posts.Application.Commands.Friends
{
    public class DeleteFriendCommand : IRequest<int>
    {
        public uint UserId { get; init; }
        public uint FriendId { get; init; }

        public class DeleteFriendCommandHandler : IRequestHandler<DeleteFriendCommand, int>
        {
            private readonly IFriendsRepository _friendsRepository;
            public DeleteFriendCommandHandler(IFriendsRepository friendsRepository)
            {
                _friendsRepository = friendsRepository;
            }
            public Task<int> Handle(DeleteFriendCommand command, CancellationToken cancellationToken) =>
                _friendsRepository.DeleteFriendAsync(command.UserId, command.FriendId, cancellationToken);            
        }
    }
}
