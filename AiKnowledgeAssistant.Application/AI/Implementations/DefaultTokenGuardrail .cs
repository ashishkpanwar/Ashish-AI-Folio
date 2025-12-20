using AiKnowledgeAssistant.Application.AI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.AI.Implementations
{
    public sealed class DefaultTokenGuardrail : ITokenGuardrail
    {
        // Conservative limit for explanation prompts
        private const int MaxInputTokens = 2_000;

        public bool CanProceed(
            int estimatedInputTokens,
            out string reason)
        {
            if (estimatedInputTokens <= 0)
            {
                reason = "Estimated token count is invalid.";
                return false;
            }

            if (estimatedInputTokens > MaxInputTokens)
            {
                reason =
                    $"Estimated token count ({estimatedInputTokens}) exceeds allowed limit ({MaxInputTokens}).";
                return false;
            }

            reason = string.Empty;
            return true;
        }
    }

}
