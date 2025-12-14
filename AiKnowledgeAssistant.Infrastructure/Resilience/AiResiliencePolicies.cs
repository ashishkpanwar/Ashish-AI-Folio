using Polly;
using Polly.Timeout;

namespace AiKnowledgeAssistant.Infrastructure.Resilience;

public static class AiResiliencePolicies
{
    public static IAsyncPolicy CreatePolicy()
    {
        var timeoutPolicy =
            Policy.TimeoutAsync(
                TimeSpan.FromSeconds(15),
                TimeoutStrategy.Pessimistic);

        var retryPolicy =
            Policy
                .Handle<TimeoutRejectedException>()
                .Or<Exception>() // we’ll narrow later
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, attempt))
                );

        return Policy.WrapAsync(retryPolicy, timeoutPolicy);
    }
}
