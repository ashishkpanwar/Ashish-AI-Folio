using AiKnowledgeAssistant.Application.Failures.Interfaces;
using AiKnowledgeAssistant.Application.Failures.Models;
using AiKnowledgeAssistant.Infrastructure.Repositories.Interfaces;
using AiKnowledgeAssistant.Infrastructure.Repositories.Models;

namespace AiKnowledgeAssistant.Application.Failures.Implementations
{
    public sealed class FailureWindowResolver: IFailureWindowResolver
    {
        private readonly IJobExecutionReader _jobExecutionReader;

        public FailureWindowResolver(IJobExecutionReader jobRepository)
        {
            _jobExecutionReader = jobRepository;
        }

        public async Task<FailureWindow> ResolveAsync(string jobId)
        {
            JobExecution job = await _jobExecutionReader.GetByIdAsync(jobId)
                      ?? throw new InvalidOperationException($"Job '{jobId}' not found.");

            if (job.Status != JobStatus.Failed)
                throw new InvalidOperationException("Failure window can only be resolved for failed jobs.");

            TimeSpan DefaultLookback =
                TimeSpan.FromDays(1);

            int DefaultMaxJobs = 50;

            IReadOnlyList<JobExecution> failedJobs = await _jobExecutionReader.GetFailureWindowAsync(
                    job.WorkflowId,
                    job.Environment,
                    job.ExecutedAt,
                    DefaultLookback,
                    DefaultMaxJobs); // this should include ExecutedAt filtering in the repository layer for performance

            if (failedJobs.Count == 0)
                throw new InvalidOperationException(
                    "Failure window resolution returned no failed jobs.");
            var firstFailedJob = failedJobs.OrderByDescending(job => job.ExecutedAt).First().JobId;

            return new FailureWindow
            {
                WorkflowId = job.WorkflowId,
                Environment = job.Environment,
                FailedJobs = failedJobs,
                FirstFailedJob = firstFailedJob
            };
        }
    }

}
