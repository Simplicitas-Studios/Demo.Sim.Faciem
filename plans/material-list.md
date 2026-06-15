# Plan: Sim.Faciem.Material list controls

## Context
- The next feature belongs to `Sim.Faciem.Material`.
- Goal: add Angular Material-inspired list controls, limited for now to:
  - `mat-list`
  - `mat-selection-list`
- You already decided the first version should be:
  - primarily **data-driven** using Unity's `ListView` model
  - **multi-select only** for `mat-selection-list`
  - included in the **Material demo/sample**
  - **template-driven** via an exposed `VisualTreeAsset` field that users can assign for row rendering, similar to `ListView` item templating
- The package already contains custom Material controls such as `MatButton`, `MatFormField`, and `MatSelect`, plus some earlier selection-related work (`MultiSelectionDropdown`).

## Approach
- Build the first version around Unity `ListView` unless implementation reveals a concrete blocker for Material styling or multi-selection behavior.
- Reuse existing package patterns:
  - `MatSelect.cs` for CSS constants, DOM/state conventions, and selected-state visuals
  - `BindableListView.cs` from core `Sim.Faciem` for existing `ListView`-binding ideas
  - existing Material style/theme injection flow (`MatEditorStyles.cs` + runtime `.tss` files)
- Keep the scope intentionally narrow: no nav-list, action-list, or other Angular variants in this first pass.
- Recommended implementation shape:
  - `MatList : ListView` as the base styled control
  - `MatSelectionList : MatList` (or a sibling built on the same helpers) configured for multi-selection behavior
  - expose a `VisualTreeAsset` item template property and instantiate it in `makeItem`
  - assign each instantiated item's `dataSource` to the corresponding item object in `bindItem` so normal UI Toolkit bindings inside the template can resolve against the row model
  - expose **selected indices** as the first public selection API, keeping item identity/equality concerns out of the initial version
  - support USS/theme tokens plus a demo page

## Files to modify
- `plans/material-list.md`
- Likely runtime controls:
  - `Packages/Sim.Faciem.Material/Runtime/Controls/MatList.cs`
  - `Packages/Sim.Faciem.Material/Runtime/Controls/MatSelectionList.cs`
  - possibly a small shared row/item helper only if template binding or selection-state decoration needs one
- Likely styling/theming:
  - `Packages/Sim.Faciem.Material/Runtime/Controls/Styles/MatListStyles.uss`
  - possibly `Packages/Sim.Faciem.Material/Runtime/Controls/Styles/MatListColors.uss` if list-specific tokens are introduced
  - `Packages/Sim.Faciem.Material/Runtime/Themes/Mat*.tss` to import the list stylesheet
  - `Packages/Sim.Faciem.Material/Runtime/Themes/Mat*.uss` if new list tokens are needed per theme
  - `Packages/Sim.Faciem.Material/Editor/MatEditorStyles.cs` to inject the new list stylesheet in editor windows
- Likely demo/sample files:
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/List/IListDemoDataContext.cs`
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/List/ListDemoViewModel.cs`
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/List/ListDemoView.uxml`
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/List/` item template UXML asset(s)
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/MatDemoWindowViewModel.cs`
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/IMatDemoWindowDataContext.cs`
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/MatDemoWindowView.uxml`
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/WellKnownMatDemoViewIds.cs`
  - corresponding view-id/editor assets if the sample page is added as a first-class demo section

## Reuse
- `Packages/Sim.Faciem.Material/Runtime/Controls/MatSelect.cs` for CSS constants, DOM/state patterns, and multi-selection conventions
- `Packages/Sim.Faciem/Runtime/Controls/BindableListView.cs` for existing `ListView` binding patterns in the broader Faciem ecosystem
- `Packages/Sim.Faciem.Material/Runtime/Controls/MultiSelectionDropdown.cs` and `MultiSelectionItem.cs` for prior multi-select/list-related ideas
- `Packages/Sim.Faciem.Material/Editor/MatEditorStyles.cs` for editor stylesheet injection work that a new list stylesheet must join
- Existing style/theme structure under `Packages/Sim.Faciem.Material/Runtime/Controls/Styles/` and `Runtime/Themes/`
- Existing Material demo page structure under `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/Select/`

## Steps
- [x] Inspect existing Material control and styling patterns relevant to lists.
- [x] Decide whether Unity `ListView` is the preferred starting point.
- [x] Define the public control surface for the first version:
  - data-driven item source shape
  - exposed `VisualTreeAsset` template API for item instantiation
  - **selected indices** as the initial bindable selection API
  - item data attached through `bindItem` / `VisualElement.dataSource` for template bindings
- [x] Add runtime list controls (`MatList`, `MatSelectionList`) in `Runtime/Controls/`.
- [x] Add list USS styling and list-specific fallback tokens.
- [x] Update theme imports and `MatEditorStyles.cs` so lists are styled in runtime panels and editor demo windows.
- [x] Add demo coverage showing:
  - plain `mat-list`
  - multi-select `mat-selection-list`
  - template-driven row rendering
  - selected-indices reflection
- [x] Define verification for visual behavior, selection behavior, editor styling, runtime theming, and sample navigation.

## Verification
- Confirm `mat-list` renders with Material list structure, spacing, hover/focus states, and row layout.
- Confirm assigned `VisualTreeAsset` templates are instantiated per row and receive the correct item object through `dataSource` so bindings resolve inside the template.
- Confirm `mat-selection-list` supports multi-selection and exposes the expected selected-indices state.
- Confirm selected rows receive Material selected-state visuals without inline styles.
- Confirm the editor demo window receives list styles via `MatEditorStyles`.
- Confirm runtime themes/TSS files include the new list stylesheet and any required tokens.
- Confirm the sample coverage exists inside the Material demo content.
- Confirm the chosen implementation does not violate existing Material package conventions (no inline styles, runtime/editor boundary respected).
- Build verification completed:
  - `dotnet build Sim.Faciem.Material.csproj`
  - `dotnet build Sim.Faciem.Material.Samples.csproj`
  - `dotnet build Sim.Faciem.Material.Editor.csproj`
