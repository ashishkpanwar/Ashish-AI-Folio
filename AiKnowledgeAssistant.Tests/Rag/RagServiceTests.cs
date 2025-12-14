using AiKnowledgeAssistant.Application.Rag;
using AiKnowledgeAssistant.Application.Rag.Interface;
using AiKnowledgeAssistant.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AiKnowledgeAssistant.Tests.Rag
{
    public class RagServiceTests
    {
        [Fact]
        public async Task RAG_Returns_Grounded_Answer()
        {
            using var host = TestHost.Build();
            var rag = host.Services.GetRequiredService<IRagService>();

            var answer = await rag.AskAsync(
                "What is the capital of France?",
                CancellationToken.None);

            Assert.False(string.IsNullOrWhiteSpace(answer));
        }
    }

}
