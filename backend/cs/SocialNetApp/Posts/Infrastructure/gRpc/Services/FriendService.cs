using AutoMapper;
using Friend;
using Grpc.Core;
using MediatR;
using Posts.Application.Queries.Friends;
using static Friend.Friend;

namespace Posts.Infrastructure.gRpc.Services
{
    public class FriendService : FriendBase
    {
        private readonly IMediator _mediator;

        public FriendService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<GetFriendIdsReply> GetFriendIds(GetFriendIdsRequest request, ServerCallContext context)
        {
            var friends = await _mediator.Send(new GetUserFriendsQuery { UserId = request.Id });
            var reply = new GetFriendIdsReply { };
            if(friends is null)
            {
                return reply;
            }
            reply.Ids.AddRange(friends.Select(t => Convert.ToUInt32(t)));
            return reply;
        }
    }
}
