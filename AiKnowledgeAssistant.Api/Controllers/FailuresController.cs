using AiKnowledgeAssistant.Application.Failures.Interfaces;
using AiKnowledgeAssistant.Application.Failures.Queries;
using Microsoft.AspNetCore.Mvc;

namespace AiKnowledgeAssistant.Api.Controllers
{
    [ApiController]
    [Route("api/failure")]
    public class FailuresController: ControllerBase
    {
        private readonly IFailureAnalysisService _analysisService;
        public FailuresController(
            IFailureAnalysisService analysisService)
        {
            _analysisService = analysisService;
        }

        [HttpPost("analyze")]
        public async Task<IActionResult> Analyze(
        [FromBody] FindSimilarFailuresQuery query,
        CancellationToken cancellationToken)
        {
            var result = await _analysisService.AnalyzeAsync(
                query,
                cancellationToken);

            return Ok(result);
        }

    }
}
