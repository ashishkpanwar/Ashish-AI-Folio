using AiKnowledgeAssistant.Application.Failures.Models;
using AiKnowledgeAssistant.Infrastructure.Repositories.Interfaces;
using AiKnowledgeAssistant.Infrastructure.Repositories.Models;

namespace AiKnowledgeAssistant.Application.Failures.Implementations
{
    public sealed class FailureWindowResolver
    {
        private readonly IJobExecutionReader _jobRepository;

        public FailureWindowResolver(IJobExecutionReader jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<FailureWindow> ResolveAsync(string jobId)
        {
            JobExecution job = await _jobRepository.GetByIdAsync(jobId)
                      ?? throw new InvalidOperationException($"Job '{jobId}' not found.");

            if (job.Status != JobStatus.Failed)
                throw new InvalidOperationException("Failure window can only be resolved for failed jobs.");

            IReadOnlyList<JobExecution> scopedJobs = await _jobRepository.GetByWorkflowAndEnvironmentAsync(
                job.WorkflowId,
                job.Environment); // this should include ExecutedAt filtering in the repository layer for performance

            var orderedJobs = scopedJobs
                .Where(j => j.ExecutedAt <= job.ExecutedAt)
                .OrderByDescending(j => j.ExecutedAt);

            var failedJobs = new List<JobExecution>();
            string? previousSuccessJobId = null;

            foreach (var scopedJob in orderedJobs)
            {
                if (scopedJob.Status == JobStatus.Success)
                {
                    previousSuccessJobId = scopedJob.JobId;
                    break;
                }

                failedJobs.Add(scopedJob);
            }

            return new FailureWindow
            {
                WorkflowId = job.WorkflowId,
                Environment = job.Environment,
                FailedJobs = failedJobs,
                PreviousSuccessJobId = previousSuccessJobId
            };
        }
    }

}
