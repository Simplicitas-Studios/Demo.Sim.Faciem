---
description: "Use when writing, editing, or reviewing any part of the Sim.Faciem package. Provides package-level boundaries and routing across runtime, shared, controls, editor, and internal editor code."
applyTo: ["Packages/Sim.Faciem/Runtime/**", "Packages/Sim.Faciem/Shared/**", "Packages/Sim.Faciem/Editor/**", "Packages/Sim.Faciem/InternalEditor/**"]
---
# Sim.Faciem Framework — Package Boundaries

## Package Layout
- `Runtime/` — MVVM runtime, commands, navigation, regions, view model construction
- `Shared/` — reusable helpers shared by package assemblies
- `Runtime/Controls/` — bindable UI Toolkit controls
- `Editor/` — editor windows, overlays, editor DI, editor navigation
- `InternalEditor/` — code generation, setup helpers, design-time tooling
- `InternalEditor/Controls/` — property drawers and control-related editor tooling

## Routing
- Runtime + Shared core logic → follow `faciem-core`
- Runtime controls + `InternalEditor/Controls` → follow `faciem-controls`
- `Editor/**` → follow `faciem-editor`
- `InternalEditor/**` outside `Controls/` → follow `faciem-internal-editor`

## Boundary Rules
- Runtime assemblies must not depend on editor assemblies.
- ViewModels stay plain C# and use Faciem command/navigation abstractions.
- Editor-only logic stays in `Editor/` or `InternalEditor/`.
- Control styling and behavior stay inside the controls layer; do not leak them into the MVVM core.

## General Rules
- Reuse established R3 and UniTask patterns already present in the package.
- Prefer extending existing interfaces/services over parallel abstractions.
- Keep generated-code and setup concerns out of runtime files.
