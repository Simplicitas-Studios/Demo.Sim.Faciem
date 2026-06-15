# Plan: MatGridList virtualization

## Context
- `MatGridList` currently builds one `VisualElement` tile per item in `ItemSource` and positions all of them absolutely inside the scroll content.
- That works for small/medium grids, but the user now wants the control to handle **multiple hundreds of entries efficiently**.
- The current implementation already has a stable tile-size model:
  - `Direction`
  - `TrackCount`
  - `TileSize`
  - `GutterSize`
- That is a good fit for virtualization because tile positions can be derived mathematically without realizing every tile.
- The control must remain template-driven: virtualization should not remove or weaken the existing `VisualTreeAsset` + per-tile `dataSource` authoring model.
- The user wants a **large-data demo example included** in this pass so the virtualization behavior is easy to test visually.

## Approach
- Keep `MatGridList` as a custom `ScrollView`-based control.
- Add **windowed virtualization with pooled tile elements** instead of instantiating one tile per item.
- Reuse the existing tile-template/data-source pattern, but move it to a pooled lifecycle:
  - create only enough tile elements to cover the visible viewport plus a small overscan buffer
  - recycle those tile elements as the scroll position changes
  - rebind pooled elements to the correct item index and data object
- Preserve the public API where possible; virtualization should be an internal implementation detail for v1.
- Recommended v1 virtualization model:
  - compute cross-axis tile size and main-axis tile size exactly as today
  - compute visible line range from scroll offset and viewport size
  - realize only the indices in the visible line range plus overscan
  - maintain a pool of reusable tile visuals and rebind them during scroll/layout updates
- Large-scope note:
  - do not attempt variable-size virtualization in this pass
  - rely on the current fixed/ratio-derived tile size model so visible range math stays deterministic and efficient

## Files to modify
- `plans/mat-grid-list-virtualization.md`
- Runtime control:
  - `Packages/Sim.Faciem.Material/Runtime/Controls/MatGridList.cs`
- Demo/sample files for the approved large-data example:
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/GridList/IGridListDemoDataContext.cs`
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/GridList/GridListDemoViewModel.cs`
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/GridList/GridListDemoView.uxml`
  - possibly `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/MatDemoStyles.uss`

## Reuse
- `Packages/Sim.Faciem.Material/Runtime/Controls/MatGridList.cs`
  - current layout math for direction, tracks, gutters, and tile sizing
  - current template-cloning and `dataSource` assignment behavior
- `Packages/Sim.Faciem.Material/Runtime/Controls/MatList.cs`
  - conceptual reference for bind/rebind lifecycle, although `MatGridList` needs custom pooling rather than `ListView`
- `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/GridList/GridListDemoViewModel.cs`
  - existing demo data model that can be extended with a larger dataset

## Steps
- [ ] Analyze the current full-realization flow in `MatGridList` and identify where to replace it with pooled virtualization.
- [ ] Define the internal virtualization model:
  - [ ] visible line calculation
  - [ ] overscan strategy
  - [ ] pooled tile allocation count
  - [ ] tile rebind/update lifecycle
- [ ] Refactor `MatGridList` so it no longer creates one visual per item and instead separates:
  - [ ] logical item count / content extent
  - [ ] pooled realized visuals
  - [ ] per-index bind/reposition logic
- [ ] Implement a reusable tile pool that:
  - [ ] creates a bounded number of tile roots
  - [ ] clones template content once per pooled tile
  - [ ] reassigns `dataSource` as tiles are recycled
  - [ ] updates absolute tile position and visibility per bound index
- [ ] Keep content extent calculation correct so scrollbars still represent the full logical dataset.
- [ ] Trigger virtualization refresh on:
  - [ ] geometry changes
  - [ ] scroll position changes
  - [ ] item-source changes
  - [ ] layout-setting changes (`Direction`, `TrackCount`, `TileSize`, `GutterSize`)
  - [ ] template changes (`ItemTemplate`)
- [ ] Add or expand a demo example with several hundred entries to make the feature practically testable.
- [ ] Prefer keeping the current small examples too, so the demo covers both ordinary usage and large-data virtualization behavior.
- [ ] Build runtime, samples, and editor projects to verify the virtualization refactor compiles cleanly.

## Verification
- Confirm the number of realized tile visuals stays bounded as item count grows.
- Confirm a large dataset (several hundred items) scrolls smoothly in both vertical and horizontal modes.
- Confirm recycled tiles always bind to the correct item and template content updates correctly.
- Confirm no full-content rebuild occurs during ordinary scrolling once the pool is initialized.
- Confirm scrollbars and content extents still represent the full logical dataset.
- Confirm direction changes still work with virtualization enabled.
- Confirm layout-setting changes rebuild/reflow the virtual window correctly.
- Confirm the large-data demo example renders and scrolls correctly in the Material Demo.
- Build verification:
  - `dotnet build Sim.Faciem.Material.csproj`
  - `dotnet build Sim.Faciem.Material.Samples.csproj`
  - `dotnet build Sim.Faciem.Material.Editor.csproj`

## Requested Note
- This virtualization pass is specifically intended to make `MatGridList` efficient for several hundred entries without changing the template-driven authoring model.
- The implementation should aim for smooth ordinary scrolling rather than introducing a broader architectural rewrite or a second control type.

## Summary
- Goal: keep the current `MatGridList` API and template workflow, but replace full realization with pooled, windowed virtualization suitable for hundreds of items.
- Expected result: scrolling cost should scale with the visible window, not the total item count.
