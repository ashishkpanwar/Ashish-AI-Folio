using AiKnowledgeAssistant.Application.Failures.Interfaces;
using AiKnowledgeAssistant.Application.Failures.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.Failures.Implementations
{
    public sealed class FailureSummaryBuilder: IFailureSummaryBuilder
    {
        public FailureTechnicalSummary Build(IReadOnlyList<FailureRecord> records)
        {
            if (records.Count == 0)
                throw new InvalidOperationException("No failure records found.");

            var failureCount = records.Count;

            var confidence = failureCount switch
            {
                1 => FailureConfidence.Low,
                <= 3 => FailureConfidence.Medium,
                _ => FailureConfidence.High
            };

            return new FailureTechnicalSummary
            {
                FailureCount = failureCount,
                FirstFailureAt = records.Min(r => r.Timestamp),
                LastFailureAt = records.Max(r => r.Timestamp),

                ErrorTypes = records
                    .GroupBy(r => r.ErrorType)
                    .ToDictionary(g => g.Key, g => g.Count()),

                CommonErrorMessages = records
                    .GroupBy(r => r.Content)
                    .OrderByDescending(g => g.Count())
                    .Take(3)
                    .Select(g => g.Key)
                    .ToList(),

                Confidence = confidence // this does not look right/helpful?
            };
        }
    }


}
