---
description: "Use when writing, editing, or reviewing the Sim.Faciem core MVVM runtime (BaseViewModel, IDataContext, Region, RegionManager, navigation, Commands) or the Sim.Faciem.Shared utilities (DisposableBagHolder, VisualElement extensions). Covers ViewModel lifecycle, reactive command patterns, region-based navigation, and cross-assembly utility conventions."
applyTo: ["Packages/Sim.Faciem/Runtime/**", "Packages/Sim.Faciem/Shared/**"]
---
# Sim.Faciem Core -- Coding Conventions

## Assemblies & Namespaces
| Assembly | Namespace | Platform |
|---|---|---|
| `Sim.Faciem` | `Sim.Faciem`, `Sim.Faciem.Commands` | netstandard2.1 |
| `Sim.Faciem.Shared` | `Plugins.Sim.Faciem.Shared` | netstandard2.1 |

## ViewModel Pattern
- Extend `BaseViewModel` for all ViewModels.
- ViewModels are plain C# -- **no `MonoBehaviour`**, no Unity Engine types.
- Use `protected DisposableBag Disposables` (inherited) to track all subscription lifetimes.
- Override `NavigateTo()` / `NavigateAway()` for lifecycle logic; always return `UniTask.CompletedTask` if not async.
- Use `protected IViewModelNavigationService Navigation` for programmatic navigation; never resolve `INavigationService` directly.
- Use `protected ICommandBuilderFactory Command` (inherited) to create commands -- do not instantiate `Command` / `Command<T>` directly inside ViewModels.

```csharp
public class MyViewModel : BaseViewModel
{
    [CreateProperty]
    public string Title { get; private set; } = "Hello";

    public Command DoSomething { get; private set; }

    public MyViewModel()
    {
        DoSomething = Command.Execute(() => Title = "Clicked");
    }

    protected override UniTask NavigateTo()
    {
        // setup subscriptions here, add to Disposables
        return UniTask.CompletedTask;
    }
}
```

## Bindable Properties
- Annotate every UXML-bound property with `[CreateProperty]`.
- Use `SetProperty(ref _field, value)` for change notification in mutable properties.
- Keep interfaces (`IDataContext` subtypes) as the binding surface -- ViewModels implement interfaces, not the other way around.

## Commands
- `Command` (no parameter) and `Command<T>` (typed parameter) both extend `ReactiveCommand`.
- Commands expose `CanExecuteObs` and `IsVisibleObs` as `Observable<bool>` -- controls subscribe automatically.
- Use `ICommandBuilderFactory` (via `Command.Execute(...)`, `Command.Create(...)`) inside `BaseViewModel` subclasses.
- Outside ViewModels (e.g. demo code), use `Command.Execute(action)` factory directly.

## Regions & Navigation
- `Region` is a `VisualElement` that manages one or more views identified by `ViewId`.
- `RegionManager` owns `Region` instances; do not hold direct references to `Region` from ViewModels -- use `AddRegion(IRegion)` to register.
- Navigation is always async (`UniTask`); await navigation calls before proceeding.

## DisposableBagHolder (Sim.Faciem.Shared)
- Use `DisposableBagHolder` (not raw `DisposableBag`) for disposables attached to `VisualElement` lifetimes.
- Register via extension: `this.RegisterDisposableBag()` on a `VisualElement`.
- Inside `BaseViewModel`, use the inherited `Disposables` (`DisposableBag`) instead.

## Reactive (R3)
- Use `R3` observables throughout. Do not use `UniRx` or `System.Reactive`.
- Subscriptions must always be added to `Disposables` or a `DisposableBag` -- no fire-and-forget subscriptions.

## Do Not
- Do not use `MonoBehaviour` in ViewModels.
- Do not reference `UnityEditor` APIs -- this is a runtime assembly.
- Do not reference `Sim.Faciem.Controls` or `Sim.Faciem.Material` from the core runtime.
