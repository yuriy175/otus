using Common.MQ.Core.Model;
using Common.MQ.Core.Model.Interfaces;
using Common.MQ.Core.Model.Types;
using Dialogs.Core.Model;
using Dialogs.Core.Model.Interfaces;
using Dialogs.Infrastructure.Repositories;
using Dialogs.Infrastructure.Repositories.Interfaces;
using System.Text;
using System.Text.Json;

namespace Dialogs.Core.Services
{
    public class DialogsService : IDialogsService
    {        
        private readonly IDialogsRepository _dialogsRepository;
        private readonly IMQSender _mqSender;
        private readonly IMQReceiver _mqReceiver;

        public DialogsService(
            IDialogsRepository dialogsRepository,
            IMQSender mqSender,
            IMQReceiver mqReceiver)
        {
            _dialogsRepository = dialogsRepository;
            _mqSender = mqSender;
            _mqReceiver = mqReceiver;

            _mqReceiver.CreateUnreadDialogMessagesCountFailedReceiver(async (data) =>
            {
                var text = Encoding.UTF8.GetString(data);
                var message = JsonSerializer.Deserialize<UnreadCountMessage>(text);
                if (message == null)
                {
                    return;
                }

                if (message.MessageType == MQMessageTypes.UpdateUnreadDialogMessagesCompensate)
                {
                    await _dialogsRepository.SetUnreadDialogMessagesAsync(
                        message.UnreadMessageIds, 
                        CancellationToken.None);
                }
            });
        }

        public async Task<Message> CreateMessageAsync(uint authorId, uint userId, string text, CancellationToken cancellationToken)
        {
            var message = await _dialogsRepository.AddMessageAsync(authorId, userId, text, cancellationToken);
            _mqSender.SendDialogMessage(message);
            return message;
        }

        public Task<IEnumerable<Message>> GetMessagesAsync(uint userId1, uint userId2, CancellationToken cancellationToken) =>
            _dialogsRepository.GetMessagesAsync(userId1, userId2,cancellationToken);

        public Task<IEnumerable<int>> GetBuddyIdsAsync(uint userId, CancellationToken cancellationToken) =>
            _dialogsRepository.GetBuddyIdsAsync(userId, cancellationToken);

        public async Task<int> SetUnreadMessagesFromUserAsync(uint authorId, uint userId, CancellationToken cancellationToken)
        {
            var unreadMsgIds = (await _dialogsRepository.SetReadDialogMessagesFromUserAsync(authorId, userId, cancellationToken))?.ToArray();
            var count = unreadMsgIds?.Length ?? 0;
            if (unreadMsgIds != null)
            {
                // increase counter
                _mqSender.SendUnreadDialogMessageIds(userId, false, unreadMsgIds);
            }

            return count;
        }
    }
}
