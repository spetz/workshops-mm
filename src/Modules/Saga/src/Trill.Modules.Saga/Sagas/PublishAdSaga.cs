using System;
using System.Threading.Tasks;
using Chronicle;
using Trill.Modules.Saga.Commands.External;
using Trill.Modules.Saga.Events.External;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Saga.Sagas
{
    internal class PublishAdSagaData
    {
        public Guid AdId { get; set; }
    }

    internal class PublishAdSaga : Saga<PublishAdSagaData>,
        ISagaStartAction<AdApproved>,
        ISagaAction<AdPaid>,
        ISagaAction<AdPublished>,
        ISagaAction<AdActionRejected>
    {
        private readonly IMessageBroker _messageBroker;

        public override SagaId ResolveId(object message, ISagaContext context)
            => message switch
            {
                AdApproved m => m.AdId.ToString(),
                AdPaid m => m.AdId.ToString(),
                AdPublished m => m.AdId.ToString(),
                _ => base.ResolveId(message, context)
            };

        public PublishAdSaga(IMessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(AdApproved message, ISagaContext context)
        {
            await _messageBroker.PublishAsync(new PayAd(message.AdId));
        }

        public async Task CompensateAsync(AdApproved message, ISagaContext context)
        {
            await Task.CompletedTask;
        }

        public async Task HandleAsync(AdPaid message, ISagaContext context)
        {
            await _messageBroker.PublishAsync(new PublishAd(message.AdId));
        }

        public async Task CompensateAsync(AdPaid message, ISagaContext context)
        {
            await Task.CompletedTask;
        }

        public async Task HandleAsync(AdPublished message, ISagaContext context)
        {
            await CompleteAsync();
        }

        public async Task CompensateAsync(AdPublished message, ISagaContext context)
        {
            await Task.CompletedTask;
        }

        public async Task HandleAsync(AdActionRejected message, ISagaContext context)
        {
            await RejectAsync();
        }

        public async Task CompensateAsync(AdActionRejected message, ISagaContext context)
        {
            await Task.CompletedTask;
        }
    }
}