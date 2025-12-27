using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.Failures.Models;

public sealed class FailureRecord
{
    public string Content { get; init; } = default!;
    public DateTimeOffset Timestamp { get; init; }
    public required string JobId { get; init; }
    public string Environment { get; init; } = default!;
    public string ErrorType { get; init; } = default!;
    public int Severity { get; init; }
    public bool IsActive { get; init; }

}

