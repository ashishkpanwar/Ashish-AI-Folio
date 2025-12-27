using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.Failures.Models
{
    public class FailureWindow
    {
        public string WorkflowId { get; init; } = default!;
        public string Environment { get; init; } = default!;
        public IEnumerable<Infrastructure.Repositories.Models.JobExecution> FailedJobs { get; init; } = default!;
        public string? PreviousSuccessJobId { get; init; }
    }
}
