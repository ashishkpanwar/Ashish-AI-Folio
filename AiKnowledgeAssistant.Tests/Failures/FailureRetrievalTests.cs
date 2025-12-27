using AiKnowledgeAssistant.Application.Failures.Interfaces;
using AiKnowledgeAssistant.Application.Failures.Queries;

namespace AiKnowledgeAssistant.Tests.Failures
{

    [Collection("TestHost collection")]
    public sealed class FailureRetrievalTests
    {
        private readonly IFailureRetrievalService _sut;

        public FailureRetrievalTests()
        {
            _sut = TestHost.GetService<IFailureRetrievalService>();

        }

        [Fact]
        public async Task Finds_similar_active_failures_for_service()
        {
            // Arrange
            var query = new FindSimilarFailuresQuery(
                Environment: "Prod",
                JobId: "1000",
                MinSeverity: 3,
                Question: "How to fix job worker failures?",
                OnlyActive: true,
                Top: 5);

            // Act
            var results = await _sut.FindSimilarAsync(
                query,
                CancellationToken.None);

            // Assert
            //results.Should().NotBeEmpty();
            Assert.NotEmpty(results);

            Assert.Contains(results, f =>
                f.Environment == "Prod" &&
                f.JobId == "1000" &&
                f.IsActive &&
                f.Severity >= 3);
        }
    }
}

