using System.ComponentModel;
using ModelContextProtocol.Server;

namespace CalcMcpServer
{
    [McpServerToolType]
    public static class CalculatorTool
    {
        [McpServerTool, Description("Add two numbers")]
        public static int Add(int a, int b)
        {
            return a + b;
        }

        [McpServerTool, Description("Subtract two numbers")]
        public static int Subtract(int a, int b)
        {
            return a - b;
        }

        [McpServerTool, Description("Multiply two numbers")]
        public static int Multiply(int a, int b)
        {
            return a * b;
        }

        [McpServerTool, Description("Divide two numbers")]
        public static double Divide(int a, int b)
        {
            if (b == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero.");
            }
            return (double)a / b;
        }
    }
}