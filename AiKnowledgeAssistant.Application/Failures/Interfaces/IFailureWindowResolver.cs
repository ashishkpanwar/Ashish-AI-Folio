using AiKnowledgeAssistant.Application.Failures.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.Failures.Interfaces
{
    public interface IFailureWindowResolver
    {
        Task<FailureWindow> ResolveAsync(string jobId, int lookBack = 1, int maxJobs = 50);
    }
}
