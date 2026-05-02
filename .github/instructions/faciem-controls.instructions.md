---
description: "Use when writing, editing, or reviewing Sim.Faciem.Controls (BindableButton, BindableListView, HyperLinkLabel, SerializedCommand, command binding, list binding) or Sim.Faciem.Controls.Editor (SerializedCommandPropertyDrawer, SerializedListReferencePropertyDrawer). Covers reactive control binding, SerializedCommand patterns, and property drawer conventions."
applyTo: ["Packages/Sim.Faciem/Runtime/Controls/**", "Packages/Sim.Faciem/InternalEditor/Controls/**"]
---
# Sim.Faciem.Controls -- Coding Conventions

## Assemblies & Namespaces
| Assembly | Namespace | Platform |
|---|---|---|
| `Sim.Faciem.Controls` | `Sim.Faciem` | netstandard2.1 |
| `Sim.Faciem.Controls.Editor` | `Plugins.Sim.Faciem.Editor` | Editor only |

**Dependencies:** `Sim.Faciem` (core), `Plugins.Sim.Faciem.Shared`, R3, UI Toolkit.

## Control Pattern
- Mark controls with `[UxmlElement]` and declare as `public partial class Bindable{Name} : {BaseControl}`.
- Use `[UxmlAttribute, CreateProperty]` on properties settable from UXML that participate in binding.
- Manage control-lifetime subscriptions via `this.RegisterDisposableBag()` (from `Plugins.Sim.Faciem.Shared`).
- Use a separate inner `DisposableBag` for subscriptions that must reset when a bound property changes (e.g. `_commandSubscriptions`).

```csharp
public BindableButton()
{
    var lifeTimeDisposables = this.RegisterDisposableBag();
    _commandSubscriptions = new DisposableBag();
    lifeTimeDisposables.Add(_commandSubscriptions);
    // wrap Unity events as R3 observables
    lifeTimeDisposables.Add(Observable.FromEvent(
        x => clickable.clicked += x,
        x => clickable.clicked -= x)
        .Subscribe(_ => _command?.Command?.Execute(Unit.Default)));
}
```

## SerializedCommand Binding
- `SerializedCommand` is the UXML/serialized reference to a command property on the data source.
- When a `SerializedCommand` is assigned, dispose and recreate `_commandSubscriptions`, then subscribe to `CanExecuteObs` and `IsVisibleObs`.
- Controls call `_command.Command.Execute(Unit.Default)` on interaction -- never invoke ViewModel methods directly.

## Observable Event Pattern
- Wrap Unity events as R3 observables using `Observable.FromEvent(add, remove)`.
- Always add resulting subscriptions to the control's lifetime `DisposableBag`.

## List Binding
- `BindableListView` binds to observable collections; list binding infrastructure lives in `ListBinding/`.
- Use `ControlConverterRegistry` for registering custom value converters.

## Property Drawers (Sim.Faciem.Controls.Editor)
- Property drawers live in `InternalEditor/Controls/` under the `Sim.Faciem.Controls.Editor` asmdef.
- One drawer per serialized type: `SerializedCommandPropertyDrawer`, `SerializedListReferencePropertyDrawer`.
- Use `[CustomPropertyDrawer(typeof(X))]` -- do not add editor logic to the runtime type.

## Do Not
- Do not reference `UnityEditor` APIs from the `Sim.Faciem.Controls` runtime assembly.
- Do not apply inline styles via `style.*` -- appearance must come from USS.
- Do not reference `Sim.Faciem.Material` from this assembly.
