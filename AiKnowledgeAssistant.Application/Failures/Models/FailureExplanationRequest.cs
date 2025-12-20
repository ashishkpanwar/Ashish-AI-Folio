using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.Failures.Models
{
    public sealed class FailureExplanationRequest
    {
        public string Question { get; init; } = default!;
    }
}
