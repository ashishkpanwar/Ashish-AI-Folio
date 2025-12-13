using AiKnowledgeAssistant.Application.AI;
using AiKnowledgeAssistant.Infrastructure.AI;
using Azure;
using Azure.AI.OpenAI;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(sp =>
{
    var endpoint = new Uri(builder.Configuration["AzureOpenAI:Endpoint"]!);
    var key = builder.Configuration["AzureOpenAI:ApiKey"]!;
    return new AzureOpenAIClient(endpoint, new AzureKeyCredential(key));
});


builder.Services.AddSingleton<IAiClient>(sp =>
{
    var azureClient = sp.GetRequiredService<AzureOpenAIClient>();
    var deployment = builder.Configuration["AzureOpenAI:ChatDeployment"]!;
    return new AzureOpenAiClient(azureClient,deployment);
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ashish AI Folio API",
        Version = "v1"
    });
});

var app = builder.Build();
app.UseDeveloperExceptionPage();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();