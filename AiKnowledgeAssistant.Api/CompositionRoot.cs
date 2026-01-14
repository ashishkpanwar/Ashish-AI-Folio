using AiKnowledgeAssistant.Application.AI.Implementations;
using AiKnowledgeAssistant.Application.AI.Interfaces;
using AiKnowledgeAssistant.Application.Failures;
using AiKnowledgeAssistant.Application.Failures.Implementations;
using AiKnowledgeAssistant.Application.Failures.Interfaces;
using AiKnowledgeAssistant.Application.Failures.Models;
using AiKnowledgeAssistant.Infrastructure.AI;
using AiKnowledgeAssistant.Infrastructure.Persistence;
using AiKnowledgeAssistant.Infrastructure.Search;
using Azure;
using Azure.AI.OpenAI;
using Azure.Search.Documents;
using Microsoft.EntityFrameworkCore;


namespace AiKnowledgeAssistant.Api
{
    public static class CompositionRoot
    {
        public static void ConfigureServices(
            IServiceCollection services,
            IConfiguration configuration)
        {


            services.AddScoped(sp =>
            {
                var endpoint = new Uri(configuration["AzureOpenAI:Endpoint"]!);
                var key = configuration["AzureOpenAI:ApiKey"]!;
                return new AzureOpenAIClient(endpoint, new AzureKeyCredential(key));
            });

            services.AddScoped(sp =>
            {
                var azureClient = sp.GetRequiredService<AzureOpenAIClient>();
                var deployment = configuration["AzureOpenAI:EmbeddingDeployment"]!;
                return azureClient.GetEmbeddingClient(deployment);
            });

            services.AddScoped(sp =>
            {
                var endpoint = new Uri(configuration["AzureSearchService:Endpoint"]!);
                var index = configuration["AzureSearchService:IndexName"]!;
                var apiKey = configuration["AzureSearchService:ApiKey"]!;

                return new SearchClient(endpoint, index, new AzureKeyCredential(apiKey));
            });

            services.AddScoped(sp =>
            {
                var azureClient = sp.GetRequiredService<AzureOpenAIClient>();
                var chatDeployment = configuration["AzureOpenAI:ChatDeployment"]!;
                return azureClient.GetChatClient(chatDeployment);
            });

            services.AddInfrastructure(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sql =>
                    {
                        sql.MigrationsAssembly("AiKnowledgeAssistant.Api");
                        sql.EnableRetryOnFailure();
                    });
            });

            services.AddScoped<IFailureWindowResolver, FailureWindowResolver>();
            services.AddScoped<IAiClient, AzureOpenAiClient>();
            services.AddScoped<IAiEmbeddingClient, AzureOpenAiEmbeddingClient>();
            services.AddScoped<IFailureVectorStore, FailureVectorSearchStore>();
            services.AddScoped<IFailureRetrievalService, FailureRetrievalService>();
            services.AddScoped<ITokenGuardrail, DefaultTokenGuardrail>();
            services.AddScoped<IFailureRecordReader, FailureRecord>
            services.AddScoped<FailureOverviewService>();
        }
    }

}
