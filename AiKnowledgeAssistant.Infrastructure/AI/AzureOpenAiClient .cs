using AiKnowledgeAssistant.Application.AI;
using Azure.AI.OpenAI;
using OpenAI.Chat;

namespace AiKnowledgeAssistant.Infrastructure.AI;

public sealed class AzureOpenAiClient: IAiClient
{
    private readonly AzureOpenAIClient _client;
    private readonly string _chatDeployment;

    public AzureOpenAiClient(AzureOpenAIClient client, string chatDeployment)
    {
        _client = client;
        _chatDeployment = chatDeployment;
    }

    public async Task<string> GetChatResponseAsync(
        string systemPrompt,
        string userPrompt,
        CancellationToken cancellationToken)
    {
        ChatClient chatClient = _client.GetChatClient(_chatDeployment);

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
}
