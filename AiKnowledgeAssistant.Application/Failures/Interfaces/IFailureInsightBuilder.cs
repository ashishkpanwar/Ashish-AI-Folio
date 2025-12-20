using AiKnowledgeAssistant.Application.Failures.Models;

namespace AiKnowledgeAssistant.Application.Failures.Interfaces
{

    public interface IFailureInsightBuilder
    {
        FailureInsight Build(IReadOnlyList<FailureRecord> failures);
    }

}
