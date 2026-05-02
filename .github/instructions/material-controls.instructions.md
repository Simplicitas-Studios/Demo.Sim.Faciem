---
description: "Use when writing, editing, or reviewing Material Design UI Toolkit controls (MatButton, MatSelect, MatFormField, MatOption, etc.). Covers C# conventions, MVVM binding patterns, CSS class naming, and asmdef boundaries for Sim.Faciem.Material runtime controls."
applyTo: "Packages/Sim.Faciem.Material/Runtime/Controls/**"
---
# Material Controls — Coding Conventions

## Assembly & Namespace
- Assembly: `Sim.Faciem.Material` (netstandard2.1)
- Namespace: `Sim.Faciem.Controls`
- Depends on: `com.sim.faciem` (parent package)

## Class Structure
- Mark controls with `[UxmlElement]` and declare as `public partial class Mat{Name} : VisualElement` (or extend an appropriate Faciem base class such as `BindableButton`).
- Use `[UxmlAttribute]` on public UXML-settable properties.
- Keep internal DOM construction inside the constructor or an `Init()` method; never in property setters.

## CSS Class Constants
- Declare all CSS class names as `public const string {Role}ClassName` at the top of the class.
- Use BEM-style naming: `mat-{component}`, `mat-{component}__{element}`, `mat-{component}--{modifier}`.
- Apply variant/color via `AddToClassList` / `RemoveFromClassList` only — **never load stylesheets programmatically**.

```csharp
// Good
public const string BaseClassName    = "mat-button-base";
public const string RaisedClassName  = "mat-raised-button";
public const string PrimaryClassName = "mat-primary";
```

## Styling Rules
- Stylesheets are applied exclusively via PanelSettings `.tss` theme files at runtime.
- For editor panels, stylesheets are injected via `MatEditorStyles.ApplyTo()`.
- Never call `styleSheets.Add(...)` or `StyleSheet.CreateInstance(...)` inside a control.

## MVVM & Binding
- Expose bindable data as `[CreateProperty]` properties.
- Use `SetProperty(ref _field, value)` to trigger change notifications.
- Commands must be of type `Command` (from `Sim.Faciem.Commands`) and initialized in the constructor.
- Controls consume bindings from a parent `IDataContext`; they do not hold ViewModels themselves.

## Reactive Streams
- Use `R3` observables for internal event plumbing.
- Dispose subscriptions via `DisposableBagHolder` attached to the control's `detachFromPanelEvent`.

## Partial Classes
- Split large controls across partial files: `Mat{Name}.cs` (main), `Mat{Name}.Uxml.cs` (UXML factory helpers) if needed.
- Avoid sprawling single files; keep each file focused on one concern.

## Do Not
- Do not reference `UnityEditor` APIs — this assembly is runtime-only.
- Do not add inline styles via `style.*`; all appearance must come from USS.
- Do not reference the Samples assembly (`Sim.Faciem.Material.Samples`).
