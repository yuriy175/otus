using DialogGrpc;
using Dialogs.Core.Model.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using static DialogGrpc.Dialog;

namespace Dialogs.Infrastructure.gRpc.Services
{
    public class DialogService : DialogBase
    {
        private readonly IDialogsService _dialogsService;

        public DialogService(IDialogsService dialogsService)
        {
            _dialogsService = dialogsService;
        }

        public override async Task<MessageReply> CreateMessage(CreateMessageRequest request, ServerCallContext context)
        {
            var message = await _dialogsService.CreateMessageAsync(request.AuthorId, request.UserId, request.Text, context.CancellationToken);
            return new MessageReply
            {
                AuthorId = message.AuthorId,
                Text = message.Text,
                UserId = Convert.ToUInt32(message.UserId)
            };
        }

        public override async Task<GetMessagesReply> GetMessages(GetMessagesRequest request, ServerCallContext context)
        {
            var messages = await _dialogsService.GetMessagesAsync(request.AuthorId, request.UserId, context.CancellationToken );
            var reply = new GetMessagesReply { };

            reply.Messages.AddRange(messages.Select(m => new MessageReply{
                AuthorId = m.AuthorId,
                Text = m.Text,
                UserId = Convert.ToUInt32(m.UserId)
            }));
            return reply;
        }
    }
}
