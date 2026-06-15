---
description: "Use when writing, editing, or reviewing Sim.Faciem.uGUI runtime code — bindable properties, runtime binding creation, data sources, converter behaviours, property paths, and codeless uGUI binding components."
applyTo: "Packages/Sim.Faciem.uGUI/Runtime/**"
---
# Sim.Faciem.uGUI Runtime — Coding Conventions

## Assembly & Namespace
- Assembly: `Sim.Faciem.uGUI`
- Root namespace: `Sim.Faciem.uGUI`
- Depends on: `Sim.Faciem`, `R3`, `Unity.Properties`, Unity runtime APIs

## Runtime Model
- `SimDataSourceMonoBehaviour` is the MonoBehaviour-side data source base type.
- `BindableProperty<T>` and related bindable abstractions hold serialized binding configuration and runtime binding state.
- Binding creation belongs in `Binding/` infrastructure (`SimBindingFactory`, visitors, runtime binding types).
- Components such as `TMPBindable` and `SimAutoBindingComponent` activate and own bindings at runtime.

## Binding Rules
- Preserve serialization shape for `SimBindingInfo`, property paths, targets, and converter lists.
- Keep runtime binding creation deterministic and centralized; do not duplicate path-evaluation logic inside UI components.
- Use R3 for all observable plumbing.
- Tie subscriptions to object lifetime with `AddTo(this)`, a disposable holder, or explicit disposal.

## Component Rules
- Runtime components may use `MonoBehaviour` and Unity runtime APIs.
- Keep components thin: configure or activate bindings, then delegate actual reactive wiring to the binding infrastructure.
- Editor-only quality-of-life methods such as `Reset()` must remain inside `#if UNITY_EDITOR` guards.

## Do Not
- Do not reference `UnityEditor` APIs.
- Do not move editor authoring behavior into runtime classes.
- Do not break binding compatibility by renaming serialized fields casually.
