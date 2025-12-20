using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.Helpers
{
    public static class TokenEstimator
    {
        // Simple token estimation: 1 token ~ 4 characters in English text
        public static int EstimateTokens(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }
            return text.Length / 4;
        }
    }
}
