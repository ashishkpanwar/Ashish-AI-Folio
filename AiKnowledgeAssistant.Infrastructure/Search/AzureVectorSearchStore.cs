using AiKnowledgeAssistant.Application.Search.Interface;
using AiKnowledgeAssistant.Application.Search.Models;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace AiKnowledgeAssistant.Infrastructure.Search
{
    public sealed class AzureVectorSearchStore : IVectorSearchStore
    {
        private readonly SearchClient _searchClient;

        public AzureVectorSearchStore(SearchClient searchClient)
        {
            _searchClient = searchClient;
        }

        public async Task IndexAsync(
            string id,
            string content,
            float[] vector,
            string source,
            int chunkIndex,
            CancellationToken cancellationToken)
        {
            var document = new
            {
                id,
                content,
                contentVector = vector,
                source,
                chunkIndex
            };

            await _searchClient.UploadDocumentsAsync(
                new[] { document }, cancellationToken :cancellationToken);
        }

        public async Task<IReadOnlyList<VectorSearchResult>> SearchAsync(
        float[] queryVector,
        int topK,
        CancellationToken cancellationToken)
        {
            var options = new SearchOptions
            {
                Size = topK,
                VectorSearch = new()
                {
                    Queries =
                    {
                        new VectorizedQuery(queryVector)
                        {
                            Fields = { "contentVector" }
                        }
                    }
                }
            };

            var results = await _searchClient.SearchAsync<SearchDocument>(
                null,
                options,
                cancellationToken);

            var matches = new List<VectorSearchResult>();

            await foreach (var result in results.Value.GetResultsAsync())
            {
                matches.Add(new VectorSearchResult (
                    result.Document["content"]!.ToString()!,
                    result.Score ?? 0.0
                ));
            }

            return matches;
        }

    }
}
