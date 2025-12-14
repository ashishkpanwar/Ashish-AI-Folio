using AiKnowledgeAssistant.Application.AI;
using AiKnowledgeAssistant.Application.Rag.Interface;
using AiKnowledgeAssistant.Application.Search;
using System.Text;

namespace AiKnowledgeAssistant.Application.Rag.Implementation
{
    public sealed class RagService : IRagService
    {
        private readonly IAiEmbeddingClient _embeddingClient;
        private readonly IVectorSearchStore _vectorStore;
        private readonly IAiClient _aiClient;

        public RagService(
            IAiEmbeddingClient embeddingClient,
            IVectorSearchStore vectorStore,
            IAiClient aiClient)
        {
            _embeddingClient = embeddingClient;
            _vectorStore = vectorStore;
            _aiClient = aiClient;
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
                topK: 3,
                cancellationToken);

            // 3️ Build grounded prompt
            var systemPrompt = BuildSystemPrompt(chunks);

            // 4️ Ask the LLM
            return await _aiClient.GetChatResponseAsync(
                systemPrompt,
                question,
                cancellationToken);
        }

        private static string BuildSystemPrompt(
            IReadOnlyList<string> chunks)
        {
            var sb = new StringBuilder();

            sb.AppendLine("You are a factual assistant.");
            sb.AppendLine("Answer ONLY using the provided context.");
            sb.AppendLine("If the answer is not in the context, say:");
            sb.AppendLine("\"I don't have enough information to answer.\"");
            sb.AppendLine();
            sb.AppendLine("Context:");

            for (int i = 0; i < chunks.Count; i++)
            {
                sb.AppendLine($"[{i + 1}] {chunks[i]}");
            }

            return sb.ToString();
        }
    }

}
