using AiKnowledgeAssistant.Api.Dtos.Request;
using AiKnowledgeAssistant.Api.Dtos.Response;
using AiKnowledgeAssistant.Application.AI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AiKnowledgeAssistant.Api.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IAiClient _aiClient;
        private readonly IAiEmbeddingClient _aiEmbeddingClient;

        public ChatController(IAiClient aiClient, IAiEmbeddingClient aiEmbeddingClient)
        {
            _aiClient = aiClient;
            _aiEmbeddingClient = aiEmbeddingClient;
        }

        [HttpPost("chat-embedding")]
        public async Task<IActionResult> ChatEmbedding(
            [FromBody] IEnumerable<string> request,
            CancellationToken cancellationToken)
        {
            List<ChatEmbedding> response = new List<ChatEmbedding>();
            foreach (var chat in request)
            {
              var embedding = await _aiEmbeddingClient.GenerateEmbeddingAsync(
              chat,
              cancellationToken);
              var chatEmbedidng = new ChatEmbedding {Question = chat , Length = embedding.Length}; 
              response.Add(chatEmbedidng);
            }

            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> Chat(
            [FromBody] ChatRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _aiClient.GetChatResponseAsync(
                "You are a helpful assistant.",
                request.Question,
                cancellationToken);

            return Ok(new { response });
        }
    }
}
