using AiKnowledgeAssistant.Ingestion.Application.Abstractions;
using AiKnowledgeAssistant.Ingestion.Application.Models;

namespace AiKnowledgeAssistant.Ingestion.Application.Normalization;

public sealed class DefaultLogNormalizer : ILogNormalizer
{
    public FailureRecord? Normalize(RawLogEntry log)
    {
        if (string.IsNullOrWhiteSpace(log.Level))
            return null;

        var level = log.Level.Trim().ToUpperInvariant();

        // Ignore informational noise
        if (level == "INFO")
            return null;

        var severity = level switch
        {
            "WARN" => 2,
            "ERROR" => InferErrorSeverity(log.Message),
            "CRITICAL" => 5,
            _ => 3
        };

        var errorType = InferErrorType(log.Message);

        return new FailureRecord
        {
            Id = GenerateId(log),
            Content = NormalizeMessage(log.Message),
            Timestamp = log.Timestamp,

            ServiceName = log.ServiceName ?? "UnknownService",
            WorkflowName = log.WorkflowName ?? "UnknownWorkflow",
            Environment = "Prod", // injected later via context
            ErrorType = errorType,

            Severity = severity,
            IsActive = level == "CRITICAL",
            Source = log.Source ?? "UnknownSource"
        };
    }

    private static int InferErrorSeverity(string message)
    {
        if (message.Contains("abort", StringComparison.OrdinalIgnoreCase))
            return 4;

        if (message.Contains("failed", StringComparison.OrdinalIgnoreCase))
            return 3;

        return 3;
    }

    private static string InferErrorType(string message)
    {
        var text = message.ToLowerInvariant();

        if (text.Contains("license"))
            return "LicenseIssue";

        if (text.Contains("timeout"))
            return "Timeout";

        if (text.Contains("network"))
            return "Network";

        if (text.Contains("latency"))
            return "Performance";

        return "Unknown";
    }

    private static string NormalizeMessage(string message)
    {
        // Trim noise, keep meaning
        return message.Trim();
    }

    private static string GenerateId(RawLogEntry log)
    {
        return $"{log.Timestamp:yyyyMMddHHmmss}-{Guid.NewGuid():N}";
    }
}
