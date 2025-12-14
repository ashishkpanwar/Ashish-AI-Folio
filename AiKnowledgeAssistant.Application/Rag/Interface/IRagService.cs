using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.Rag.Interface
{
    public interface IRagService
    {
        Task<string> AskAsync(
            string question,
            CancellationToken cancellationToken);
    }

}
