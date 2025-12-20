using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.Failures.Models
{

    public sealed class FailureAnalysisResult
    {
        public FailureInsight Insight { get; init; } = default!;
        public FailureExplanationResult Explanation { get; init; } = default!;
    }
}
