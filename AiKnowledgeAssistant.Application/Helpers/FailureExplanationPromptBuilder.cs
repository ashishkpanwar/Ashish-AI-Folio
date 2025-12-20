using AiKnowledgeAssistant.Application.Failures.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.Helpers
{
    public static class FailureExplanationPromptBuilder
    {
        public static string Build(FailureInsight insight)
        {
            return $"""
            You are an assistant helping engineers understand job failures.

            Known failure: {insight.IsKnownFailure}
            Current severity: {insight.CurrentSeverity}
            Historical severity range: {insight.MinSeverityObserved}–{insight.MaxSeverityObserved}
            Occurrence count: {insight.OccurrenceCount}
            Last seen: {insight.LastSeenAt}
            Active failures present: {insight.HasActiveFailures}

            Explain what this means operationally.
            Avoid speculation. Do not invent causes.
            """;
        }
    }

}
