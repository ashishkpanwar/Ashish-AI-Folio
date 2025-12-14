namespace AiKnowledgeAssistant.Application.Search;

public interface IVectorSearchStore
{
    Task IndexAsync(
        string id,
        string content,
        float[] vector,
        string source,
        int chunkIndex,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<string>> SearchAsync(
        float[] queryVector,
        int topK,
        CancellationToken cancellationToken);
}
