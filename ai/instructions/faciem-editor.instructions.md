---
description: "Use when writing, editing, or reviewing Sim.Faciem.Editor -- FaciemEditorWindow, FaciemToolbarOverlay, FaciemPopupWindowContent, editor DI container (EditorInjector), or editor navigation services. Covers editor window lifecycle, RegionManager wiring, async UniTask patterns, and DI conventions for Unity Editor tooling in the Faciem framework."
applyTo: "Packages/Sim.Faciem/Editor/**"
---
# Sim.Faciem.Editor -- Coding Conventions

## Assembly & Namespace
- Assembly: `Sim.Faciem.Editor`, editor-only (NET 4.6)
- Namespace: `Plugins.Sim.Faciem.Editor`
- Depends on: `Sim.Faciem` (core runtime), R3, Unity Editor APIs, UniTask

## Editor Windows
- Extend `FaciemEditorWindow` (not `EditorWindow` directly) for all Faciem-aware editor windows.
- `FaciemEditorWindow.CreateGUI()` wires up a `RegionManager`, navigates to `_initialViewId`, and sets `rootVisualElement.dataSource = this`.
- Serialized fields `_windowRegionName` (`RegionNameDefinition`) and `_initialViewId` (`EditorViewIdAsset`) are required -- configure them in the inspector or via `[SerializeField]`.
- Override `NavigateTo()` / `NavigateAway()` for lifecycle; always call `return base.NavigateTo()` / `base.NavigateAway()` unless intentionally replacing.

```csharp
public class MyEditorWindow : FaciemEditorWindow
{
    protected override async UniTask NavigateTo()
    {
        await base.NavigateTo();
        // additional per-window setup
    }
}
```

## Toolbar Overlays
- Extend `FaciemToolbarOverlay` for Faciem-aware toolbar overlays.
- Override `CreateRootElement()` to customize the panel root.

## Popup Window Content
- Extend `FaciemPopupWindowContent` for popup menus with MVVM support.

## DI Container
- Resolve services via `EditorInjector.Instance.ResolveInstance<T>()`.
- Service registration lives in `DI/` -- follow existing installer patterns.
- Do not use `FaciemBridge` (runtime-only DI bridge) in editor code.

## Navigation
- Use `IEditorToolNavigationService Navigation` (exposed by `FaciemEditorWindow`) for page-level navigation.
- Navigation is always async (`UniTask`); await calls before proceeding.

## Async Patterns
- Use `UniTask` everywhere. Do not use `Task`, coroutines, or `async void`.
- Use `async UniTaskVoid` for fire-and-forget; ensure exceptions are handled with `.Forget(e => Debug.LogException(e))`.

## Do Not
- Do not call `EditorApplication.delayCall` for view bootstrap -- `FaciemEditorWindow.CreateGUI` handles it.
- Do not resolve services with `FaciemBridge`.
- Do not reference `Sim.Faciem.Material.Editor` from this assembly.
