using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ORMComparison;

var host = Host.CreateDefaultBuilder(args)
    .Configure()
    .Build();

await host.Services.GetRequiredService<IExecutor>().RunAsync(new Execution
{
    NumberOfRuns= 2,
    NumberOfUsers= 1000,
    NumberOfPostsPerUser= 2,
    NumberOfUsersToGetById= 100,
    NumberOfLikesPerUser= 10,
    NumberOfMostLikedPosts= 100,
});