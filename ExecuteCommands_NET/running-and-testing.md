# Running and Testing

This guide provides commands for building, running, and testing the SpeechRecognitionHelpers application using .NET 10. It also shows how to supply natural language actions via the command line.

## Build the Application
```pwsh
# Build the main project (requires .NET 10)
dotnet build --framework net10.0-windows
```

## Run the Application
```pwsh
# Run the application (default behavior)
dotnet run --framework net10.0-windows

# Run with a natural language command
dotnet run --framework net10.0-windows -- natural "open notepad"
```

## Test the Application
```pwsh
# Build and run the test project
dotnet build Tests/ExecuteCommands_NET.Tests.csproj --framework net10.0-windows

dotnet test Tests/ExecuteCommands_NET.Tests.csproj --framework net10.0-windows
```

## Example Natural Language Actions
```pwsh
# Run with a specific action (replace with your desired command)
dotnet run --framework net10.0-windows -- natural "clothed"
dotnet run --framework net10.0-windows -- natural "open calculator"
dotnet run --framework net10.0-windows -- natural "type hello world"
```

## Notes
- All commands must be run from the `ExecuteCommands_NET` directory.
- The application only supports .NET 10.0 (Windows).
- Natural language actions are passed after `-- natural "<your action>"`.
- Test code is located in the `Tests/` directory and excluded from main builds.
