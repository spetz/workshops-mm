using System;
using System.Threading.Tasks;
using NSubstitute;
using Trill.Modules.Stories.Application.Commands;
using Trill.Modules.Stories.Application.Commands.Handlers;
using Trill.Modules.Stories.Application.Services;
using Trill.Modules.Stories.Core.Entities;
using Trill.Modules.Stories.Core.Policies;
using Trill.Modules.Stories.Core.Repositories;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Generators;
using Trill.Shared.Abstractions.Messaging;
using Xunit;

namespace Trill.Modules.Stories.Tests.Unit.Handlers
{
    public class SendStoryHandlerTests
    {
        private Task Act(SendStory command) => _handler.HandleAsync(command);
        
        [Fact]
        public async Task story_should_be_added_given_valid_data()
        {
            // Arrange
            var user = SetupUser();
            var command = CreateCommand(user.Id);
            
            // Act
            await Act(command);
            
            // Assert
            await _userRepository.Received(1).GetAsync(user.Id);
            _storyTextPolicy.Received(1).Verify(command.Text);
            _dateTimeProvider.Received(1).Get();
            _storyRequestStorage.Received(1).SetStoryId(command.Id, Arg.Any<long>());
            await _storyRepository.Received(1).AddAsync(Arg.Is<Story>(x => x.Title == command.Title));
        }

        #region Arrange
        
        private readonly IStoryRepository _storyRepository;
        private readonly IStoryTextPolicy _storyTextPolicy;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IIdGenerator _idGenerator;
        private readonly IStoryRequestStorage _storyRequestStorage;
        private readonly IUserRepository _userRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly ICommandHandler<SendStory> _handler;

        public SendStoryHandlerTests()
        {
            _storyRepository = Substitute.For<IStoryRepository>();
            _storyTextPolicy = Substitute.For<IStoryTextPolicy>();
            _dateTimeProvider = Substitute.For<IDateTimeProvider>();
            _idGenerator = Substitute.For<IIdGenerator>();
            _storyRequestStorage = Substitute.For<IStoryRequestStorage>();
            _userRepository = Substitute.For<IUserRepository>();
            _messageBroker = Substitute.For<IMessageBroker>();
            _handler = new SendStoryHandler(_storyRepository, _storyTextPolicy, _dateTimeProvider, _idGenerator,
                _storyRequestStorage, _userRepository, _messageBroker);
        }

        private User SetupUser(bool locked = false)
        {
            var user = new User(Guid.NewGuid(), "test", DateTime.UtcNow, locked);
            _userRepository.GetAsync(user.Id).Returns(user);
            return user;
        }

        private static SendStory CreateCommand(Guid userId)
            => new SendStory(default, userId, "Test", "Lorem ipsum", new[] {"test1", "{test2"});

        #endregion
    }
}