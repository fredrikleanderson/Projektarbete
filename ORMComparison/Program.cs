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
    NumberOfUsers= 100_000,
    NumberOfPostingUsers = 10_000,
    NumberOfPostsPerUser= 10,
    NumberOfUserPages= 5000,
    NumberOfLikingUsers = 10_000,
    NumberOfLikesPerUser= 10,
    NumberOfMostLikedPosts= 10_000,
    NumberOfUsersToUpdate= 5000,
    NumberOfUsersDeletingTheirPosts = 2000
});