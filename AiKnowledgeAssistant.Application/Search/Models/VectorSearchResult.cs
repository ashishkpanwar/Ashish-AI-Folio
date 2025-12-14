using System;
using System.Collections.Generic;
using System.Text;

namespace AiKnowledgeAssistant.Application.Search.Models
{
    public sealed record VectorSearchResult(
    string Content,
    double Score);

}
