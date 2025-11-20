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

## Visual Studio Natural Language Command Tests

You can run these commands from the integrated terminal inside Visual Studio:

| Natural Language Command         | Expected Visual Studio Action         | Canonical Command Name      |
|----------------------------------|--------------------------------------|----------------------------|
| build the solution               | Build the entire solution            | Build.BuildSolution        |
| build the project                | Build the current project            | Build.BuildProject         |
| start debugging                  | Start debugging the startup project  | Debug.Start                |
| start application                | Start without debugging              | Debug.StartWithoutDebugging|
| stop debugging                   | Stop debugging                       | Debug.StopDebugging        |
| close tab                        | Close the current document tab       | Window.CloseDocumentWindow |
| format document                  | Format the current document          | Edit.FormatDocument        |
| find in files                    | Open the Find in Files dialog        | Edit.FindinFiles           |
| go to definition                 | Go to definition of symbol           | Edit.GoToDefinition        |
| rename symbol                    | Rename the selected symbol           | Refactor.Rename            |
| show solution explorer           | Focus Solution Explorer              | View.SolutionExplorer      |
| open recent files                | Show recent files                    | File.RecentFiles           |

Example usage:
```pwsh
dotnet run --framework net10.0-windows -- natural "build the solution"
dotnet run --framework net10.0-windows -- natural "start debugging"
dotnet run --framework net10.0-windows -- natural "close tab"
dotnet run --framework net10.0-windows -- natural "format document"
```

Check your application log (`app.log`) for debug output and confirmation of command execution.
 