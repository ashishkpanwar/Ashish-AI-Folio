namespace AiKnowledgeAssistant.Ingestion.Application.Abstractions;

using AiKnowledgeAssistant.Ingestion.Application.Models;

public interface ILogNormalizer
{
    FailureRecord? Normalize(RawLogEntry log);
}
