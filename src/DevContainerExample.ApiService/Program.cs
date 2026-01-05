
using Microsoft.Extensions.AI;
var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IChatClient>(new OllamaChatClient(
    new Uri("http://localhost:11434"),
    "qwen3:1.7b"
));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/", () => "API service is running. Navigate to /chat to see sample data.");

app.MapGet("/chat", async (IChatClient chatClient) =>
{
    var chatResponse = await chatClient.GetResponseAsync("Hello, how are you?");

    return TypedResults.Ok(chatResponse);
})
.WithName("Chat");

app.MapDefaultEndpoints();

app.Run();
