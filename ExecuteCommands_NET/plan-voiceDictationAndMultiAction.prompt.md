# Plan: Voice Dictation Form & Multi-Action Command Support

Create a dedicated voice typing form that auto-launches Windows voice dictation, captures natural language input with large font for accessibility, and enable multi-action commands defined in JSON. Implement composite action type (`RunMultipleActionsAction`) to execute sequential commands, store complex command definitions in a JSON file for easy customization, and add a keyboard shortcut (e.g., "natural dictate") to trigger the voice form without bloating the existing `NaturalLanguageInterpreter` class.

## Steps

1. **Create `VoiceDictationForm.cs`** — New WinForms dialog with multiline textbox (large font), auto-launch Windows voice typing, **Copy button** (copies textbox content to clipboard), Submit/Cancel buttons. Timer behavior: if `voiceDictationTimeoutMs = 0` (default), unlimited time—manual submit required; if > 0, auto-submit after timeout. Follows `DisplayMessage.cs` styling pattern.

2. **Create `RunMultipleActionsAction` record** in `ActionModels.cs` — New action type that holds a `List<ActionBase>` to execute actions sequentially with optional delays between them.

3. **Create `MultiActionExecutor.cs` helper class** in `Helpers/` — Encapsulates sequential action execution logic, applies delays between actions, logs execution progress, keeps `NaturalLanguageInterpreter` lean.

4. **Create `multi_actions.json`** — User-editable configuration file with command definitions (e.g., "setup desktop for development" → list of actions) and `voiceDictationTimeoutMs` field (0 = unlimited, >0 = auto-submit milliseconds). Loaded at startup by a new `MultiActionLoader.cs` helper.

5. **Extend `NaturalLanguageInterpreter.InterpretAsync()`** — Add rules to detect "natural dictate" phrase and multi-action commands from JSON; return appropriate action types without adding implementation code to the class itself.

6. **Add execution handler** in `NaturalLanguageInterpreter.ExecuteActionAsync()` — Single case for `RunMultipleActionsAction` that delegates to `MultiActionExecutor.Execute()`.

## Further Considerations

1. **Action serialization** — How to represent actions in JSON (`RunMultipleActionsAction` contains `ActionBase` records)? → Recommend: Define action JSON schema (type + parameters object) and deserialize via reflection or factory pattern in `MultiActionExecutor`.

2. **Keyboard shortcut binding** — Should "natural dictate" trigger form via rule in `InterpretAsync()`, or needs OS-level hotkey? → Recommend: Use existing natural language interpretation ("natural dictate" → `OpenVoiceDictationFormAction`), leverages existing architecture.

3. **Error handling in sequences** — Should multi-action stop on first failure or continue? → Recommend: Add `continueOnError` boolean flag to `RunMultipleActionsAction`, log failures per action.
