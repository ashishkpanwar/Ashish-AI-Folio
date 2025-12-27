using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.Failures.Models
{
    public sealed class FailureTechnicalSummary
    {
        public int FailureCount { get; init; }

        public DateTimeOffset FirstFailureAt { get; init; }
        public DateTimeOffset LastFailureAt { get; init; }

        public IReadOnlyDictionary<string, int> ErrorTypes { get; init; } =
            new Dictionary<string, int>();

        public IReadOnlyList<string> CommonErrorMessages { get; init; } = [];

        public FailureConfidence Confidence { get; init; } 
    }

    public enum FailureConfidence
    {
        Low,     // 1 failure
        Medium,  // 2–3 failures
        High     // 4+ failures
    }


}
