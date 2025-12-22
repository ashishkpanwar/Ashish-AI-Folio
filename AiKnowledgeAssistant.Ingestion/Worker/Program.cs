using AiKnowledgeAssistant.Ingestion.Application.Abstractions;
using AiKnowledgeAssistant.Ingestion.Application.Normalization;
using AiKnowledgeAssistant.Ingestion.Application.Pipeline;
using AiKnowledgeAssistant.Ingestion.Infrastructure.LogSources;
using AiKnowledgeAssistant.Ingestion.Infrastructure.Search;
using Azure;
using Azure.Search.Documents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false);
    })
    .ConfigureServices(services =>
    {
        // 🔹 Log source (mock for now)
        services.AddSingleton<ILogSource>(_ =>
            new JsonFileLogSource("mock-logs"));

        // 🔹 Normalizer
        services.AddSingleton<ILogNormalizer, DefaultLogNormalizer>();

        // 🔹 Azure AI Search client
        services.AddSingleton(_ =>
        {
            var endpoint = new Uri("<SEARCH-ENDPOINT>");
            var apiKey = new AzureKeyCredential("<SEARCH-API-KEY>");
            return new SearchClient(endpoint, "<INDEX-NAME>", apiKey);
        });

        // 🔹 Embedding generator (reuse existing logic / client)
        services.AddSingleton<Func<string, CancellationToken, Task<float[]>>>(
            _ => async (text, ct) =>
            {
                // Plug your existing AzureOpenAiEmbeddingClient here
                throw new NotImplementedException("Wire embedding client");
            });

        services.AddSingleton<FailureIngestionService>();
        services.AddSingleton<FailureIngestionPipeline>();
    })
    .Build();

    await host.RunAsync();

    var pipeline = host.Services.GetRequiredService<FailureIngestionPipeline>();

    await pipeline.RunAsync(
        from: DateTimeOffset.UtcNow.AddDays(-2),
        to: DateTimeOffset.UtcNow,
        cancellationToken: CancellationToken.None);
