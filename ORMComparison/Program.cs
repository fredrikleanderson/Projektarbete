using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ORMComparison;

var host = Host.CreateDefaultBuilder(args)
    .Configure()
    .Build();

await host.Services.GetRequiredService<IExecutor>().RunAsync(new Execution
{
    NumberOfRuns= 1,
    NumberOfUsers= 1000,
    NumberOfPostingUsers = 100,
    NumberOfPostsPerUser= 10,
    NumberOfUsersToGetById= 100,
    NumberOfLikingUsers = 1000,
    NumberOfLikesPerUser= 10,
    NumberOfMostLikedPosts= 100,
    NumberOfUsersToUpdate= 1000,
});