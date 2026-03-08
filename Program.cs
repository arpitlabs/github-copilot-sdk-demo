using System.Text;
using GitHub.Copilot.SDK;

Console.WriteLine("====================================================================");
Console.WriteLine("   GitHub Copilot SDK - Interactive Message Test Application       ");
Console.WriteLine("====================================================================\n");

Console.WriteLine("HELP: Enter your questions below. Type 'exit' to quit the session.\n");

try
{
    // Step 1: Create and start client
    Console.WriteLine("[*] Creating GitHub Copilot client...");
    await using var client = new CopilotClient();
    
    Console.WriteLine("[*] Starting Copilot client connection...");
    await client.StartAsync();
    Console.WriteLine("[OK] Client started successfully\n");

    // Step 2: Create session with GPT-5 model
    Console.WriteLine("[CONFIG] Session Configuration:");
    Console.WriteLine("--------------------------------------------------------------------");
    Console.WriteLine("  Model: gpt-5");
    Console.WriteLine("  Session ID: {0}", Guid.NewGuid().ToString().Substring(0, 8));
    Console.WriteLine("  Timestamp: {0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
    Console.WriteLine("--------------------------------------------------------------------\n");

    // Create session with GPT-5 model
    Console.WriteLine("[*] Creating session with GPT-5 model...");
    var sessionConfig = new SessionConfig
    {
        Model = "gpt-5",
        OnPermissionRequest = PermissionHandler.ApproveAll
    };
    
    await using var session = await client.CreateSessionAsync(sessionConfig);
    Console.WriteLine("[OK] Session created successfully\n");

    // Step 3: Interactive message loop
    bool continueChat = true;
    while (continueChat)
    {
        Console.WriteLine("[INPUT] User Prompt:");
        Console.WriteLine("--------------------------------------------------------------------");
        Console.Write("> ");
        string userMessage = Console.ReadLine() ?? string.Empty;
        
        // Check for exit command
        if (userMessage.Equals("exit", StringComparison.OrdinalIgnoreCase) || 
            userMessage.Equals("quit", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("--------------------------------------------------------------------");
            Console.WriteLine("[*] Exit command received. Closing session...\n");
            continueChat = false;
            break;
        }

        // Skip empty messages
        if (string.IsNullOrWhiteSpace(userMessage))
        {
            Console.WriteLine("--------------------------------------------------------------------");
            Console.WriteLine("[!] Please enter a valid question or type 'exit' to quit.\n");
            continue;
        }

        Console.WriteLine("--------------------------------------------------------------------");
        Console.WriteLine("[RESPONSE] Copilot Response:");
        Console.WriteLine("--------------------------------------------------------------------");
        
        var responseBuffer = new StringBuilder();
        var responseReceived = new TaskCompletionSource<bool>();
        var handlerRegistered = false;
        
        // Register event handler only once per message
        session.On(evt =>
        {
            if (evt is AssistantMessageEvent msg)
            {
                // Print response in real-time
                Console.Write(msg.Data.Content);
                responseBuffer.Append(msg.Data.Content);
            }
            else if (evt is SessionIdleEvent)
            {
                // Session is idle, meaning response is complete
                if (!handlerRegistered)
                {
                    handlerRegistered = true;
                    responseReceived.TrySetResult(true);
                }
            }
            else if (evt is SessionErrorEvent errorEvent)
            {
                Console.WriteLine("\n[ERROR] Session error: {0}", errorEvent.Data?.Message);
                responseReceived.TrySetException(new Exception(errorEvent.Data?.Message ?? "Unknown error"));
            }
        });

        // Send the message
        await session.SendAsync(new MessageOptions 
        { 
            Prompt = userMessage 
        });

        // Wait for response to complete with timeout
        try
        {
            var timeoutTask = Task.Delay(30000); // 30 second timeout
            var completedTask = await Task.WhenAny(responseReceived.Task, timeoutTask);
            
            if (completedTask == timeoutTask)
            {
                Console.WriteLine("\n[ERROR] Response timeout - no response received within 30 seconds");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n[ERROR] Error waiting for response: {0}", ex.Message);
        }
        
        Console.WriteLine("\n--------------------------------------------------------------------");
        Console.WriteLine("[STATS] Message Exchange Summary:");
        Console.WriteLine("  Response length: {0} characters", responseBuffer.Length);
        Console.WriteLine("--------------------------------------------------------------------\n");
    }

    // Step 4: Close session
    Console.WriteLine("[*] Closing session...");
    await session.DisposeAsync();
    Console.WriteLine("[OK] Session closed successfully\n");

    // Step 5: Stop client
    Console.WriteLine("[*] Stopping Copilot client...");
    await client.StopAsync();
    Console.WriteLine("[OK] Client stopped successfully\n");
}
catch (Exception ex)
{
    Console.WriteLine("\n[ERROR] Error: {0}", ex.Message);
    Console.WriteLine("\nTROUBLESHOOTING:");
    Console.WriteLine("  1. Ensure GitHub Copilot CLI is installed on your system");
    Console.WriteLine("  2. Verify GitHub authentication is configured");
    Console.WriteLine("  3. Check that you have access to GPT-5 model");
    Console.WriteLine("  4. Ensure internet connection is available");
    Console.WriteLine("  5. Review the error message above for details\n");
    
    if (ex.InnerException != null)
    {
        Console.WriteLine("  Inner Error: {0}", ex.InnerException.Message);
    }
}

Console.WriteLine("====================================================================");
Console.WriteLine("   Test Completed                                                  ");
Console.WriteLine("====================================================================");
