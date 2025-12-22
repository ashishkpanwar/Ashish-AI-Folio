using AiKnowledgeAssistant.Ingestion.Application.Abstractions;
using AiKnowledgeAssistant.Ingestion.Infrastructure.Search;

namespace AiKnowledgeAssistant.Ingestion.Application.Pipeline;

public sealed class FailureIngestionPipeline
{
    private readonly ILogSource _logSource;
    private readonly ILogNormalizer _normalizer;
    private readonly FailureIngestionService _ingestionService;

    public FailureIngestionPipeline(
        ILogSource logSource,
        ILogNormalizer normalizer,
        FailureIngestionService ingestionService)
    {
        _logSource = logSource;
        _normalizer = normalizer;
        _ingestionService = ingestionService;
    }

    public async Task RunAsync(
        DateTimeOffset from,
        DateTimeOffset to,
        CancellationToken cancellationToken)
    {
        var logs = await _logSource.FetchAsync(from, to, cancellationToken);

        foreach (var log in logs)
        {
            var failure = _normalizer.Normalize(log);

            if (failure is null)
                continue;

            await _ingestionService.IngestAsync(
                failure,
                cancellationToken);
        }
    }
}
