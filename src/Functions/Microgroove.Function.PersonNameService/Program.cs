using Azure.Storage.Queues;
using Microgroove.Application;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.RegisterApplication();

        // Queue Client
        string storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        services.AddSingleton(new QueueClient(storageConnectionString, "person-queue"));

        // HttpClientFactory
        services.AddHttpClient();

    })
    .Build();

host.Run();
