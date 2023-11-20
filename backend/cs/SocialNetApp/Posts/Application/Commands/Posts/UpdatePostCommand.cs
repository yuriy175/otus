using MediatR;
using Posts.Application.Commands.Friends;
using Posts.Infrastructure.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Posts.Application.Commands.Posts
{
    public class UpdatePostCommand : IRequest<int>
    {
        public uint UserId { get; init; }
        public uint PostId { get; init; }
        public string Text { get; init; } = default!;
        public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, int>
        {
            private readonly IPostsRepository _postsRepository;
            public UpdatePostCommandHandler(IPostsRepository postsRepository)
            {
                _postsRepository = postsRepository;
            }
            public Task<int> Handle(UpdatePostCommand command, CancellationToken cancellationToken) =>
                _postsRepository.UpdatePostAsync(command.UserId, command.PostId, command.Text, cancellationToken);
        }
    }
}
