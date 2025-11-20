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
dotnet run --framework net10.0-windows -- natural "move this window to the other screen"
dotnet run --framework net10.0-windows -- natural "maximize this window"
dotnet run --framework net10.0-windows -- natural "put this window on the left half"
dotnet run --framework net10.0-windows -- natural "put this window on the right half"
```

## Window Management Actions
You can use natural language to control window position and size:

- Move to next monitor: `dotnet run --framework net10.0-windows -- natural "move this window to the other screen"`
- Maximize: `dotnet run --framework net10.0-windows -- natural "maximize this window"`
- Left half: `dotnet run --framework net10.0-windows -- natural "put this window on the left half"`
- Right half: `dotnet run --framework net10.0-windows -- natural "put this window on the right half"`

## Running Integration Tests
To run all integration tests (including window management):

```pwsh
dotnet test Tests/IntegrationTests.csproj
```

Tests will verify that window management, app launching, folder opening, and other actions work as expected.

## Notes
 