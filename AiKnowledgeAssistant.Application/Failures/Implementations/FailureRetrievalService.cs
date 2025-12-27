using AiKnowledgeAssistant.Application.AI.Interfaces;
using AiKnowledgeAssistant.Application.Failures.Interfaces;
using AiKnowledgeAssistant.Application.Failures.Models;
using AiKnowledgeAssistant.Application.Failures.Queries;

namespace AiKnowledgeAssistant.Application.Failures;

public sealed class FailureRetrievalService : IFailureRetrievalService
{
    private readonly IAiEmbeddingClient _embeddingClient;
    private readonly IFailureVectorStore _vectorStore;

    public FailureRetrievalService(
        IAiEmbeddingClient embeddingClient,
        IFailureVectorStore vectorStore)
    {
        _embeddingClient = embeddingClient;
        _vectorStore = vectorStore;
    }

    public async Task<IReadOnlyList<FailureRecord>> FindSimilarAsync(
        FindSimilarFailuresQuery query,
        CancellationToken cancellationToken)
    {
        var embedding = await _embeddingClient.GenerateEmbeddingAsync(
            query.Question,
            cancellationToken);

        var filter = BuildFilter(query);

        return await _vectorStore.SearchSimilarFailuresAsync(
            embedding,
            filter,
            query.Top,
            cancellationToken);
    }

    private static string BuildFilter(FindSimilarFailuresQuery query)
    {

        //Supported Operators in Azure Search Filters
        //eq Equal to => price eq 100
        //ne Not equal to    => category ne 'books'
        //gt Greater than => rating gt 4
        //ge Greater than or equal => date ge 2025 - 01 - 01T00: 00:00Z
        //lt  Less than  => price lt 50
        //le Less than or equal => stock le 10
        var filters = new List<string>
        {
            $"environment eq '{query.Environment}'",
            $"jobId eq '{query.JobId}'",
            $"severity ge {query.MinSeverity}"
        };

        if (query.OnlyActive)
        {
            filters.Add("isActive eq true");
        }

        return string.Join(" and ", filters);

    }
}