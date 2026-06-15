# Plan: MatGridList

## Context
- The next Material control should be `MatGridList`, inspired by Angular Material's basic `mat-grid-list`.
- Scope is intentionally limited to the basic grid-list only; no advanced variants are needed for the first version.
- The user wants the control to be **template-driven**: a `VisualTreeAsset` should be assigned and instantiated for each tile.
- Based on the current package structure, this should live in `Sim.Faciem.Material` runtime controls and follow the same template/data-source pattern already used by `MatList`.

## Approach
- Recommended implementation: build `MatGridList` as a **custom tiled `VisualElement`-based control**, not on top of `ListView`.
- Reasoning:
  - `mat-grid-list` is fundamentally a layout/grid problem, not a scrolling list-selection problem.
  - A custom layout is a better fit for fixed columns, gutter spacing, row-height logic, and tile sizing parity with Angular Material.
  - It avoids `ListView` virtualization and wrapper-item behavior that would complicate precise tile layout.
- Reuse the proven pattern from `MatList` for:
  - `IList ItemSource`
  - `VisualTreeAsset ItemTemplate`
  - assigning the current item to each instantiated tile's `dataSource`
- Keep v1 focused on uniform tile sizing.
- Recommended v1 feature set:
  - `ItemSource`
  - `ItemTemplate`
  - `Cols`
  - `GutterSize`
  - `RowHeight` supporting both a fixed pixel value and a simple ratio-style mode
  - uniform tiles only (no per-item colspan/rowspan yet)

## Files to modify
- `plans/material-list-polish.md`
- Likely runtime controls:
  - `Packages/Sim.Faciem.Material/Runtime/Controls/MatGridList.cs`
  - possibly a small helper enum/value type for row-height interpretation if needed
- Likely styles:
  - `Packages/Sim.Faciem.Material/Runtime/Controls/Styles/MatGridListStyles.uss`
  - possibly `Packages/Sim.Faciem.Material/Runtime/Controls/Styles/MatGridListColors.uss`
- Likely theme/editor wiring:
  - `Packages/Sim.Faciem.Material/Runtime/Themes/MatIndigoTheme.tss`
  - `Packages/Sim.Faciem.Material/Runtime/Themes/MatDeepPurpleTheme.tss`
  - `Packages/Sim.Faciem.Material/Runtime/Themes/MatPinkBlueGreyTheme.tss`
  - `Packages/Sim.Faciem.Material/Runtime/Themes/MatPurpleGreenTheme.tss`
  - `Packages/Sim.Faciem.Material/Editor/MatEditorStyles.cs`
- Likely sample/demo files:
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/GridList/IGridListDemoDataContext.cs`
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/GridList/GridListDemoViewModel.cs`
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/GridList/GridListDemoView.uxml`
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/GridList/` tile template UXML asset(s)
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/IMatDemoWindowDataContext.cs`
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/MatDemoWindowViewModel.cs`
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/MatDemoWindowView.uxml`
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/WellKnownMatDemoViewIds.cs`
  - matching runtime/editor view-id assets for a demo page

## Reuse
- `Packages/Sim.Faciem.Material/Runtime/Controls/MatList.cs`
  - template cloning
  - `IList ItemSource`
  - per-item `dataSource` assignment
- `Packages/Sim.Faciem.Material/Runtime/Controls/Styles/MatListStyles.uss`
  - naming/style organization pattern for control USS
- `Packages/Sim.Faciem.Material/Editor/MatEditorStyles.cs`
  - editor stylesheet injection pattern
- `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/List/ListDemoViewModel.cs`
  - sample data/viewmodel shape for template-driven controls
- `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/List/MaterialListItemTemplate.uxml`
  - reference for template-driven tile content binding

## Steps
- [x] Inspect the final `MatList` implementation and reuse its template/data-source binding pattern for tiles.
- [x] Define the v1 public API for `MatGridList`:
  - [x] `IList ItemSource`
  - [x] `VisualTreeAsset ItemTemplate`
  - [x] `int Cols`
  - [x] `float GutterSize`
  - [x] row-height API (fixed pixels and/or ratio mode)
- [x] Implement a custom tiled runtime layout control in `Runtime/Controls/MatGridList.cs`.
- [x] Implement tile instantiation/rebuild logic so each tile clones the template and receives the correct `dataSource`.
- [x] Add `MatGridList` USS styling and grid-specific fallback tokens.
- [x] Wire the stylesheet into runtime themes and `MatEditorStyles.cs`.
- [x] Add a dedicated Material demo page for Grid List with tile templates.
- [x] Add demo navigation wiring so Grid List appears as its own page in the Material Demo.
- [x] Build runtime, samples, and editor projects to verify the implementation compiles cleanly.

## Verification
- Confirm `MatGridList` lays out items into the configured number of columns.
- Confirm gutter spacing is applied consistently horizontally and vertically.
- Confirm each tile instantiates the assigned `VisualTreeAsset` and binds to the tile item through `dataSource`.
- Confirm fixed row-height mode produces stable tile sizing.
- Confirm ratio-style row-height mode produces predictable square/rectangular tile sizing.
- Confirm light and dark themes both style the grid container/tile surface appropriately.
- Confirm the Grid List sample page can be opened from the Material Demo navigation.
- Build verification completed:
  - `dotnet build Sim.Faciem.Material.csproj`
  - `dotnet build Sim.Faciem.Material.Samples.csproj`
  - `dotnet build Sim.Faciem.Material.Editor.csproj`
