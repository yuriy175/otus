using Common.MQ.Core.Model;
using Common.MQ.Core.Model.Interfaces;
using Common.MQ.Core.Model.Types;
using Dialogs.Core.Model;
using Dialogs.Core.Model.Interfaces;
using Dialogs.Infrastructure.Caches;
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
        private readonly ICacheService _cacheService;

        public DialogsService(
            IDialogsRepository dialogsRepository,
            IMQSender mqSender,
            IMQReceiver mqReceiver,
            ICacheService cacheService)
        {
            _dialogsRepository = dialogsRepository;
            _mqSender = mqSender;
            _mqReceiver = mqReceiver;
            _cacheService = cacheService;

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
            var wsAddress = await _cacheService.GetUserWebSocketAddressAsync(userId);
            if (string.IsNullOrEmpty(wsAddress))
            {
                // increment unread counter
                _mqSender.SendUnreadDialogMessageIds(userId, true, new[] { Convert.ToInt32(message.Id) });
            }
            else
            {
                _mqSender.SendNewDialogMessage(wsAddress, message);
            }

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
