using AiKnowledgeAssistant.Application.AI.Interfaces;
using Microsoft.Extensions.DependencyInjection;


namespace AiKnowledgeAssistant.Tests.AI
{
    [Collection("TestHost collection")]
    public class AiClientTests
    {
        [Fact]
        public async Task Chat_ReturnsResponse()
        {
            //using var host = TestHost.Build();
            var client = TestHost.GetService<IAiClient>();

            var response = await client.GetChatResponseAsync(
                "You are a helpful assistant.",
                "What is cosine similarity?",
                CancellationToken.None);

            Assert.False(string.IsNullOrWhiteSpace(response));
        }
    }

}
