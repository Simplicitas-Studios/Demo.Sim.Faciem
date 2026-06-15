---
description: "Use when writing, editing, or reviewing any part of the Sim.Faciem.uGUI package. Provides package-level routing across runtime data binding and editor tooling."
applyTo: ["Packages/Sim.Faciem.uGUI/Runtime/**", "Packages/Sim.Faciem.uGUI/Editor/**"]
---
# Sim.Faciem.uGUI Framework — Package Boundaries

## Package Layout
- `Runtime/` — bindable properties, runtime binding creation, data sources, converter behaviours, property paths, uGUI binding components
- `Editor/` — property drawers, binding window, editor interaction providers, binding authoring UX

## Routing
- `Runtime/**` → follow `ugui-runtime`
- `Editor/**` → follow `ugui-editor`

## Boundary Rules
- Runtime code must not reference `UnityEditor` APIs.
- Editor tooling may depend on runtime serialization/contracts, but runtime types must remain editor-agnostic.
- Inspector tooling should edit `SimBindingInfo`, bindable properties, and property paths without reimplementing runtime binding behavior.

## General Rules
- Preserve the editor-first binding workflow described in the package README.
- Reuse existing R3, Unity.Properties, and property-path visitor patterns.
- Keep binding lifetimes explicit and tied to component or object lifetime.
