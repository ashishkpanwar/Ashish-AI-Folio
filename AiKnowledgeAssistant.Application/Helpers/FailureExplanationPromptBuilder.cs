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

            Failure summary:
            - Known failure: {insight.IsKnownFailure}
            - Current severity: {insight.CurrentSeverity}
            - Historical severity range: {insight.MinSeverityObserved}–{insight.MaxSeverityObserved}
            - Occurrence count: {insight.OccurrenceCount}
            - Last seen: {insight.LastSeenAt}
            - Active failures present: {insight.HasActiveFailures}


            Rules:
            - Answer only using the failure summary
            - Do not invent causes
            - Do not speculate beyond the data
            - Be concise and operational
            - If information is insufficient, say so
            """;
        }
    }


}
