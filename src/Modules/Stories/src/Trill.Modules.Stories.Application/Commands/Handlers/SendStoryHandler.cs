using System.Threading.Tasks;
using Trill.Modules.Stories.Application.Services;
using Trill.Modules.Stories.Core.Entities;
using Trill.Modules.Stories.Core.Policies;
using Trill.Modules.Stories.Core.Repositories;
using Trill.Modules.Stories.Core.ValueObjects;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Generators;

namespace Trill.Modules.Stories.Application.Commands.Handlers
{
    internal sealed class SendStoryHandler : ICommandHandler<SendStory>
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IStoryTextPolicy _storyTextPolicy;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IIdGenerator _idGenerator;
        private readonly IStoryRequestStorage _storyRequestStorage;

        public SendStoryHandler(IStoryRepository storyRepository,
            IStoryTextPolicy storyTextPolicy, IDateTimeProvider dateTimeProvider, IIdGenerator idGenerator,
            IStoryRequestStorage storyRequestStorage)
        {
            _storyRepository = storyRepository;
            _storyTextPolicy = storyTextPolicy;
            _dateTimeProvider = dateTimeProvider;
            _idGenerator = idGenerator;
            _storyRequestStorage = storyRequestStorage;
        }

        public async Task HandleAsync(SendStory command)
        {
            var author = Author.Create(command.UserId, $"user-{command.UserId:N}");
            var storyText = new StoryText(command.Text);
            _storyTextPolicy.Verify(storyText);
            var now = _dateTimeProvider.Get();
            var visibility = command.VisibleFrom.HasValue && command.VisibleTo.HasValue
                ? new Visibility(command.VisibleFrom.Value, command.VisibleTo.Value, command.Highlighted)
                : Visibility.Default(now);
            var storyId = command.StoryId == default ? _idGenerator.Generate() : command.StoryId;
            var story = new Story(storyId, author, command.Title, storyText, command.Tags, now, visibility);
            await _storyRepository.AddAsync(story);
            _storyRequestStorage.SetStoryId(command.Id, story.Id);
        }
    }
}