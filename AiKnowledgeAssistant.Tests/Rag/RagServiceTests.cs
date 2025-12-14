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
                "Is query timedout?",
                CancellationToken.None);

            Assert.False(string.IsNullOrWhiteSpace(answer));
        }

        [Fact]
        public async Task RAG_Returns_NoInfo_When_Context_Is_Weak()
        {
            using var host = TestHost.Build();
            var _rag = host.Services.GetRequiredService<IRagService>();
            var answer = await _rag.AskAsync(
                "What is the capital of France?",
                CancellationToken.None);

            Assert.Contains("don't have enough information", answer);
        }

        [Fact]
        public async Task RAG_Returns_Grounded_Answer_When_Context_Strong()
        {
            using var host = TestHost.Build();
            var _rag = host.Services.GetRequiredService<IRagService>();
            var answer = await _rag.AskAsync(
                "Why does order processing fail?",
                CancellationToken.None);

            Assert.DoesNotContain("don't have enough information", answer);
        }


    }

}
