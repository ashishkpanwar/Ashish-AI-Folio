using AiKnowledgeAssistant.Infrastructure.Repositories.Models;


namespace AiKnowledgeAssistant.Application.Failures.Models
{
    public class FailureWindow
    {
        public string WorkflowId { get; init; } = default!;
        public string Environment { get; init; } = default!;
        public IEnumerable<JobExecution> FailedJobs { get; init; } = default!;
    }
}
