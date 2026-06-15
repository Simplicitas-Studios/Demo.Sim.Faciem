# Plan: MatGridList direction support

## Context
- `MatGridList` already exists as a basic, template-driven Material control.
- The next enhancement is to support **layout direction**:
  - `Vertical`
  - `Horizontal`
- Depending on the chosen direction, the user wants to configure:
  - either the amount of **columns** or the amount of **rows**
  - and the size expression should represent either **row height** or **column width**
- The user explicitly wants to **avoid duplicating fields**, so the public API should use shared property names that make sense in both directions.
- The Material demo should also gain additional examples covering the new direction feature.

## Approach
- Keep `MatGridList` as a custom tiled layout control built on `ScrollView`.
- Add a `Direction` setting and reinterpret the existing layout math based on that direction.
- Replace direction-specific public configuration with shared names.
- Recommended shared API:
  - `Direction` — vertical or horizontal flow
  - `TrackCount` — means columns in vertical mode, rows in horizontal mode
  - `TileSize` — means row height in vertical mode, column width in horizontal mode
  - `GutterSize` remains unchanged
- Preserve template-driven tile rendering via `VisualTreeAsset ItemTemplate` and per-tile `dataSource` assignment.
- Expand the demo page with both vertical and horizontal examples, covering ratio-based and fixed-size configurations.

## Files to modify
- `plans/mat-grid-list-direction.md`
- Likely runtime controls:
  - `Packages/Sim.Faciem.Material/Runtime/Controls/MatGridList.cs`
  - `Packages/Sim.Faciem.Material/Runtime/Controls/MatGridListDirection.cs`
- Likely sample/demo files:
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/GridList/IGridListDemoDataContext.cs`
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/GridList/GridListDemoViewModel.cs`
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/GridList/GridListDemoView.uxml`
  - possibly `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/MatDemoStyles.uss` if demo sizing/layout classes need expansion

## Reuse
- `Packages/Sim.Faciem.Material/Runtime/Controls/MatGridList.cs`
  - current tile creation/rebuild pattern
  - current ratio/fixed-size parsing logic
  - current `ScrollView`-based grid layout foundation
- `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/GridList/GridListDemoViewModel.cs`
  - existing demo tile data model and sample data approach
- `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/GridList/MaterialGridTileTemplate.uxml`
  - reusable tile template for new examples

## Steps
- [x] Define the shared public API for dual-direction layout:
  - [x] `Direction`
  - [x] `TrackCount`
  - [x] `TileSize`
  - [x] keep `GutterSize`
- [x] Refactor `MatGridList` layout calculation so:
  - [x] vertical mode uses `TrackCount` as column count and `TileSize` as row height
  - [x] horizontal mode uses `TrackCount` as row count and `TileSize` as column width
  - [x] ratio expressions continue to work sensibly in both directions
  - [x] scroll mode/scroller visibility align with the chosen direction
- [x] Add the direction enum/type needed for the new API.
- [x] Update demo data contracts/viewmodels as needed for additional examples.
- [x] Expand the Grid List demo page to include additional examples for:
  - [x] vertical ratio mode
  - [x] vertical fixed-size mode
  - [x] horizontal ratio mode
  - [x] horizontal fixed-size mode
- [x] Adjust demo USS for extra sizing/layout helper classes.
- [x] Build runtime, samples, and editor projects to verify the enhancement compiles cleanly.

## Verification
- Confirm vertical mode behaves like the current grid-list model, using columns and row-height semantics.
- Confirm horizontal mode lays out tiles left-to-right using rows and column-width semantics.
- Confirm `TrackCount` is the only count field exposed and works in both directions.
- Confirm `TileSize` is the only main-axis size field exposed and works in both directions.
- Confirm ratio expressions still produce predictable tile proportions in both modes.
- Confirm scroll behavior changes appropriately with direction.
- Confirm the demo page clearly shows all new direction scenarios.
- Build verification completed:
  - `dotnet build Sim.Faciem.Material.csproj`
  - `dotnet build Sim.Faciem.Material.Samples.csproj`
  - `dotnet build Sim.Faciem.Material.Editor.csproj`
