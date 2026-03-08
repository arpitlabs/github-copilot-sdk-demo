# GitHub Copilot SDK - Interactive Demo Application

A C# sample application demonstrating interactive messaging with GitHub Copilot using the official GitHub Copilot SDK v0.1.32. Send multiple questions to GPT-5 in an interactive chat session.

## Overview

This project showcases the GitHub Copilot SDK capabilities, including:
- ✓ Initializing a Copilot client connection
- ✓ Creating sessions with GPT-5 model selection
- ✓ Sending messages and receiving AI responses
- ✓ Interactive multi-message chat loop
- ✓ Real-time response streaming
- ✓ Comprehensive error handling

## Project Structure

```
github-copilot-sdk-demo/
├── Program.cs                    # Interactive chat application
├── copilot-demo.csproj          # .NET 10.0 project configuration
├── .gitignore                   # Git ignore rules for .NET projects
├── README.md                    # This file
├── MESSAGE_SENDING_GUIDE.md     # Detailed message sending guide
└── QUICKSTART.md                # Quick setup guide
```

## Prerequisites

Before running this application, ensure you have:

1. **GitHub Copilot CLI**
   - Install: https://github.com/github/copilot-cli
   - Authenticate: `copilot auth login`

2. **.NET SDK**
   - .NET 10.0 or later
   - Download from: https://dotnet.microsoft.com/download

3. **GitHub Account**
   - Authenticated and authorized with GitHub Copilot CLI
   - Access to GPT-5 model

## Installation & Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/github-copilot-sdk-demo.git
   cd github-copilot-sdk-demo
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the project**
   ```bash
   dotnet build
   ```

## Running the Application

Execute the sample application with:

```bash
dotnet run
```

### Interactive Usage

The application presents an interactive prompt where you can:
1. Enter your questions one at a time
2. Receive real-time AI responses from GPT-5
3. Continue asking multiple questions in the same session
4. Type `exit` or `quit` to end the session

### Expected Output

```
====================================================================
   GitHub Copilot SDK - Interactive Message Test Application       
====================================================================

HELP: Enter your questions below. Type 'exit' to quit the session.

[*] Creating GitHub Copilot client...
[*] Starting Copilot client connection...
[OK] Client started successfully

[CONFIG] Session Configuration:
--------------------------------------------------------------------
  Model: gpt-5
  Session ID: 8c069407
  Timestamp: 2026-03-07 20:26:26
--------------------------------------------------------------------

[*] Creating session with GPT-5 model...
[OK] Session created successfully

[INPUT] User Prompt:
--------------------------------------------------------------------
> What is GitHub Copilot?
--------------------------------------------------------------------
[RESPONSE] Copilot Response:
--------------------------------------------------------------------
GitHub Copilot is an AI-powered coding assistant built by GitHub that 
helps you write code faster and smarter. It uses machine learning trained 
on public repositories to:

- Auto-complete code as you type, suggesting entire functions or code blocks
- Explain code to help you understand what it does
- Generate tests and documentation
- Fix bugs and suggest improvements

I'm GitHub Copilot CLI—a terminal-based version designed to help with 
software engineering tasks directly from your command line.

How can I help you today?
--------------------------------------------------------------------
[STATS] Message Exchange Summary:
  Response length: 631 characters
--------------------------------------------------------------------

[INPUT] User Prompt:
--------------------------------------------------------------------
> exit
--------------------------------------------------------------------
[*] Exit command received. Closing session...

[*] Closing session...
[OK] Session closed successfully

[*] Stopping Copilot client...
[OK] Client stopped successfully

====================================================================
   Test Completed                                                  
====================================================================
```

## Code Example

### Basic Session Creation

```csharp
using GitHub.Copilot.SDK;

// Create client
await using var client = new CopilotClient();

// Start connection
await client.StartAsync();

// Create session with GPT-5
var sessionConfig = new SessionConfig
{
    Model = "gpt-5",
    OnPermissionRequest = PermissionHandler.ApproveAll
};

await using var session = await client.CreateSessionAsync(sessionConfig);

// Send message and handle response
var responseReceived = new TaskCompletionSource<bool>();

session.On(evt =>
{
    if (evt is AssistantMessageEvent msg)
    {
        Console.Write(msg.Data.Content);
    }
    else if (evt is SessionIdleEvent)
    {
        responseReceived.SetResult(true);
    }
});

await session.SendAsync(new MessageOptions { Prompt = "Your question here" });
await responseReceived.Task;

// Cleanup
await session.DisposeAsync();
await client.StopAsync();
```

## Dependencies

The project uses the following NuGet packages:

| Package | Version | Purpose |
|---------|---------|---------|
| GitHub.Copilot.SDK | 0.1.32 | Official GitHub Copilot SDK |
| Microsoft.Extensions.AI | 10.3.0 | AI extensions framework |

## Features

### Interactive Chat
- Continuous message loop until user exits
- Supports multiple questions in one session
- Real-time response streaming
- Character count reporting

### Model Selection
- GPT-5 (default in this demo)
- Easily switch to other models (Claude, etc.)
- Configured in `SessionConfig`

### Event Handling
- `AssistantMessageEvent` - Response content
- `SessionIdleEvent` - Response complete
- `SessionErrorEvent` - Error handling

### Resource Management
- Automatic cleanup with `await using`
- Graceful shutdown
- Timeout protection (30 seconds per response)

## Configuration

### SessionConfig Options

```csharp
var config = new SessionConfig
{
    Model = "gpt-5",                           // AI model
    OnPermissionRequest = PermissionHandler.ApproveAll,  // Permission handler (required)
    ReasoningEffort = "medium",                // "low", "medium", "high", "xhigh"
    SystemMessage = new SystemMessage { },     // Custom system prompt
    Tools = new List<Tool>()                   // Custom tools
};
```

## Error Handling

The application includes comprehensive error handling:

| Error | Cause | Solution |
|-------|-------|----------|
| "Connection refused" | Copilot CLI not running | Install and start Copilot CLI |
| "Authentication failed" | Not authenticated | Run `copilot auth login` |
| "Model not available" | Limited access | Check subscription |
| "Response timeout" | No response in 30s | Check network connection |

## Troubleshooting

### Application Won't Start

1. Verify GitHub Copilot CLI is installed:
   ```bash
   which copilot
   ```

2. Authenticate with GitHub:
   ```bash
   copilot auth login
   ```

3. Check authentication status:
   ```bash
   copilot auth status
   ```

### No Response Received

1. Verify internet connection
2. Check Copilot CLI is running properly
3. Try again with a simpler question
4. Review error messages in output

### Build Issues

- **Framework mismatch**: Ensure .NET 10.0+
  ```bash
  dotnet --version
  ```

- **Package issues**: Clear and restore
  ```bash
  dotnet nuget locals all --clear
  dotnet restore
  ```

## Advanced Usage

1. **Custom Models**: Change `Model` in `SessionConfig`
2. **Multiple Sessions**: Create separate sessions for different tasks
3. **Streaming Responses**: Process `AssistantMessageEvent` in real-time
4. **Error Recovery**: Implement retry logic with exponential backoff

## Additional Resources

- **MESSAGE_SENDING_GUIDE.md** - Detailed guide to message sending
- **QUICKSTART.md** - Fast setup instructions
- **Official Docs**: https://docs.github.com/en/copilot
- **GitHub Copilot CLI**: https://github.com/github/copilot-cli
- **SDK Repository**: https://github.com/github/copilot-sdk

## Project Status

| Item | Status |
|------|--------|
| Build | ✓ Passing |
| Documentation | ✓ Complete |
| Interactive Chat | ✓ Working |
| Multi-Message Loop | ✓ Working |
| Error Handling | ✓ Complete |

## Contributing

Contributions are welcome! To contribute:

1. Fork the repository
2. Create a feature branch
3. Make your improvements
4. Submit a pull request

## License

This project is provided as a sample for learning GitHub Copilot SDK integration.

## Support

For issues or questions:

1. Check the troubleshooting section above
2. Review MESSAGE_SENDING_GUIDE.md for detailed API info
3. Check Copilot CLI logs: `copilot logs`
4. Open an issue on the GitHub repository

---

**Last Updated**: March 8, 2026  
**SDK Version**: 0.1.32  
**.NET Version**: 10.0  
**Remote**: github-copilot-sdk-demo  
**Application Type**: Interactive Chat CLI
