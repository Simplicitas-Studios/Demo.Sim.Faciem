---
description: "Use when writing, editing, or reviewing Unity Editor tooling for Sim.Faciem.Material — editor windows, toolbar overlays, setup wizards, and the MatStyleAutoInjector. Covers editor-only asmdef rules, MatEditorStyles usage, and Unity Editor API patterns."
applyTo: "Packages/Sim.Faciem.Material/Editor/**"
---
# Material Editor Tools — Coding Conventions

## Assembly & Namespace
- Assembly: `Sim.Faciem.Material.Editor` (NET 4.6, editor-only platform)
- Namespace: `Sim.Faciem.Material.Editor`
- Depends on: `Sim.Faciem.Material` (runtime) and `Plugins.Sim.Faciem.Editor`
- **Never** reference `Sim.Faciem.Material.Samples` from this assembly.

## Editor Windows
- Extend `MatFaciemEditorWindow` (not `EditorWindow` directly) for windows that display Material Design controls.
  - `MatFaciemEditorWindow` auto-injects the active Material editor theme via `MatEditorStyles.ApplyTo(rootVisualElement)` and reacts to theme switches.
- For toolbar overlays, extend `MatFaciemToolbarOverlay`.
  - Override `CreateRootElement()` only; `CreatePanelContent()` is sealed.

## Stylesheet Injection
- Never call `rootVisualElement.styleSheets.Add(...)` manually in a Material editor window.
- Always rely on `MatEditorStyles.ApplyTo(element)` which returns an `IDisposable` — store it and dispose in `NavigateAway` / overlay teardown.

```csharp
// Good — automatic theme tracking
_themeSubscription = MatEditorStyles.ApplyTo(rootVisualElement);

// Bad — manual and static
rootVisualElement.styleSheets.Add(mySheet);
```

## MatStyleAutoInjector
- Use `[InitializeOnLoad]` for auto-run editor logic; call `EditorApplication.delayCall +=` to defer until `AssetDatabase` is ready.
- Detection helpers (`HasMaterialControlsInProject`, `FindUnconfiguredPanelSettingsPaths`) are `internal static` so setup windows can reuse them without duplication.
- Session-scoped flags use `SessionState.SetBool(key, value)` with a key constant defined in the injector.

## Menu Items
- Register menu items under `Tools/Sim.Faciem/Material/...`.
- Manual menu triggers must reset session dismissal state before opening any window.

## Async Patterns
- Use `UniTask` for async editor operations (not `Task` or coroutines).
- `NavigateTo()` and `NavigateAway()` override points are async — always `return base.NavigateTo()` / `base.NavigateAway()` unless intentionally replacing the base behavior.

## Do Not
- Do not use `UnityEngine.UIElements` runtime panel APIs inside editor windows — use Editor-specific variants.
- Do not add `[RuntimeInitializeOnLoadMethod]` — this assembly is editor-only.
- Do not reference the Samples assembly.
