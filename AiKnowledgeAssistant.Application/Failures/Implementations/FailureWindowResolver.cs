using AiKnowledgeAssistant.Application.Failures.Interfaces;
using AiKnowledgeAssistant.Application.Failures.Models;
using AiKnowledgeAssistant.Domain.Enums;
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

        public async Task<FailureWindow> ResolveAsync(string jobId, int lookBack = 1, int maxJobs = 50)
        {
            JobExecution job = await _jobExecutionReader.GetByIdAsync(jobId)
                      ?? throw new InvalidOperationException($"Job '{jobId}' not found.");

            if (job.Status != JobStatus.Failed)
                throw new InvalidOperationException("Failure window can only be resolved for failed jobs.");

            TimeSpan lookbackTime =
                TimeSpan.FromDays(lookBack);

            IReadOnlyList<JobExecution> failedJobs = await _jobExecutionReader.GetFailureWindowAsync(
                    job.WorkflowId,
                    job.Environment,
                    job.ExecutedAt,
                    lookbackTime,
                    maxJobs); // this should include ExecutedAt filtering in the repository layer for performance

            if (failedJobs.Count == 0)
                throw new InvalidOperationException(
                    "Failure window resolution returned no failed jobs.");

            return new FailureWindow
            {
                WorkflowId = job.WorkflowId,
                Environment = job.Environment,
                FailedJobs = failedJobs,
            };
        }
    }

}
