using AiKnowledgeAssistant.Application.Failures.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.Failures.Interfaces
{
    public interface IFailureExplanationService
    {
        Task<FailureExplanationResult> ExplainAsync(
            FailureInsight insight,
            string question,
            CancellationToken cancellationToken);
    }
}
