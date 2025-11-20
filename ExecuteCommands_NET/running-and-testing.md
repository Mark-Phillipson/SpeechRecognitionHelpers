## Build the Application
```pwsh
# Build the main project (requires .NET 10)
dotnet build --framework net10.0-windows
dotnet clean --framework net10.0-windows

dotnet build --framework net10.0-windows --configuration Release

## Example Natural Language Actions
```pwsh
# Run with a specific action (replace with your desired command)
dotnet run --framework net10.0-windows -- natural "open calculator"
dotnet run --framework net10.0-windows -- natural "type hello world"
dotnet run --framework net10.0-windows -- natural "open notepad"
```

## Notes
- All commands must be run from the `ExecuteCommands_NET` directory.
- The application only supports .NET 10.0 (Windows).
- Natural language actions are passed after `-- natural "<your action>"`.
- Test code is located in the `Tests/` directory and excluded from main builds.
 