using AiKnowledgeAssistant.Application.AI.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AiKnowledgeAssistant.Tests.AI
{
    [Collection("TestHost collection")]
    public class EmbeddingClientTests
    {
        [Fact]
        public async Task GenerateEmbedding_ReturnsFixedLengthVector()
        {
            //using var host = TestHost.Build();
            var client = TestHost.GetService<IAiEmbeddingClient>();

            var vector = await client.GenerateEmbeddingAsync(
                "SQL timeout during order processing",
                CancellationToken.None);

            Assert.NotNull(vector);
            Assert.True(vector.Length > 0);
        }
    }

}
