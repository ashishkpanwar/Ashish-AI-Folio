using AiKnowledgeAssistant.Application.AI;
using AiKnowledgeAssistant.Infrastructure.Resilience;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using Polly;
using Microsoft.Extensions.Logging;


namespace AiKnowledgeAssistant.Infrastructure.AI;

public sealed class AzureOpenAiClient: IAiClient
{
    private readonly ChatClient _chatClient;
    private readonly ILogger<AzureOpenAiClient> _logger;
    private readonly IAsyncPolicy _policy;

    public AzureOpenAiClient(ChatClient chatClient, ILogger<AzureOpenAiClient> logger)
    {
        _chatClient = chatClient;
        _logger = logger;
        _policy = AiResiliencePolicies.CreatePolicy();
    }

    public async Task<string> GetChatResponseAsync(
        string systemPrompt,
        string userPrompt,
        CancellationToken cancellationToken)
    {

        return await _policy.ExecuteAsync(
             ct => GetChatResponseInternalAsync(
                _chatClient,
                systemPrompt,
                userPrompt,
                ct),
            cancellationToken);

    }

    private async Task<string> GetChatResponseInternalAsync(
        ChatClient chatClient,
        string systemPrompt,
        string userPrompt,
        CancellationToken cancellationToken)
    {
        try
        {
            var options = new ChatCompletionOptions
            {
                Temperature = 0.2f,
                MaxOutputTokenCount = 500
            };
            var response = await chatClient.CompleteChatAsync(
                [
                    new SystemChatMessage(systemPrompt),
                    new UserChatMessage(userPrompt)
                ],
                options,
                cancellationToken);
            return response.Value.Content[0].Text;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting chat response from Azure OpenAI.");
            throw;
        }
    }
}
