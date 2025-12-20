using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.AI.Interfaces
{
    public interface ITokenGuardrail
    {
        bool CanProceed(int estimatedTokens, out string reason);
    }
}
