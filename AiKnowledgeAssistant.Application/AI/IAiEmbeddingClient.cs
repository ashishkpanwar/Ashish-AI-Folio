namespace AiKnowledgeAssistant.Application.AI;

public interface IAiEmbeddingClient
{
    Task<float[]> GenerateEmbeddingAsync(
        string text,
        CancellationToken cancellationToken);
}
