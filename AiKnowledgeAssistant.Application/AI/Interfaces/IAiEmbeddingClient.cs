namespace AiKnowledgeAssistant.Application.AI.Interfaces;

public interface IAiEmbeddingClient
{
    Task<float[]> GenerateEmbeddingAsync(
        string text,
        CancellationToken cancellationToken);
}
