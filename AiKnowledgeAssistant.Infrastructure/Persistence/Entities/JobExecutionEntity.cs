using AiKnowledgeAssistant.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Infrastructure.Persistence.Entities
{
    public sealed class JobExecutionEntity
    {
        public string JobId { get; set; } = default!;
        public string WorkflowId { get; set; } = default!;
        public string Environment { get; set; } = default!;
        public JobStatus Status { get; set; }
        public DateTimeOffset ExecutedAt { get; set; }
    }

}
