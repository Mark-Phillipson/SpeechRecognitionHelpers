# Copilot Instructions for SpeechRecognitionHelpers/ExecuteCommands_NET

## Project Overview
This is a Windows desktop automation and speech recognition helper application built with .NET 10, using Windows Forms and WPF. The codebase is organized for extensibility and integration with external data sources and automation libraries.

## Architecture & Major Components
- **Main Logic**: Core automation and command execution logic is in files like `Commands.cs`, `CustomMethods.cs`, `NaturalActions.cs`, and `NaturalLanguageInterpreter.cs`.
- **Helpers**: Utility classes for parsing, notifications, and cursor/mouse handling are in `Helpers/` and files like `TrayNotificationHelper.cs`, `WinCursors.cs`, and `ShowMouse.cs`.
- **Models**: Data models are in `Models/`, including context and cheatsheet items for voice commands.
- **Repositories**: External data access and integration logic is in `Repositories/`.
- **UI**: Windows Forms UI components are in `DisplayMessage.cs`, `DisplayMessage.Designer.cs`, and related resource files.
- **Tests**: All test code is in `Tests/` and excluded from main builds via csproj rules.

## Build & Developer Workflow
- **Build**: Use `dotnet build --framework net10.0-windows` from the project root. Only .NET 10 is supported.
- **Test**: Test projects are in `Tests/` and must target .NET 10.0. Build with `dotnet build Tests/ExecuteCommands_NET.Tests.csproj --framework net10.0-windows`.
- **Debug**: Main entry point is `Program.cs`. UI debugging uses Windows Forms/WPF tools.
- **Exclude Tests**: The main project excludes all files in `Tests/` via `<Compile Remove="Tests\**" />` in the csproj.

## Patterns & Conventions
- **External Models**: Many models are linked from outside the repo (see csproj `Compile Include` entries). Always reference via the `Models/` link path.
- **Implicit Usings**: Enabled via `<ImplicitUsings>enable</ImplicitUsings>`.
- **Nullable**: Nullable reference types are enabled.
- **Resource Management**: UI resources are managed via `.Designer.cs` and `.resx` files, with `<DependentUpon>` relationships in the csproj.
- **Automation**: Command execution and process handling are abstracted in `HandleProcesses.cs`, `IHandleProcesses.cs`, and related helpers.

## Integration Points
- **Azure/OpenAI**: Uses packages like `Azure.AI.OpenAI` and `OpenAI` for AI integration.
- **Entity Framework**: Uses `Microsoft.EntityFrameworkCore.SqlServer` for database access.
- **Input Simulation**: Uses `InputSimulatorCore` for keyboard/mouse automation.
- **Interop/UI Automation**: Uses `Interop.UIAutomationClient` for Windows UI automation.

## Examples
- To add a new automation command, extend `Commands.cs` and update `NaturalLanguageInterpreter.cs` for parsing.
- To add a new model, link it in the csproj under the `Models/` path, even if the source is external.
- To add a new helper, place it in `Helpers/` and follow the utility class pattern.

## Key Files & Directories
- `Commands.cs`, `CustomMethods.cs`, `NaturalLanguageInterpreter.cs`: Core logic
- `Helpers/`: Utility classes
- `Models/`: Data models (linked)
- `Repositories/`: Data access/integration
- `DisplayMessage.cs`, `.Designer.cs`, `.resx`: UI components
- `Tests/`: Test code (excluded from main build)

---
_If any section is unclear or missing important project-specific knowledge, please provide feedback to improve these instructions._
