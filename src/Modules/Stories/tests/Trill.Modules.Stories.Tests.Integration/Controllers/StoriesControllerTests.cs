using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using Trill.Api;
using Trill.Modules.Stories.Application.Commands;
using Trill.Modules.Stories.Infrastructure.Mongo.Documents;
using Trill.Shared.Tests.Integration;
using Xunit;

namespace Trill.Modules.Stories.Tests.Integration.Controllers
{
    public class StoriesControllerTests : WebApiTestBase
    {
        [Fact]
        public async Task post_send_story_should_create_story_and_return_location_header()
        {
            var storyId = 1;
            var userId = Guid.NewGuid();
            await AddUserToDatabaseAsync(userId);
            var id = Guid.NewGuid();
            var command = new SendStory(storyId, userId, $"Test story {id:N}", $"Lorem ipsum {id}",
                new[] {$"test-1-{id:N}", $"test-2-{id:N}"});
            var response = await PostAsync("stories", command);
            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            var location = response.Headers.Location;
            location.ShouldNotBeNull();
        }

        private async Task AddUserToDatabaseAsync(Guid id)
        {
            var document = new UserDocument
            {
                Id = id,
                Name = $"user-{id}",
                CreatedAt = DateTime.UtcNow
            };
            await Mongo.Database.GetCollection<UserDocument>("stories-module.users")
                .InsertOneAsync(document);
        }
        
        public StoriesControllerTests(WebApplicationFactory<Program> factory, MongoFixture mongo) : base(factory, mongo)
        {
            SetPath("stories-module");
        }
    }
}