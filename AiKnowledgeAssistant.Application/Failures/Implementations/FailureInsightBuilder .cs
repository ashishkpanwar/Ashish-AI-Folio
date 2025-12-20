using AiKnowledgeAssistant.Application.Failures.Interfaces;
using AiKnowledgeAssistant.Application.Failures.Models;


namespace AiKnowledgeAssistant.Application.Failures.Implementations
{
    public sealed class FailureInsightBuilder : IFailureInsightBuilder
    {
        public FailureInsight Build(IReadOnlyList<FailureRecord> failures)
        {
            if (failures.Count == 0)
            {
                return new FailureInsight
                {
                    IsKnownFailure = false,
                    OccurrenceCount = 0
                };
            }

            return new FailureInsight
            {
                IsKnownFailure = true,
                OccurrenceCount = failures.Count,
                LastSeenAt = failures.Max(f => f.Timestamp),
                MaxSeverityObserved = failures.Max(f => f.Severity),
                MinSeverityObserved = failures.Min(f => f.Severity),
                HasActiveFailures = failures.Any(f => f.IsActive),
                CurrentSeverity = failures
                                    .OrderByDescending(f => f.Timestamp)
                                    .First().Severity

            };
        }
    }

}
