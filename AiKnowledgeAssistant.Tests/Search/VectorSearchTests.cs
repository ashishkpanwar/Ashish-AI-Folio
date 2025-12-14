using AiKnowledgeAssistant.Application.AI;
using AiKnowledgeAssistant.Application.Search;
using AiKnowledgeAssistant.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Numerics;
using Xunit;
using static System.Net.Mime.MediaTypeNames;

namespace AiKnowledgeAssistant.Tests.Search
{

    public class VectorSearchTests
    {
        [Fact]
        public async Task Search_ReturnsRelevantChunks()
        {
            using var host = TestHost.Build();

            var embeddingClient = host.Services.GetRequiredService<IAiEmbeddingClient>();
            var searchStore = host.Services.GetRequiredService<IVectorSearchStore>();

            var text = "Order processing failed due to SQL timeout";


            var vector = await embeddingClient.GenerateEmbeddingAsync(
                text,
                CancellationToken.None);

            await searchStore.IndexAsync(
                id: Guid.NewGuid().ToString(),
                content: text,
                vector: vector,
                source: "test",
                chunkIndex: 0,
                CancellationToken.None);

            await Task.Delay(1500);

            var queryVector = await embeddingClient.GenerateEmbeddingAsync(
            "Database timeout during order processing",
            CancellationToken.None);

            var results = await searchStore.SearchAsync(
                queryVector,
                topK: 3,
                CancellationToken.None);

            Assert.NotEmpty(results);
        }
    }

}
