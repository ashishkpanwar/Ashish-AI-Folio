using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.Failures.Models
{
    public sealed class FailureExplanationResult
    {
        public bool IsGenerated { get; init; }
        public string? Explanation { get; init; }
        public string? SkippedReason { get; init; }

        public static FailureExplanationResult Skipped(string reason) =>
            new()
            {
                IsGenerated = false,
                SkippedReason = reason
            };

        public static FailureExplanationResult Generated(string explanation) =>
            new()
            {
                IsGenerated = true,
                Explanation = explanation
            };
    }

}
