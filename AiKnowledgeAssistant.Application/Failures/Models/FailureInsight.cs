using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.Failures.Models
{
    public sealed class FailureInsight
    {
        public bool IsKnownFailure { get; init; }

        // PRIMARY
        public int CurrentSeverity { get; init; }

        // CONTEXT
        public int MaxSeverityObserved { get; init; }
        public int MinSeverityObserved { get; init; }

        public int OccurrenceCount { get; init; }
        public DateTimeOffset? LastSeenAt { get; init; }
        public bool HasActiveFailures { get; init; }
    }

}
