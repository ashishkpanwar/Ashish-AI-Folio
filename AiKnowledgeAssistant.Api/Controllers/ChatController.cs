using AiKnowledgeAssistant.Application.AI;
using Microsoft.AspNetCore.Mvc;

namespace AiKnowledgeAssistant.Api.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IAiClient _aiClient;

        public ChatController(IAiClient aiClient)
        {
            _aiClient = aiClient;
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

    public record ChatRequest(string Question);

}
