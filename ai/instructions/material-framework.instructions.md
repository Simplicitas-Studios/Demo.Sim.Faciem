---
description: "Use when writing, editing, or reviewing any part of the Sim.Faciem.Material package. Provides package-level routing across controls, theming, editor tooling, diagnostics, and samples."
applyTo: ["Packages/Sim.Faciem.Material/Runtime/**", "Packages/Sim.Faciem.Material/Editor/**", "Packages/Sim.Faciem.Material/Samples/**"]
---
# Sim.Faciem.Material Framework — Package Boundaries

## Package Layout
- `Runtime/Controls/` — Material UI Toolkit control classes
- `Runtime/Controls/Styles/` — component USS
- `Runtime/Themes/` — theme USS/TSS and token definitions
- `Editor/` — editor-only windows, setup helpers, editor stylesheet injection
- `Samples/` — demo runtime/editor code

## Routing
- Control behavior in `Runtime/Controls/**` → follow `material-controls`
- Theme/token/style work in `Runtime/Themes/**` or `Runtime/Controls/Styles/**` → follow `material-theming`
- Editor tooling in `Editor/**` → follow `material-editor`
- Demo/sample code in `Samples/**` → follow `material-demo`
- Visual bug diagnosis before editing → follow `material-diagnostics`

## Boundary Rules
- Runtime controls must not use `UnityEditor` APIs.
- Editor tooling must not depend on sample assemblies.
- Sample code must not leak back into runtime or editor assemblies.
- Styling belongs in USS/theme files, not inline C# styles.

## General Rules
- Preserve BEM naming and token-driven theming.
- Reuse existing Material control patterns before adding new ones.
- Keep diagnostic workflows read-only unless the task explicitly moves into a fixing specialist.
