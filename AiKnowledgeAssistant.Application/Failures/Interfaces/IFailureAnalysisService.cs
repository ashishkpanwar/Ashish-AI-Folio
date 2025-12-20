using AiKnowledgeAssistant.Application.Failures.Models;
using AiKnowledgeAssistant.Application.Failures.Queries;


namespace AiKnowledgeAssistant.Application.Failures.Interfaces
{
    public interface IFailureAnalysisService
    {
        Task<FailureAnalysisResult> AnalyzeAsync(
            FindSimilarFailuresQuery query,
            CancellationToken cancellationToken);
    }

}
