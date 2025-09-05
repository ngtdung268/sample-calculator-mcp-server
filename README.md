# Sample Calculator MCP Server

## Introduction

This project is a sample codebase to learn about Microsoft's ModelContextProtocol (MCP) framework. It features a simple MCP Server that acts as a calculator tool, running on Stdio transport. The solution demonstrates how a basic MCP Server can be implemented and interacted with using an MCP Client.

## How to Run This Project

1. Clone this repository to your local machine.
2. Open the solution in Visual Studio IDE or other compatible IDE (with .NET 9 support).
3. Before running, update the following constant values in `McpClient/Program.cs`:
   - `GithubToken`: Replace with your own GitHub token. Ensure you select "Copilot" as the scope when creating the token.
   - `YourPrompt`: Set your desired prompt text.
4. Build the solution.
5. Start the MCP Client project and observe the console output.

## Further Reading

For a step-by-step tutorial and more information about Microsoft's ModelContextProtocol framework, please visit:

[https://github.com/microsoft/mcp-for-beginners](https://github.com/microsoft/mcp-for-beginners)
