# GitHub Copilot SDK - Sample Application

A C# sample application demonstrating how to connect to GitHub Copilot using the official GitHub Copilot SDK and send test messages.

## Overview

This project showcases the GitHub Copilot SDK capabilities, including:
- ✅ Initializing a Copilot client connection
- ✅ Managing client lifecycle (start/stop)
- ✅ Handling messages and responses
- ✅ Error handling and troubleshooting
- ✅ Integration with Microsoft.Extensions.AI

## Project Structure

```
copilot-demo/
├── Program.cs              # Main application entry point
├── copilot-demo.csproj     # Project configuration
└── README.md               # This file
```

## Prerequisites

Before running this application, ensure you have:

1. **GitHub Copilot Installed**
   - Install GitHub Copilot from your IDE (VS Code, Visual Studio, JetBrains IDEs)
   - Available at: https://github.com/features/copilot

2. **.NET SDK**
   - .NET 10.0 or later
   - Download from: https://dotnet.microsoft.com/download

3. **GitHub Authentication**
   - Authenticated GitHub account
   - Copilot subscription or free trial

## Installation & Setup

1. **Clone or navigate to the project directory**
   ```bash
   cd copilot-demo
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

### Expected Output

```
╔════════════════════════════════════════════════════════════╗
║   GitHub Copilot SDK - Test Application                   ║
╚════════════════════════════════════════════════════════════╝

📋 Creating GitHub Copilot client...
🔌 Starting Copilot client connection...
✅ Client started successfully

📊 Session Information:
───────────────────────────────────────────────────────────
  Client Status: Connected
  Timestamp: 2026-03-08 01:59:06
───────────────────────────────────────────────────────────

💬 Test Message:
───────────────────────────────────────────────────────────
  User: "Hello! Can you briefly explain what GitHub Copilot is?"

🤖 Copilot Status: Ready to receive and send messages
   - Connection: Established ✓
   - Authentication: Configured ✓
   - Session: Active ✓

💾 Stopping Copilot client connection...
✅ Client stopped successfully

[Test Summary...]
```

## Code Example

```csharp
using GitHub.Copilot.SDK;

// Create a new Copilot client
var client = new CopilotClient();

// Start the connection
await client.StartAsync();

// Client is now ready to send and receive messages
// ... perform operations ...

// Close the connection
await client.StopAsync();
```

## Dependencies

The project uses the following NuGet packages:

| Package | Version | Purpose |
|---------|---------|---------|
| GitHub.Copilot.SDK | 0.1.32 | Official GitHub Copilot SDK |
| Microsoft.Extensions.AI | 10.3.0 | AI extensions framework |

## Architecture

### CopilotClient Class

The main entry point for interacting with GitHub Copilot:

```
CopilotClient
├── StartAsync()      - Establish connection
├── StopAsync()       - Close connection
└── ForceStopAsync()  - Force shutdown
```

## Error Handling

The application includes comprehensive error handling:

- **Connection Errors**: Catches and displays connection failures
- **Authentication Errors**: Identifies auth-related issues
- **Service Errors**: Handles Copilot service unavailability

Common error scenarios and solutions:

| Error | Solution |
|-------|----------|
| "Connection refused" | Ensure GitHub Copilot is installed and running |
| "Authentication failed" | Verify GitHub credentials and Copilot subscription |
| "Service unavailable" | Check your internet connection and Copilot service status |

## Troubleshooting

### GitHub Copilot Not Connected

1. **Install GitHub Copilot**
   - Open your IDE (VS Code, Visual Studio, etc.)
   - Navigate to Extensions/Add-ons
   - Search for "GitHub Copilot"
   - Install the official extension

2. **Authenticate**
   - Open the IDE
   - Sign in with your GitHub account
   - Authorize GitHub Copilot

3. **Verify Installation**
   - Check that Copilot is running in your IDE
   - Look for the Copilot indicator in your editor status bar

### Build Errors

- **Framework mismatch**: Ensure you have .NET 10.0 or later installed
  ```bash
  dotnet --version
  ```

- **Package restore issues**: Clear NuGet cache and restore
  ```bash
  dotnet nuget locals all --clear
  dotnet restore
  ```

## Advanced Usage

For more advanced scenarios, you can:

1. **Custom Message Handling**: Extend the `CopilotClient` to process specific message types
2. **Session Management**: Implement session persistence and recovery
3. **Integration**: Embed Copilot functionality into larger applications

## API Reference

### CopilotClient Methods

- `StartAsync()` - Initiates connection to GitHub Copilot
- `StopAsync()` - Gracefully closes the connection
- `ForceStopAsync()` - Forces immediate shutdown

### Events

The client exposes various events for lifecycle management:
- Session events
- Model change events
- Error events
- Status change events

## Contributing

This is a sample application for educational purposes. To contribute improvements:

1. Fork the repository
2. Create a feature branch
3. Make your improvements
4. Submit a pull request

## License

This project is provided as a sample for learning GitHub Copilot SDK integration.

## Resources

- **Official Documentation**: https://docs.github.com/en/copilot
- **GitHub Copilot**: https://github.com/features/copilot
- **SDK Repository**: https://github.com/github/copilot-sdk
- **.NET Documentation**: https://docs.microsoft.com/en-us/dotnet/

## Support

For issues or questions:

1. Check the troubleshooting section above
2. Review GitHub Copilot documentation
3. Open an issue on the repository
4. Contact GitHub Support

---

**Last Updated**: March 8, 2026  
**SDK Version**: 0.1.32  
**.NET Version**: 10.0
