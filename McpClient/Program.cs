using Azure.AI.Inference;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using System.Text.Json;

//////////////////////////////////////////////////

// We'll use the LLM hosted on Azure.
const string AzAiEndpoint = "https://models.inference.ai.azure.com";

// https://github.com/microsoft/mcp-for-beginners/tree/main/03-GettingStarted/03-llm-client#authentication-using-github-personal-access-token
// Ensure you select "Copilot" as the scope when creating the token.
const string GithubToken = "input-your-token-here";

const string LlmModel = "gpt-4o-mini";

Console.WriteLine($"Using LLM model: {LlmModel}, from: {AzAiEndpoint}");
Console.WriteLine();

const string YourPrompt = "Please add 22 and 11, then multiple the result by 33.";

var jsonSerializerOptions = new JsonSerializerOptions()
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true,
};

//////////////////////////////////////////////////

// Get the tools from MCP server.
async Task<List<ChatCompletionsToolDefinition>> GetTools(IMcpClient client)
{
    var tools = await client.ListToolsAsync();

    Console.WriteLine($"Tools found: {tools.Count}");

    for (var i = 0; i < tools.Count; i++)
    {
        var tool = tools[i];
        Console.WriteLine($"{i + 1}. {tool.Name} - {tool.Description}: {tool.JsonSchema}");
    }

    Console.WriteLine();

    return [.. tools.Select(ToChatCompletionsTool)];
}

// Convert the MCP tool to the datatype that LLM can understand.
ChatCompletionsToolDefinition ToChatCompletionsTool(McpClientTool tool)
{
    tool.JsonSchema.TryGetProperty("properties", out JsonElement properties);

    return new ChatCompletionsToolDefinition(new FunctionDefinition(tool.Name)
    {
        Description = tool.Description,
        Parameters = BinaryData.FromObjectAsJson(
            new
            {
                Type = "object",
                Properties = properties
            },
            jsonSerializerOptions)
    });
}

// Create the options for the LLM request.
ChatCompletionsOptions CreateLlmOptions(List<ChatRequestMessage> chatHistory, List<ChatCompletionsToolDefinition> tools)
{
    var options = new ChatCompletionsOptions(chatHistory)
    {
        Model = LlmModel,
    };

    tools.ForEach(options.Tools.Add);

    return options;
}

//////////////////////////////////////////////////

// Create a client that connects to the MCP server via stdio protocol.
// It will start the server process if it is not running, then connect.
await using var mcpClient = await McpClientFactory.CreateAsync(
    new StdioClientTransport(new()
    {
        Name = "mcp-calc-server",
        Command = "dotnet",
        Arguments = [".\\CalculatorMcpServer.dll"]
    }));

// Get the list of tools from the MCP server.
var mcpTools = await GetTools(mcpClient);

// Create a client that connects to the LLM service.
var llmClient = new ChatCompletionsClient(new Uri(AzAiEndpoint), new Azure.AzureKeyCredential(GithubToken));

// Prepare the chat history and options for the LLM request.
var chatHistory = new List<ChatRequestMessage>
{
    new ChatRequestSystemMessage("You are a helpful assistant that knows about AI."),
    new ChatRequestUserMessage(YourPrompt),
};

// Tool call loop for chained calls.
while (true)
{
    // Get the chat completions from the LLM service.
    var llmOptions = CreateLlmOptions(chatHistory, mcpTools);
    var llmResponse = await llmClient.CompleteAsync(llmOptions);

    // If there's no tool call, print the final content and break the loop.
    if (llmResponse.Value.ToolCalls == null || !llmResponse.Value.ToolCalls.Any())
    {
        Console.WriteLine($"Final content: {llmResponse.Value.Content}");
        break;
    }

    // Check if the LLM response requires tool calls.
    if (llmResponse.Value.ToolCalls.Any())
    {
        // The subsequent tool calls must be preceded by the LLM response content that requires the tool calls.
        chatHistory.Add(new ChatRequestAssistantMessage(llmResponse.Value));

        foreach (var call in llmResponse.Value.ToolCalls)
        {
            Console.WriteLine($"Tool call: {call.Name} with arguments {call.Arguments}");

            // Call the tool with the arguments provided by the LLM.
            var callArgs = JsonSerializer.Deserialize<Dictionary<string, object>>(call.Arguments);
            var callResult = await mcpClient.CallToolAsync(call.Name, callArgs!);

            // Collect the tool call result.
            var resultText = default(string);
            foreach (var content in callResult.Content)
            {
                if (content is TextContentBlock textContent)
                {
                    resultText = textContent.Text;
                    Console.WriteLine($"Tool call result: {resultText}");
                }
                else
                {
                    throw new NotImplementedException();
                }

                // Feed the tool-call result back to the chat history.
                chatHistory.Add(new ChatRequestToolMessage(resultText, call.Id));
            }

            Console.WriteLine();
        }
    }

    Console.WriteLine($"LLM response: {JsonSerializer.Serialize(llmResponse.Value, jsonSerializerOptions)}");
    Console.WriteLine();
}

Console.ReadKey();