using System;
using System.Threading.Tasks;
using Chronicle;
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
        ISagaAction<AdPublished>
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
        }

        public async Task CompensateAsync(AdApproved message, ISagaContext context)
        {
        }

        public async Task HandleAsync(AdPaid message, ISagaContext context)
        {
        }

        public async Task CompensateAsync(AdPaid message, ISagaContext context)
        {
        }

        public async Task HandleAsync(AdPublished message, ISagaContext context)
        {
        }

        public async Task CompensateAsync(AdPublished message, ISagaContext context)
        {
        }
    }
}