var builder = DistributedApplication.CreateBuilder(args);

var ollama = builder.AddOllama("ollama", port: 11434)
    // .WithOpenWebUI()
    .WithDataVolume();
    
 var qwen3 = ollama.AddModel("qwen3:1.7b");

var apiService = builder.AddProject<Projects.DevContainerExample_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithReference(qwen3)
    .WaitFor(qwen3);

builder.AddProject<Projects.DevContainerExample_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();