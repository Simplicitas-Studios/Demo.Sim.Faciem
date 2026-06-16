# Plan: Material Demo icon browser

## Context
- Add a new **Icon** menu item to the Material Demo window navigation, positioned after **Theming**.
- The new demo page should show a search bar at the top and a vertical `MatGridList` below it.
- The grid should surface the SVG icons shipped in `Packages/com.sim.faciem-material/Icons/fontawesome-free-7.2.0/`.
- The source scope is **both** Font Awesome SVG folders in that package (`svgs` and `svgs-full`).
- Search should filter by icon file name and only show icons whose names contain the typed text.
- Search matching should be **case-insensitive**.
- Each tile should show **icon preview + file name**.
- The icon set is large (`~5720` SVG files), so the page should lean on the existing `MatGridList` virtualization pattern rather than rendering everything naïvely.

## Approach
- Add a first-class Material Demo page for icons, wired into the existing left-nav shell and `WellKnownMatDemoViewIds` navigation flow.
- Reuse the sample-page pattern already used by `GridList`, `List`, `Select`, and `Theming`:
  - page-specific data-context interface
  - page-specific view model
  - page UXML
  - runtime + editor view-id assets
- Populate the icon dataset from **both** Font Awesome SVG package folders (`svgs` and `svgs-full`) and expose one combined filtered list for binding into `MatGridList`.
- Bind a top search field two-way to the page view model and recalculate the filtered item list whenever the query changes.
- Perform filtering against file names using **case-insensitive substring matching**.
- Use a tile template so each grid item shows the icon preview and its file name.
- Include enough metadata per item to avoid name collisions between folders/styles when duplicate file names exist.

## Files to modify
- Demo shell/navigation:
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/IMatDemoWindowDataContext.cs`
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/MatDemoWindowViewModel.cs`
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/MatDemoWindowView.uxml`
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/WellKnownMatDemoViewIds.cs`
  - matching runtime/editor view-id assets for the new page
- New icon page (new files under `.../Runtime/Icon/` and `.../Editor/Icon/`)
- Shared sample styling:
  - `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/MatDemoStyles.uss`

## Reuse
- `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/MatDemoWindowViewModel.cs`
  - existing nav-index and page navigation pattern
- `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/GridList/GridListDemoView.uxml`
  - existing `MatGridList` binding pattern and vertical layout usage
- `Packages/Sim.Faciem.Material/Samples/MaterialDemo/Runtime/GridList/GridListDemoViewModel.cs`
  - existing bindable item-model pattern for grid tiles
- `Packages/Sim.Faciem.uGUI/Editor/BindingWindow/View/BindingWindowView.uxml`
  - example of two-way `value` binding on an input control
- `Packages/Sim.Faciem.Material/Editor/MatEditorStyles.cs`
  - package-root path convention (`Packages/com.sim.faciem-material/...`) likely relevant when locating icon assets

## Steps
- [x] Confirm the icon source scope: include both `svgs` and `svgs-full`.
- [x] Confirm tile content: icon preview + file name.
- [x] Confirm search behavior: case-insensitive substring matching.
- [ ] Inspect existing sample/runtime constraints to choose the safest icon-loading path for the editor demo page.
- [ ] Add the new Icon page to the Material Demo shell and navigation order.
- [ ] Create the icon demo data context, view model, UXML view, and item template.
- [ ] Implement search-query state and filtered item-source updates.
- [ ] Implement icon asset discovery and per-tile preview/name binding.
- [ ] Add shared USS rules for the search row and icon tiles.
- [ ] Add runtime + editor view-id assets so the page is discoverable in the demo window.
- [ ] Verify nav, filtering, scrolling, and icon rendering with the combined dataset.

## Verification
- Open `Sim/Faciem/Material Demo` and confirm **Icon** appears directly after **Theming**.
- Navigate to the Icon page and confirm the top search field is visible.
- Confirm the grid initially shows the available icons from **both** `svgs` and `svgs-full`.
- Confirm each tile shows the icon preview and file name.
- Type partial file names (for example `adjust`, `air`, `calendar`) and confirm only matching icons remain.
- Confirm matching is case-insensitive.
- Confirm scrolling remains responsive with the full combined dataset.
- Confirm icons and labels stay correctly paired while scrolling/recycling.
- Build verification after implementation:
  - `dotnet build Sim.Faciem.Material.Samples.csproj`
  - `dotnet build Sim.Faciem.Material.Samples.Editor.csproj`
