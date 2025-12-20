using AiKnowledgeAssistant.Application.Failures.Implementations;
using AiKnowledgeAssistant.Application.Failures.Interfaces;
using AiKnowledgeAssistant.Application.Failures.Models;
using AiKnowledgeAssistant.Application.Failures.Queries;
using Moq;
using Xunit;

namespace AiKnowledgeAssistant.Tests.Failures
{

    public sealed class FailureAnalysisServiceTests
    {
        private readonly Mock<IFailureRetrievalService> _retrievalService = new();
        private readonly Mock<IFailureInsightBuilder> _insightBuilder = new();
        private readonly Mock<IFailureExplanationService> _explanationService = new();

        private FailureAnalysisService CreateSut()
        {
            return new FailureAnalysisService(
                _retrievalService.Object,
                _insightBuilder.Object,
                _explanationService.Object);
        }

        [Fact]
        public async Task AnalyzeAsync_returns_insight_and_explanation_when_ai_runs()
        {
            // Arrange
            var query = CreateQuery();

            var failures = new List<FailureRecord>
            {
                new() { Severity = 3 }
            };

            var insight = new FailureInsight
            {
                IsKnownFailure = true,
                CurrentSeverity = 3
            };

            var explanation = FailureExplanationResult.Generated(
                "This is a known failure with moderate severity.");

            _retrievalService
                .Setup(r => r.FindSimilarAsync(query.SimilarFailures, It.IsAny<CancellationToken>()))
                .ReturnsAsync(failures);

            _insightBuilder
                .Setup(b => b.Build(failures))
                .Returns(insight);

            _explanationService
                .Setup(e => e.ExplainAsync(insight,query.Question ?? string.Empty, It.IsAny<CancellationToken>()))
                .ReturnsAsync(explanation);

            var sut = CreateSut();

            // Act
            var result = await sut.AnalyzeAsync(query, CancellationToken.None);

            // Assert
            Assert.Same(insight, result.Insight);
            Assert.Same(explanation, result.Explanation);
            Assert.True(result.Explanation.IsGenerated);
        }

        [Fact]
        public async Task AnalyzeAsync_returns_insight_when_ai_is_skipped()
        {
            // Arrange
            var query = CreateQuery();

            var failures = new List<FailureRecord>
        {
            new() { Severity = 2 }
        };

            var insight = new FailureInsight
            {
                IsKnownFailure = true,
                CurrentSeverity = 2
            };

            var skippedExplanation =
                FailureExplanationResult.Skipped("Token limit exceeded");

            _retrievalService
                .Setup(r => r.FindSimilarAsync(query.SimilarFailures, It.IsAny<CancellationToken>()))
                .ReturnsAsync(failures);

            _insightBuilder
                .Setup(b => b.Build(failures))
                .Returns(insight);

            _explanationService
                .Setup(e => e.ExplainAsync(insight, query.Question ?? string.Empty, It.IsAny<CancellationToken>()))
                .ReturnsAsync(skippedExplanation);

            var sut = CreateSut();

            // Act
            var result = await sut.AnalyzeAsync(query, CancellationToken.None);

            // Assert
            Assert.Same(insight, result.Insight);
            Assert.False(result.Explanation.IsGenerated);
            Assert.NotNull(result.Explanation.SkippedReason);
        }

        [Fact]
        public async Task AnalyzeAsync_calls_dependencies_in_correct_sequence()
        {
            // Arrange
            var query = CreateQuery();
            var failures = new List<FailureRecord>();
            var insight = new FailureInsight();

            var sequence = new MockSequence();

            _retrievalService
                .InSequence(sequence)
                .Setup(r => r.FindSimilarAsync(query.SimilarFailures, It.IsAny<CancellationToken>()))
                .ReturnsAsync(failures);

            _insightBuilder
                .InSequence(sequence)
                .Setup(b => b.Build(failures))
                .Returns(insight);

            _explanationService
                .InSequence(sequence)
                .Setup(e => e.ExplainAsync(insight,query.Question?? string.Empty, It.IsAny<CancellationToken>()))
                .ReturnsAsync(FailureExplanationResult.Skipped("Skipped"));

            var sut = CreateSut();

            // Act
            await sut.AnalyzeAsync(query, CancellationToken.None);

            // Assert
            _retrievalService.VerifyAll();
            _insightBuilder.VerifyAll();
            _explanationService.VerifyAll();
        }

        private static FailureAnalysisRequest CreateQuery()
        {
            var query = new FindSimilarFailuresQuery(
                Content: "database timeout",
                Environment: "Prod",
                ServiceName: "JobWorker",
                MinSeverity: 2,
                OnlyActive: true,
                Top: 5);

            return new FailureAnalysisRequest()
            {
                SimilarFailures = query,
                Question = "Is this severe?"
            };
        }
    }

}
