# Plan: Material text and numeric input fields

## Context
- Add a single generic Material input control that reuses the internal `MatFormField` chrome already introduced for `MatSelect`.
- The control must cover both text and numeric entry.
- Scope should stay lightweight: no validation / required-state behavior is needed for this iteration.
- Inputs should support optional SVG icons inside the field, with support for both leading and trailing placement.

## Approach
- Add one public control (recommended: `MatInput`) that wraps a native UI Toolkit input and internally manages text vs numeric behavior through a mode/type attribute rather than creating separate public controls.
- Reuse `Packages/com.sim.faciem-material/Runtime/Controls/MatFormField.cs` as the shared wrapper, following the same single-element public API pattern used by `Packages/com.sim.faciem-material/Runtime/Controls/MatSelect.cs`.
- Build the control around native UI Toolkit field types (`TextField` plus one or more numeric fields such as `IntegerField` / `FloatField`) so editing, focus, and binding behavior stay aligned with Unity's built-in controls.
- Extend the infix layout to support both leading and trailing icon visuals while keeping the input itself in the center/flexible slot.
- Mirror existing Material package patterns for UXML attributes, USS class naming, demo coverage, and editor stylesheet injection via `Packages/com.sim.faciem-material/Editor/MatEditorStyles.cs`.
- Reuse the existing icon pipeline (`IconCollection` / `VectorImage` background rendering) already used by `Packages/com.sim.faciem-material/Runtime/Controls/MatMenu.cs` for optional field icons.

## Files to modify
- `Packages/com.sim.faciem-material/Runtime/Controls/MatFormField.cs` (if the infix layout needs an explicit icon slot or helper API)
- New input control file(s) under `Packages/com.sim.faciem-material/Runtime/Controls/` (name still to confirm)
- New input USS file(s) under `Packages/com.sim.faciem-material/Runtime/Controls/Styles/`
- `Packages/com.sim.faciem-material/Editor/MatEditorStyles.cs` (to inject the new input stylesheet in editor contexts)
- `Packages/com.sim.faciem-material/Samples/MaterialDemo/Runtime/...` for a demo page or an extension of the existing select/menu demos
- Potentially new sample view-id / view-model / navigation wiring under `Packages/com.sim.faciem-material/Samples/MaterialDemo/Runtime/`

## Reuse
- `Packages/com.sim.faciem-material/Runtime/Controls/MatFormField.cs` — shared fill/outline/label/hint chrome with an `Infix` slot already intended for future `MatInput` / `MatTextarea` controls.
- `Packages/com.sim.faciem-material/Runtime/Controls/Styles/MatFormFieldStyles.uss` — shared form-field structure, padding, underline, outline, and subscript styling.
- `Packages/com.sim.faciem-material/Runtime/Controls/MatSelect.cs` — pattern for wrapping one public control around internal `MatFormField`, forwarding appearance/label/hint/disabled state, and managing focus styling.
- `Packages/com.sim.faciem-material/Runtime/Controls/MatMenu.cs` — existing `SetIconVisual(VisualElement, VectorImage)` pattern for showing/hiding SVG icons.
- `Packages/com.sim.faciem-material/Runtime/Icons/IconCollectionRegistry.cs` — package icon discovery / lookup.
- `Packages/com.sim.faciem-material/Editor/MatEditorStyles.cs` — centralized editor stylesheet registration that already includes form-field and per-control USS files.
- `Packages/com.sim.faciem-ugui/Editor/Controls/PropertyPathControl/PropertyPathField.cs` — example of wrapping a native `TextField` while exposing a bindable `[CreateProperty] Value` API.

## Steps
- [ ] Inspect the best native UI Toolkit base(s) for text and numeric editing (`TextField`, `IntegerField`, `FloatField`, etc.) and implement them behind one generic public `MatInput` API.
- [ ] Define the minimal public API: appearance, label, hint, placeholder, disabled, input mode/type, and leading/trailing icon attributes; leave required/error validation out of scope.
- [ ] Extend the internal field layout so the input plus optional leading/trailing icon visuals fit cleanly inside the existing `MatFormField` infix area without breaking fill/outline spacing.
- [ ] Reuse the existing icon lookup/rendering pattern (`IconCollection` + icon name -> `VectorImage`) for both icon positions.
- [ ] Add the new input USS styling and register it through `MatEditorStyles` for editor-hosted panels.
- [ ] Add or extend Material demo content to show text and numeric examples, including icon-less, leading-icon, trailing-icon, fill, outline, and disabled variants.
- [ ] Verify binding, focus visuals, disabled state, icon rendering, and numeric entry behavior in both runtime-style panels and editor windows.

## Verification
- Instantiate the new controls in the Material demo and verify fill + outline appearances.
- Verify text entry and numeric entry both update their bound values.
- Verify optional SVG icons render correctly inside the field and do not break layout/focus behavior.
- Verify editor-window styling still applies through `MatEditorStyles`.
