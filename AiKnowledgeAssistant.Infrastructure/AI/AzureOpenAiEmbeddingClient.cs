using AiKnowledgeAssistant.Application.AI.Interfaces;
using OpenAI.Embeddings;

namespace AiKnowledgeAssistant.Infrastructure.AI;

public sealed class AzureOpenAiEmbeddingClient : IAiEmbeddingClient
{
    private readonly EmbeddingClient _embeddingClient;

    public AzureOpenAiEmbeddingClient(EmbeddingClient embeddingClient)
    {
        _embeddingClient = embeddingClient;
    }

    public async Task<float[]> GenerateEmbeddingAsync(
        string text,
        CancellationToken cancellationToken)
    {
        var response = await _embeddingClient.GenerateEmbeddingAsync(
            text,
            null,
            cancellationToken);

        return response.Value.ToFloats().ToArray();
    }
}
