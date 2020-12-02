using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using Trill.Api;
using Trill.Shared.Tests.Integration;
using Xunit;

namespace Trill.Modules.Stories.Tests.Integration.Controllers
{
    public class HomeControllerTests : WebApiTestBase
    {
        [Fact]
        public async Task get_stories_module_should_return_hello_world_message()
        {
            var response = await GetAsync("api/stories-module/hello");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            content.ShouldBe("Trill");
        }

        public HomeControllerTests(WebApplicationFactory<Program> factory, MongoFixture mongo) : base(factory, mongo)
        {
        }
    }
}