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
    NumberOfPostingUsers = 1000,
    NumberOfPostsPerUser= 10,
    NumberOfUserPages= 1000,
    NumberOfLikingUsers = 1000,
    NumberOfLikesPerUser= 10,
    NumberOfMostLikedPosts= 100,
    NumberOfUsersToUpdate= 1000,
    NumberOfUsersDeletingTheirPosts = 1000
});