using AiKnowledgeAssistant.Application.Failures.Interfaces;
using AiKnowledgeAssistant.Application.Failures.Models;
using AiKnowledgeAssistant.Application.Failures.Queries;

namespace AiKnowledgeAssistant.Application.Failures.Implementations
{
    public sealed class FailureAnalysisService : IFailureAnalysisService
    {
        private readonly IFailureRetrievalService _retrievalService;
        private readonly IFailureInsightBuilder _insightBuilder;
        private readonly IFailureExplanationService _explanationService;

        public FailureAnalysisService(
            IFailureRetrievalService retrievalService,
            IFailureInsightBuilder insightBuilder,
            IFailureExplanationService explanationService)
        {
            _retrievalService = retrievalService;
            _insightBuilder = insightBuilder;
            _explanationService = explanationService;
        }

        public async Task<FailureAnalysisResult> AnalyzeAsync(
            FailureAnalysisRequest request,
            CancellationToken cancellationToken)
        {
            // 1️
            var failures = await _retrievalService.FindSimilarAsync(
                request.SimilarFailures,
                cancellationToken);

            // 2️ Deterministic insights
            var insight = _insightBuilder.Build(failures);

            // 3️ Optional AI explanation (guarded internally)
            var explanation = await _explanationService.ExplainAsync(
                insight,
                request.Question ?? string.Empty,
                cancellationToken);

            return new FailureAnalysisResult
            {
                Insight = insight,
                Explanation = explanation
            };
        }
    }

}
