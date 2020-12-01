using System.Threading.Tasks;
using Trill.Modules.Stories.Application.Clients.Users;
using Trill.Modules.Stories.Application.Exceptions;
using Trill.Modules.Stories.Core.Entities;
using Trill.Modules.Stories.Core.Repositories;
using Trill.Shared.Abstractions.Commands;

namespace Trill.Modules.Stories.Application.Commands.Handlers
{
    internal sealed class RateStoryHandler : ICommandHandler<RateStory>
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IStoryRatingRepository _storyRatingRepository;
        private readonly IUsersApiClient _usersApiClient;

        public RateStoryHandler(IStoryRepository storyRepository, IStoryRatingRepository storyRatingRepository,
            IUsersApiClient usersApiClient)
        {
            _storyRepository = storyRepository;
            _storyRatingRepository = storyRatingRepository;
            _usersApiClient = usersApiClient;
        }

        public async Task HandleAsync(RateStory command)
        {
            var user = await _usersApiClient.GetAsync(command.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(command.UserId);
            }

            if (user.Locked)
            {
                throw new UserLockedException(command.UserId);
            }
            
            var story = await _storyRepository.GetAsync(command.StoryId);
            if (story is null)
            {
                throw new StoryNotFoundException(command.StoryId);
            }
    
            await _storyRatingRepository.SetAsync(new StoryRating(new StoryRatingId(command.StoryId, command.UserId),
                command.Rate));
        }
    }
}