using AiKnowledgeAssistant.Ingestion.Application.Models;
using Azure.Search.Documents;

namespace AiKnowledgeAssistant.Ingestion.Infrastructure.Search;

public sealed class FailureIngestionService
{
    private readonly SearchClient _searchClient;
    private readonly Func<string, CancellationToken, Task<float[]>> _embeddingGenerator;

    public FailureIngestionService(
        SearchClient searchClient,
        Func<string, CancellationToken, Task<float[]>> embeddingGenerator)
    {
        _searchClient = searchClient;
        _embeddingGenerator = embeddingGenerator;
    }

    public async Task IngestAsync(
        FailureRecord failure,
        CancellationToken cancellationToken)
    {
        var embedding = await _embeddingGenerator(
            failure.Content,
            cancellationToken);

        var document = new
        {
            id = failure.Id,
            content = failure.Content,
            contentVector = embedding,

            timestamp = failure.Timestamp,
            serviceName = failure.ServiceName,
            workflowName = failure.WorkflowName,
            environment = failure.Environment,
            errorType = failure.ErrorType,
            severity = failure.Severity,
            isActive = failure.IsActive,
            source = failure.Source
        };

        await _searchClient.UploadDocumentsAsync(
            new[] { document },
            cancellationToken: cancellationToken);
    }
}
