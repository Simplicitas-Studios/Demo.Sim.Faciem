---
description: "Use when writing, editing, or reviewing Sim.Faciem.uGUI editor code — property drawers, binding-window tooling, property-path pickers, manipulation providers, and other authoring utilities."
applyTo: "Packages/Sim.Faciem.uGUI/Editor/**"
---
# Sim.Faciem.uGUI Editor — Coding Conventions

## Assembly & Namespace
- Assembly: `Sim.Faciem.uGUI.Editor`
- Root namespace: `Sim.Faciem.uGUI.Editor`
- Depends on: `Sim.Faciem.uGUI`, `Sim.Faciem`, Unity Editor APIs, `Unity.Properties`, `R3`, and `UniTask`

## Editor Responsibilities
- Author and inspect runtime binding configuration.
- Provide property drawers for bindable fields.
- Drive the binding window, property-path selection, converter selection, and related editor UX.
- Reflect runtime contracts without duplicating runtime binding execution logic.

## Patterns
- Property drawers use `[CustomPropertyDrawer]` and build UI with UIElements.
- Binding-window ViewModels follow Faciem `ViewModel<T>` patterns and use `[CreateProperty]` for bound properties.
- Manipulation providers and editor helpers should reuse shared runtime abstractions such as `SimBindingInfo`, `SimPropertyPath`, and bindable interfaces.
- Use R3 for reactive editor interactions where the package already follows that pattern.

## Do Not
- Do not put `UnityEditor` dependencies into runtime files.
- Do not mutate runtime-only behavior from editor code except through serialized configuration.
- Do not bypass the existing property-path/converter tooling with ad-hoc inspector hacks when the shared infrastructure can be extended.
