using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Trill.Modules.Stories.Application.DTO;
using Trill.Modules.Stories.Application.Queries;
using Trill.Modules.Stories.Infrastructure;
using Trill.Shared.Abstractions.Queries;
using Trill.Shared.Bootstrapper;
using Trill.Shared.Bootstrapper.Endpoints;
using Trill.Shared.Infrastructure.Modules;

[assembly: InternalsVisibleTo("Trill.Api")]
namespace Trill.Modules.Stories.Api
{
    internal class StoriesModule : IModule
    {
        public string Name { get; } = "Stories";
        public string Path { get; } = "stories-module";
        public string Schema { get; } = "stories-module";


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure();
        }

        public void ConfigureMiddleware(IApplicationBuilder app)
        {
            app.UseContracts();
            app.UseInfrastructure();
        }

        public void ConfigureEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(Path, ctx => ctx.Response.WriteAsync($"{Name} module"));
            endpoints
                .Get<BrowseStories, Paged<StoryDto>>($"{Path}/stories")
                .Get<GetStory, StoryDetailsDto>($"{Path}/stories/{{storyId}}");
        }
    }
}