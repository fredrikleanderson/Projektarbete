﻿using Microsoft.Extensions.DependencyInjection;
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
    NumberOfPostsPerUser= 1,
    NumberOfUsersToGetById= 1000,
    NumberOfLikesPerUser= 5,
    NumberOfMostLikedPosts= 100,
});