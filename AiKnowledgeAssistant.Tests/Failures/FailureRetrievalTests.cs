using AiKnowledgeAssistant.Application.AI;
using AiKnowledgeAssistant.Application.Failures.Interfaces;
using AiKnowledgeAssistant.Application.Failures.Queries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

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
                Content: "database timeout while processing job",
                Environment: "Prod",
                ServiceName: "JobWorker",
                MinSeverity: 3,
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
                f.ServiceName == "JobWorker" &&
                f.IsActive &&
                f.Severity >= 3);
        }
    }
}

