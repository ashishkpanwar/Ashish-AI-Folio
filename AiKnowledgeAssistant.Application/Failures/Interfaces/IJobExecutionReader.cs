using AiKnowledgeAssistant.Infrastructure.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Infrastructure.Repositories.Interfaces
{
    public interface IJobExecutionReader
    {
        Task<JobExecution?> GetByIdAsync(string jobId);

        Task<IReadOnlyList<JobExecution>> GetFailureWindowAsync(
        string workflowId,
        string environment,
        DateTimeOffset fromTime,
        TimeSpan maxLookback,
        int maxJobs);
    }
}
