namespace AiKnowledgeAssistant.Application.AI;

public interface IAiClient
{
    Task<string> GetChatResponseAsync(
        string systemPrompt,
        string userPrompt,
        CancellationToken cancellationToken);
}

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
