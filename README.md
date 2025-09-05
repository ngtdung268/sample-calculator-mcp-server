# Sample Calculator MCP Server

## Introduction

This project is a sample codebase to learn about Microsoft's ModelContextProtocol (MCP) framework. It features a simple MCP Server that acts as a calculator tool, running on Stdio transport. The solution demonstrates how a basic MCP Server can be implemented and interacted with using an MCP Client.

## How to Run This Project

1. Clone this repository to your local machine.
2. Open the solution in Visual Studio IDE or other compatible IDEs (with .NET 9 support).
3. Before running, update the following constant values in `McpClient/Program.cs`:
   - `GithubToken`: Replace with your own GitHub token. Ensure you select "Copilot" as the scope when creating the token.
   - `YourPrompt`: Set your desired prompt text.
4. Build the solution.
5. Start the MCP Client project and observe the console output.

## Sample Output

```
Using LLM model: gpt-4o-mini, from: https://models.inference.ai.azure.com

Tools found: 4
1. multiply - Multiply two numbers: {"type":"object","properties":{"a":{"type":"integer"},"b":{"type":"integer"}},"required":["a","b"]}
2. divide - Divide two numbers: {"type":"object","properties":{"a":{"type":"integer"},"b":{"type":"integer"}},"required":["a","b"]}
3. add - Add two numbers: {"type":"object","properties":{"a":{"type":"integer"},"b":{"type":"integer"}},"required":["a","b"]}
4. subtract - Subtract two numbers: {"type":"object","properties":{"a":{"type":"integer"},"b":{"type":"integer"}},"required":["a","b"]}

Tool call: add with arguments {"a":22,"b":11}
Tool call result: 33

LLM response: {
  "finishReason": {},
  "role": {},
  "content": null,
  "toolCalls": [
    {
      "name": "add",
      "arguments": "{\u0022a\u0022:22,\u0022b\u0022:11}",
      "id": "call_G1ZMEX8PzMjl8FtPSI77iKAQ",
      "type": {},
      "function": {
        "name": "add",
        "arguments": "{\u0022a\u0022:22,\u0022b\u0022:11}"
      }
    }
  ],
  "id": "chatcmpl-CCFtVfe3GtXyinBiZ7aRRFnb8SCAp",
  "created": "2025-09-05T01:51:09+00:00",
  "model": "gpt-4o-mini-2024-07-18",
  "usage": {
    "completionTokens": 18,
    "promptTokens": 134,
    "totalTokens": 152
  }
}

Tool call: multiply with arguments {"a":33,"b":33}
Tool call result: 1089

LLM response: {
  "finishReason": {},
  "role": {},
  "content": null,
  "toolCalls": [
    {
      "name": "multiply",
      "arguments": "{\u0022a\u0022:33,\u0022b\u0022:33}",
      "id": "call_l78o9qwJulNGUXlhiKw3cevR",
      "type": {},
      "function": {
        "name": "multiply",
        "arguments": "{\u0022a\u0022:33,\u0022b\u0022:33}"
      }
    }
  ],
  "id": "chatcmpl-CCFtWacduihtGToAGLDll4WfcwSQ4",
  "created": "2025-09-05T01:51:10+00:00",
  "model": "gpt-4o-mini-2024-07-18",
  "usage": {
    "completionTokens": 18,
    "promptTokens": 159,
    "totalTokens": 177
  }
}

Final content: The result of adding 22 and 11 is 33, and when you multiply that by 33, the final result is 1089.
```

## Further Reading

For a step-by-step tutorial and more information about Microsoft's ModelContextProtocol framework, please visit:

[https://github.com/microsoft/mcp-for-beginners](https://github.com/microsoft/mcp-for-beginners)
