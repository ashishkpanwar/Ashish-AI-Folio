using AiKnowledgeAssistant.Application.Failures.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.Failures.Models
{
    public sealed class FailureAnalysisRequest
    {
        public FindSimilarFailuresQuery SimilarFailures { get; init; } = default!;
        public string? Question { get; init; }
    }
}
