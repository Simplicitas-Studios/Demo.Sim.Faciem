---
description: "Use when writing, editing, or reviewing the Material Demo sample — ViewModels, DataContext interfaces, UXML, and the demo editor window. Covers MVVM conventions, demo-only scope rules, and cross-assembly boundary enforcement."
applyTo: "Packages/Sim.Faciem.Material/Samples/**"
---
# Material Demo Samples — Coding Conventions

## Assembly & Namespace
- Runtime assembly: `Sim.Faciem.Material.Samples` (netstandard2.1), namespace: `Sim.Faciem.Material.Samples`
- Editor assembly: `Sim.Faciem.Material.Samples.Editor` (NET 4.6), namespace: `Sim.Faciem.Material.Samples.Editor`
- Depends on: `Sim.Faciem` (core) — **not** on `Sim.Faciem.Material` directly (controls are used via UXML only).
- **Demo code must never be referenced from the runtime or editor assemblies** (`Sim.Faciem.Material`, `Sim.Faciem.Material.Editor`). Flag any such dependency as a cross-assembly leak.

## MVVM Pattern
Each demo page follows a strict three-file pattern:

| File | Purpose |
|---|---|
| `I{Page}DataContext.cs` | Interface extending `IDataContext`; declares bindable surface |
| `{Page}ViewModel.cs` | `ViewModel<TSelf>` implementing `I{Page}DataContext` |
| `{Page}.uxml` | UXML declaring the UI; binds to `I{Page}DataContext` properties |

```csharp
// Interface
public interface IButtonDemoDataContext : IDataContext
{
    bool ButtonsEnabled { get; }
    Command ToggleEnabled { get; }
}

// ViewModel
public class ButtonDemoViewModel : ViewModel<ButtonDemoViewModel>, IButtonDemoDataContext
{
    [CreateProperty] public bool ButtonsEnabled { get; private set; } = true;
    [CreateProperty] public Command ToggleEnabled { get; private set; }

    public ButtonDemoViewModel()
    {
        ToggleEnabled = Command.Execute(() => ButtonsEnabled = !ButtonsEnabled);
    }
}
```

## Bindable Properties
- Annotate every property the UXML binds to with `[CreateProperty]`.
- Use `SetProperty(ref _field, value)` for change notification in setters.
- Keep ViewModels free of Unity Engine types — no `MonoBehaviour`, no `ScriptableObject`.

## Commands
- Use `Command.Execute(action)` for simple commands.
- Use `Command.Execute(action, canExecute)` when the command has an enabled condition.

## UXML Conventions
- UXML files live alongside their ViewModel in the same folder.
- Reference Material controls by full qualified name: `<Sim.Faciem.Controls.MatButton .../>`.
- Do not embed inline styles in UXML — use USS class attributes only.

## Demo Scope
- The demo showcases features; it is not production code. Avoid over-engineering ViewModels.
- Each demo page is self-contained; pages must not share state through static fields.
- Demo data (Pokémon / Sinnoh theme, etc.) is intentional flavor — keep it consistent.

## Editor Demo Window
- `MatDemoWindow.cs` extends `MatFaciemEditorWindow` (from `Sim.Faciem.Material.Editor`).
- Regional organization files in `Editor/Regional/` define the navigation structure — follow existing patterns when adding new pages.
