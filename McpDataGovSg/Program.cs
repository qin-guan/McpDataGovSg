using McpDataGovSg.HttpServices;
using McpDataGovSg.Services;
using McpDataGovSg.Tools;
using NeoSmart.Caching.Sqlite;
using ZiggyCreatures.Caching.Fusion;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddFusionCache()
        .WithCysharpMemoryPackSerializer()
        .WithDistributedCache(
            new SqliteCache(new SqliteCacheOptions
            {
                CachePath = "./cache.db"
            })
        );
}
else
{
    builder.Services.AddFusionCache();
}

builder.Services.AddSingleton<DgsHttpService>();
builder.Services.AddSingleton<DgsSearchService>();

builder.Services
    .AddMcpServer()
    .WithHttpTransport()
    .WithTools<DatasetQueryTool>();

builder.Services.AddOpenApi();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var service = scope.ServiceProvider.GetRequiredService<DgsHttpService>();
    await service.ListAllDatasetsAsync();
}

app.UseHttpsRedirection();

app.MapOpenApi();
app.MapMcp();

app.Run();