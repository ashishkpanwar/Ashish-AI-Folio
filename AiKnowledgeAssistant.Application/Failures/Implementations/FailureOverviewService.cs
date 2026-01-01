using AiKnowledgeAssistant.Application.Failures.Interfaces;
using AiKnowledgeAssistant.Application.Failures.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.Failures.Implementations
{
    public sealed class FailureOverviewService
    {
        private readonly IFailureWindowResolver _windowResolver;
        private readonly IFailureRecordReader _failureRecordReader; //like repository
        private readonly IFailureSummaryBuilder _summaryBuilder;
        private readonly IFailureAiSummaryGenerator _aiSummaryGenerator;

        public FailureOverviewService(
            IFailureWindowResolver windowResolver,
            IFailureRecordReader failureRecordReader,
            IFailureSummaryBuilder summaryBuilder,
            IFailureAiSummaryGenerator aiSummaryGenerator)
        {
            _windowResolver = windowResolver;
            _failureRecordReader = failureRecordReader;
            _summaryBuilder = summaryBuilder;
            _aiSummaryGenerator = aiSummaryGenerator;
        }

        public async Task<FailureOverviewResponse> GetOverviewAsync(string jobId)
        {
            // 1️ Resolve continuous failure window
            FailureWindow window = await _windowResolver.ResolveAsync(jobId);

            List<string> jobIds = window.FailedJobs
                .Select(j => j.JobId)
                .ToList();

            // 2️ Load failure records for those jobs from AzureLog
            IReadOnlyList<FailureRecord> failedRecords = await _failureRecordReader.GetByJobIdsAsync(jobIds);

            if (failedRecords.Count == 0)
                throw new InvalidOperationException(
                    "Failure window resolved but no failure records found.");

            var jobFailureRecords =
                failedRecords.Where(r => r.JobId == jobId).ToList();

            // 3️ Build deterministic technical summary
            FailureTechnicalSummary technicalSummary = _summaryBuilder.Build(jobFailureRecords);

            // 4️ Generate AI summary (human-readable)
            var aiSummary = await _aiSummaryGenerator.GenerateAsync(
                technicalSummary,
                failedRecords);

            // 5️⃣ Compose final response
            return new FailureOverviewResponse
            {
                JobId = jobId,
                WorkflowId = window.WorkflowId,
                Environment = window.Environment,
                TechnicalSummary = technicalSummary,
                AiSummary = aiSummary
            };
        }
    }

}
