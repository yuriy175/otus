﻿using Common.MQ.Core.Model;
using Common.MQ.Core.Model.Interfaces;
using Common.MQ.Core.Model.Types;
using Common.MQ.Core.Services;
using Counters.Core.Model.Interfaces;
using Counters.Infrastructure.Repositories.Interfaces;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace Counters.Core.Services
{
    public class CounterService : ICounterService
    {
        private readonly IMQReceiver _mqReceiver;
        private readonly IMQSender _mqSender;
        private readonly ICountersRepository _counterRepository;

        public CounterService(
            IMQReceiver mqReceiver,
            IMQSender mqSender,
            ICountersRepository counterRepository)
        {
            _counterRepository = counterRepository;
            _mqReceiver = mqReceiver;
            _mqSender = mqSender;

            _mqReceiver.CreateUnreadDialogMessagesCountReceiver(async (data) =>
            {
                var text = Encoding.UTF8.GetString(data);
                var message = JsonSerializer.Deserialize<UnreadCountMessage>(text);
                if(message == null)
                {
                    return;
                }

                if (message.MessageType == MQMessageTypes.UpdateUnreadDialogMessages)
                {
                    try
                    {
                        //throw new Exception();
                        var count = message.UnreadMessageIds.Count();
                        _ = await _counterRepository.UpdateUnReadCounterByUserIdAsync(
                            message.UserId,
                            message.IsIncrement ? count : -count,
                            CancellationToken.None);
                    }
                    catch(Exception ex)
                    {
                        _mqSender.SendUnreadDialogMessageIdsFailed(
                            message.UserId,
                            message.IsIncrement,
                            message.UnreadMessageIds);
                    }
                }
            });
        }

        public Task<int> UpdateUnReadCounterByUserIdAsync(uint userId, int delta, CancellationToken cancellationToken) =>
            _counterRepository.UpdateUnReadCounterByUserIdAsync(userId, delta, cancellationToken);

        public Task<uint> GetUnReadCounterByUserIdAsync(uint userId, CancellationToken cancellationToken) =>
            _counterRepository.GetUnReadCounterByUserIdAsync(userId, cancellationToken);
    }
}
