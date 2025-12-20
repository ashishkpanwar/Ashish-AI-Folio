using AiKnowledgeAssistant.Application.Failures.Implementations;
using AiKnowledgeAssistant.Application.Failures.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Tests.Failures
{
    [Collection("TestHost collection")]
    public sealed class FailureInsightBuilderTests
    {
        [Fact]
        public void Builds_correct_insights_for_known_failures()
        {
            var failures = new List<FailureRecord>
        {
            new()
            {
                Severity = 3,
                IsActive = true,
                Timestamp = DateTimeOffset.UtcNow.AddHours(-2)
            },
            new()
            {
                Severity = 5,
                IsActive = false,
                Timestamp = DateTimeOffset.UtcNow
            }
        };

            var builder = new FailureInsightBuilder();

            var insight = builder.Build(failures);

            Assert.True(insight.IsKnownFailure);
            Assert.Equal(2, insight.OccurrenceCount);
            Assert.Equal(5, insight.MaxSeverityObserved);
            Assert.Equal(3, insight.MinSeverityObserved);
            Assert.Equal(5, insight.CurrentSeverity);
            Assert.True(insight.HasActiveFailures);
            Assert.NotNull(insight.LastSeenAt);
        }
    }

}
