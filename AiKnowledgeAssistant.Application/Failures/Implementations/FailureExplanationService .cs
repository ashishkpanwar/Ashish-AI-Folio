using AiKnowledgeAssistant.Application.AI.Interfaces;
using AiKnowledgeAssistant.Application.Failures.Interfaces;
using AiKnowledgeAssistant.Application.Failures.Models;
using AiKnowledgeAssistant.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.Failures.Implementations
{
    public sealed class FailureExplanationService : IFailureExplanationService
    {
        private readonly IAiClient _chatClient;
        private readonly ITokenGuardrail _tokenGuardrail;

        public FailureExplanationService(
            IAiClient chatClient,
            ITokenGuardrail tokenGuardrail)
        {
            _chatClient = chatClient;
            _tokenGuardrail = tokenGuardrail;
        }

        public async Task<FailureExplanationResult> ExplainAsync(
            FailureInsight insight,
            CancellationToken cancellationToken)
        {
            var prompt = FailureExplanationPromptBuilder.Build(insight);

            var estimatedTokens = TokenEstimator.EstimateTokens(prompt);

            if (!_tokenGuardrail.CanProceed(estimatedTokens, out var reason))
            {
                return FailureExplanationResult.Skipped(reason);
            }

            var explanation = await _chatClient.GetChatResponseAsync(
                prompt,
                string.Empty, //we will restrospect later if user prompt is needed
                cancellationToken);

            return FailureExplanationResult.Generated(explanation);
        }
    }

}
