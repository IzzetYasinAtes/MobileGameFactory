using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MobileGameFactory.Mcp.Storage;

// MobileGameFactory MCP server.
// stdio transport uzerinden calisir. Claude Code .mcp.json icinden baslatir.
// Tum agent iletisimi, task state ve log kayitlari SQLite'a (ops/factory.db) gider.
// Dosyaya mesaj yazimi yok -> token tasarrufu.

var builder = Host.CreateApplicationBuilder(args);

// stdio transport calisirken stdout'u JSON-RPC icin rezerv tutmak sart.
// Tum loglar stderr'e.
builder.Logging.ClearProviders();
builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace);
builder.Logging.SetMinimumLevel(LogLevel.Information);

var repoRoot = FindRepoRoot(AppContext.BaseDirectory) ?? Directory.GetCurrentDirectory();
var dbPath = Environment.GetEnvironmentVariable("MGF_DB")
             ?? Path.Combine(repoRoot, "ops", "factory.db");

builder.Services.AddSingleton(_ => FactoryDb.Open(dbPath));

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

await builder.Build().RunAsync();

static string? FindRepoRoot(string start)
{
    var dir = new DirectoryInfo(start);
    while (dir is not null)
    {
        if (Directory.Exists(Path.Combine(dir.FullName, ".git"))) return dir.FullName;
        dir = dir.Parent;
    }
    return null;
}
