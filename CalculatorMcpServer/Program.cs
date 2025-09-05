using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConsole(options =>
{
    options.LogToStandardErrorThreshold = LogLevel.Trace;
});

builder.Services
    .AddMcpServer(options =>
    {
        options.ServerInfo = new ModelContextProtocol.Protocol.Implementation
        {
            Name = "mcp-calculator-server",
            Title = "Calculator Server",
            Version = "1.0.0",
        };
    })
    // Register the server to use stdio transport.
    .WithStdioServerTransport()
    // Register all the tools in this assembly.
    .WithToolsFromAssembly();

await builder.Build().RunAsync();