using AiKnowledgeAssistant.Application.AI;
using AiKnowledgeAssistant.Application.Search;
using AiKnowledgeAssistant.Infrastructure.AI;
using AiKnowledgeAssistant.Infrastructure.Search;
using Azure;
using Azure.AI.OpenAI;
using Azure.Search.Documents;


var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders().AddConsole().AddDebug();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(sp =>
{
    var endpoint = new Uri(builder.Configuration["AzureOpenAI:Endpoint"]!);
    var key = builder.Configuration["AzureOpenAI:ApiKey"]!;
    return new AzureOpenAIClient(endpoint, new AzureKeyCredential(key));
});

builder.Services.AddSingleton(sp =>
{
    var azureClient = sp.GetRequiredService<AzureOpenAIClient>();
    var deployment = builder.Configuration["AzureOpenAI:EmbeddingDeployment"]!;
    return azureClient.GetEmbeddingClient(deployment);
});

builder.Services.AddSingleton(sp =>
{
    var endpoint = new Uri(builder.Configuration["AzureSearchService:Endpoint"]!);
    var index = builder.Configuration["AzureSearchService:IndexName"]!;
    var apiKey = builder.Configuration["AzureSearchService:ApiKey"]!;

    return new SearchClient(endpoint,index, new AzureKeyCredential(apiKey));
});

builder.Services.AddSingleton(sp =>
{
    var azureClient = sp.GetRequiredService<AzureOpenAIClient>();
    var chatDeployment = builder.Configuration["AzureOpenAI:ChatDeployment"]!;
    return azureClient.GetChatClient(chatDeployment);
});


builder.Services.AddSingleton<IAiClient, AzureOpenAiClient>();
builder.Services.AddSingleton<IAiEmbeddingClient, AzureOpenAiEmbeddingClient>();
builder.Services.AddSingleton<IVectorSearchStore, AzureVectorSearchStore>();




var app = builder.Build();
app.UseDeveloperExceptionPage();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();