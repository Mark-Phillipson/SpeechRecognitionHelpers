Feature request: Add emoji support for voice commands

Goal

Add the ability to associate a specific emoji with a voice command phrase and display that emoji in the "what can I say" form / help UI. This makes the available-commands list more scannable and gives quick visual cues (for example: 📥 open downloads, 🖥️ maximize window).

Implementation notes

- Provide a runtime mapping API: `NaturalLanguageInterpreter.SetCommandEmoji(string command, string? emoji)` to add or remove an emoji for a command.
- Show configured emojis in `ShowAvailableCommands` next to the command text.
- Default emoji mappings can be seeded in code; callers can override or clear them at runtime.

Example

- `NaturalLanguageInterpreter.SetCommandEmoji("open downloads", "📥")` — sets the Downloads command emoji.
- `NaturalLanguageInterpreter.SetCommandEmoji("open downloads", null)` — clears the emoji.

Please implement the mapping, expose the setter API, and update the help display to include emojis. Optionally, consider persisting emoji mappings to disk in a follow-up change.