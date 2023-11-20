using MediatR;
using Posts.Application.Commands.Friends;
using Posts.Infrastructure.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Posts.Application.Commands.Posts
{
    public class DeletePostCommand : IRequest<int>
    {
        public uint UserId { get; init; }
        public uint PostId { get; init; }
        public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, int>
        {
            private readonly IPostsRepository _postsRepository;
            public DeletePostCommandHandler(IPostsRepository postsRepository)
            {
                _postsRepository = postsRepository;
            }
            public Task<int> Handle(DeletePostCommand command, CancellationToken cancellationToken) =>
                _postsRepository.DeletePostAsync(command.UserId, command.PostId, cancellationToken);
        }
    }
}
