using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using AiKnowledgeAssistant.Api;
using Microsoft.Extensions.DependencyInjection;

namespace AiKnowledgeAssistant.Tests
{
    public static class TestHost
    {
        private static readonly IHost _host;

        static TestHost()
        {
            _host = Build();
        }
        private static IHost Build()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.json", optional: false);
                })
                .ConfigureServices((context, services) =>
                {
                    // 🔹 reuse Program.cs registrations
                        CompositionRoot.ConfigureServices(
                        services,
                        context.Configuration);
                })
                .Build();
        }

        public static T GetService<T>()
        {
            return _host.Services.GetRequiredService<T>();
        }
        public static void Dispose() => _host.Dispose();
    }

}
