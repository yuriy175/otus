using Common.MQ.Core.Model.Interfaces;
using MediatR;
using Posts.Application.Commands.Friends;
using Posts.Infrastructure.Caches;
using Posts.Infrastructure.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Posts.Application.Commands.Posts
{
    public class AddPostCommand : IRequest<int>
    {
        public uint UserId { get; init; }
        public string Text { get; init; } = default!;
        public class AddPostCommandHandler : IRequestHandler<AddPostCommand, int>
        {
            private readonly IPostsRepository _postsRepository;
            private readonly ICacheService _cacheService;
            private readonly IFriendsRepository _friendsRepository;
            private readonly IMQSender _mqSender;
            public AddPostCommandHandler(
                IPostsRepository postsRepository,
                IFriendsRepository friendsRepository,
                ICacheService cacheService,
                IMQSender mqSender)
            {
                _postsRepository = postsRepository;
                _friendsRepository = friendsRepository;
                _cacheService = cacheService;
                _mqSender = mqSender;
            }
            public async Task<int> Handle(AddPostCommand command, CancellationToken cancellationToken)
            {
                var userId = command.UserId;
                var text = command.Text;

                var post = await _postsRepository.AddPostAsync(userId, text, cancellationToken);
                _mqSender.SendPost(userId, text);
                var subscriberIds = await _friendsRepository.GetSubscriberIdsAsync(userId, cancellationToken);
                foreach (var subscriberId in subscriberIds)
                {
                    await _cacheService.AddPostAsync(Convert.ToUInt32(subscriberId), post);
                }

                return 1;
            }
        }
    }
}
