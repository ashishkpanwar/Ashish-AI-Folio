using AiKnowledgeAssistant.Domain.Enums;

namespace AiKnowledgeAssistant.Infrastructure.Repositories.Models
{
    public sealed class JobExecution
    {
        public string JobId { get; init; } = default!;
        public string WorkflowId { get; init; } = default!;
        public string Environment { get; init; } = default!;
        public JobStatus Status { get; init; }
        public DateTimeOffset ExecutedAt { get; init; }
    }

}
