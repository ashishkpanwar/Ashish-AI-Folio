using AiKnowledgeAssistant.Application.Search.Models;

namespace AiKnowledgeAssistant.Application.Search.Interface;

public interface IVectorSearchStore
{
    Task IndexAsync(
        string id,
        string content,
        float[] vector,
        string source,
        int chunkIndex,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<VectorSearchResult>> SearchAsync(
        float[] queryVector,
        int topK,
        CancellationToken cancellationToken);
}
