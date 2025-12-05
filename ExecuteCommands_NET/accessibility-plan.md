# Accessibility Voice Reliability Plan

## Focus
- Use UI Automation to make the natural-language interface more reliable, not to narrate every control. We care about identifying actionable targets, verifying favorites (build + run commands), and surfacing high-confidence alternatives when recognitions fail.

## Goals
1. **Favorites verification** — Keep tracking frequently used Visual Studio actions (Build Solution, Start/Stop Debugging) and mark them as verified whenever the automation tree exposes their accessible names. Highlight those favorites inside `ShowAvailableCommands` and reuse the verification list when suggesting accessible alternatives.
2. **Accessible suggestions** — When a spoken command has no rule-based match, query the automation tree for visible/enabled controls (buttons/menu/list items) near the VS window, filter duplicates, and present them with numbers so the user can say "choose 1" (or similar) to run a known UI target.
3. **Command selection flow** — Store the latest accessibility suggestions so `choose <number>` can convert the spoken selection back into a canonical action (when the automation name maps to a known command), otherwise show the accessible name for manual usage.

## Implementation Sketch
1. Extend `AccessibilityHelper` to expose:
   - the VS automation root
   - a reusable lookup-by-accessible-name routine
   - a filtered enumerator that returns visible/enabled controls of `ControlType.Button`, `MenuItem`, or `ListItem`.
2. In `NaturalLanguageInterpreter`:
   - keep a `VisualStudioCommandDisplayNames` map plus an inverted map so the automation name can map to canonical commands, and mark favorites.
   - when `ShowAvailableCommands` runs, annotate commands whose accessible nodes were found (favorites first).
   - add a `choose <number>` match that uses the last suggestion list to trigger the mapped command.
   - on unrecognized commands, gather actionable controls (favorites first), log them, append a numbered suggestion block to the fallback message, and store them for the `choose` helper.
3. Update plan documentation and logs to reflect the verified suggestions and help the user know which numbers to speak.

## Validation
- Run `dotnet build ExecuteCommands.csproj --framework net10.0-windows` (already passing).
- Manually trigger ``what can I say`` / fallback flows and look for `[DEBUG] Accessibility lookup ...: found` logs plus the numbered suggestion message.
- Speak `choose <number>` after a suggestion list appears and confirm the mapped command runs (log or actual VS action).

Next steps: implement numbered suggestion UI, confirm favorites, and expand the verification map once we know the real accessible names for the favorites in Visual Studio.