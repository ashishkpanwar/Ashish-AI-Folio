using AiKnowledgeAssistant.Application.AI;
using AiKnowledgeAssistant.Application.Rag.Constants;
using AiKnowledgeAssistant.Application.Rag.Interface;
using AiKnowledgeAssistant.Application.Search.Interface;
using AiKnowledgeAssistant.Application.Search.Models;
using Microsoft.Extensions.Logging;
using System.Text;


namespace AiKnowledgeAssistant.Application.Rag.Implementation
{
    public sealed class RagService : IRagService
    {
        private readonly IAiEmbeddingClient _embeddingClient;
        private readonly IVectorSearchStore _vectorStore;
        private readonly IAiClient _aiClient;
        private readonly ILogger<RagService> _logger;


        public RagService(
            IAiEmbeddingClient embeddingClient,
            IVectorSearchStore vectorStore,
            IAiClient aiClient,
            ILogger<RagService> logger)
        {
            _embeddingClient = embeddingClient;
            _vectorStore = vectorStore;
            _aiClient = aiClient;
            _logger = logger;
        }

        public async Task<string> AskAsync(
            string question,
            CancellationToken cancellationToken)
        {
            // 1 Embed the question
            var queryVector = await _embeddingClient.GenerateEmbeddingAsync(
                question,
                cancellationToken);


            // 2️ Retrieve relevant chunks
            var chunks = await _vectorStore.SearchAsync(
                queryVector,
                topK: 5,
                cancellationToken);

            var relevant = chunks
                .Where(r => r.Score >= 0.70)
                .OrderByDescending(r => r.Score)
                .Take(3)
                .ToList();


            if (!relevant.Any())
            {
                _logger.LogInformation(
                "RAG rejected query due to low similarity");
                return "I don't have enough information to answer.";
            }

            var topScore = relevant.First().Score;


            var confidence = topScore >= 0.80
                ? RagConfidence.High
                : RagConfidence.Low;


            // 3️ Build grounded prompt
            var systemPrompt = BuildSystemPrompt(relevant, confidence);

            // 4️ Ask the LLM
            return await _aiClient.GetChatResponseAsync(
                systemPrompt,
                question,
                cancellationToken);
        }

        private static string BuildSystemPrompt(
            IReadOnlyList<VectorSearchResult> chunks, RagConfidence confidence)
        {
            var sb = new StringBuilder();

            sb.AppendLine("You are a factual assistant.");
            if (confidence == RagConfidence.High)
            {
                sb.AppendLine("Answer using the provided context.");
                sb.AppendLine("Be concise and direct.");
            }
            else
            {
                sb.AppendLine("The available context is partially relevant.");
                sb.AppendLine("Answer carefully and indicate that the explanation is based on limited information.");
                sb.AppendLine("Do NOT speculate beyond the context.");
            }
            sb.AppendLine("If the answer is not in the context, say:");
            sb.AppendLine("\"I don't have enough information to answer.\"");
            sb.AppendLine();
            sb.AppendLine("Context:");

            for (int i = 0; i < chunks.Count; i++)
            {
                sb.AppendLine($"[{i + 1}] {chunks[i].Content}");
            }

            return sb.ToString();
        }
    }

}
