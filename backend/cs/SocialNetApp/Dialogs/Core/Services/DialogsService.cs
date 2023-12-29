using Common.MQ.Core.Model.Interfaces;
using Dialogs.Core.Model;
using Dialogs.Core.Model.Interfaces;
using Dialogs.Infrastructure.Repositories;
using Dialogs.Infrastructure.Repositories.Interfaces;

namespace Dialogs.Core.Services
{
    public class DialogsService : IDialogsService
    {        
        private readonly IDialogsRepository _dialogsRepository;
        private readonly IMQSender _mqSender;

        public DialogsService(
            IDialogsRepository dialogsRepository,
            IMQSender mqSender)
        {
            _dialogsRepository = dialogsRepository;
            _mqSender = mqSender;
        }

        public Task<Message> CreateMessageAsync(uint authorId, uint userId, string text, CancellationToken cancellationToken) =>
            _dialogsRepository.AddMessageAsync(authorId, userId, text, cancellationToken);

        public Task<IEnumerable<Message>> GetMessagesAsync(uint userId1, uint userId2, CancellationToken cancellationToken) =>
            _dialogsRepository.GetMessagesAsync(userId1, userId2,cancellationToken);
    }
}
