using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ORMComparison;

var builder = Host.CreateDefaultBuilder(args)
    .Configure()
    .Build();

await builder.Services.GetRequiredService<IExecutor>().RunAsync(10);