---
description: "Use when diagnosing visual bugs, USS token issues, styling conflicts, or control state problems in Sim.Faciem.Material. Covers token resolution order, Unity USS constraints, BEM class naming, common failure patterns, and handoff routing for MatButton, MatSelect, MatFormField, MatOption, and all Mat-prefixed controls."
---

# Material Design Diagnostics — Domain Knowledge

Reference document for the `material-diagnostics` agent. Contains the structural facts, constraints, and known failure patterns needed to diagnose visual bugs in `Sim.Faciem.Material`.

---

## Package Structure

```
Packages/Sim.Faciem.Material/
├── Runtime/
│   ├── Controls/
│   │   ├── MatButton.cs
│   │   ├── MatSelect.cs
│   │   ├── MatFormField.cs
│   │   ├── MatOption.cs
│   │   └── Styles/
│   │       ├── MatButtonStyles.uss      ← structural + state rules
│   │       ├── MatButtonColors.uss      ← CSS custom property declarations (default fallbacks)
│   │       ├── MatSelectStyles.uss
│   │       ├── MatSelectColors.uss
│   │       ├── MatFormFieldStyles.uss
│   │       └── MatFormFieldColors.uss
│   └── Themes/
│       ├── MatIndigoTheme.uss           ← runtime theme (light, primary #3F51B5)
│       ├── MatDeepPurpleTheme.uss       ← runtime theme (light, primary #673AB7)
│       ├── MatPinkBlueGreyTheme.uss     ← runtime theme (dark, primary #E91E63)
│       ├── MatPurpleGreenTheme.uss      ← runtime theme (dark, primary #9C27B0)
│       ├── MatEditorLightTheme.uss      ← editor skin (Unity Personal, primary #1565C0)
│       └── MatEditorDarkTheme.uss       ← editor skin (Unity Professional, primary #42A5F5)
└── Editor/
    └── MatEditorStyles.cs               ← injects stylesheets into editor window rootVisualElement
```

---

## USS Token Resolution Order

Unity UI Toolkit resolves CSS custom properties by declaration order — the **last** declaration wins within the same scope.

`MatEditorStyles.cs` injects sheets in this order:

1. `MatButtonStyles.uss`
2. `MatFormFieldStyles.uss`
3. `MatSelectStyles.uss`
4. One runtime theme (Indigo / DeepPurple / PinkBlueGrey / PurpleGreen) — selected by user preference

> **`MatEditorLightTheme.uss` and `MatEditorDarkTheme.uss` are NOT injected by `MatEditorStyles.cs`.** These files exist for alternate injection paths only. When a token is missing from the active runtime theme, the fallback in `*Colors.uss` applies — not the editor theme file.

Cross-alias chain (inside `*Colors.uss`):
```
--mat-select-trigger-text-color: var(--mat-button-on-surface, rgba(0,0,0,0.87))
--mat-select-placeholder-text-color: var(--mat-form-field-label-color, rgba(0,0,0,0.42))
```
If `--mat-button-on-surface` is missing from the active theme, the literal fallback `rgba(0,0,0,0.87)` is used — which may be wrong on dark themes.

---

## Unity USS Constraints

These are hard limitations of Unity UI Toolkit's USS parser. Violations cause silent rendering failures or console errors.

| Property | Allowed Values | Forbidden Values | Error |
|----------|---------------|-----------------|-------|
| `cursor` | `arrow`, `link`, `resize-*`, `move`, `zoom-in`, `zoom-out`, `not-allowed`, `split-resize-*` | `default`, `inherit`, `auto`, `pointer` | `cursor: default` → parse warning; `cursor: inherit` → `Enum while reading Keyword` |
| `transition` | All standard CSS | — | No runtime error but `prefers-reduced-motion` not respected |
| `background-color` on `:hover` | Works | Opacity-only hover via `opacity` on root loses all children | Use `background-color: var(--mat-...-hover-bg)` |
| `position: Absolute` panel | Must set explicit `width`/`height` or `max-height`/`overflow:hidden` | `ScrollView` as overlay collapses `contentViewport` to zero height | Options invisible |

---

## BEM Class Naming Convention

Pattern: `mat-{component}__{element}--{modifier}`

| Component | State modifier class | C# constant |
|-----------|---------------------|-------------|
| `MatButton` | `mat-button--raised`, `mat-button--flat`, `mat-button--stroked`, `mat-button--fab` | `RaisedClassName` etc. |
| `MatButton` | `mat-button--disabled` | `DisabledClassName` |
| `MatSelect` | `mat-select--open` | `OpenClassName` |
| `MatSelect` | `mat-select--disabled` | `DisabledClassName` |
| `MatFormField` | `mat-form-field--focused` | `FocusedClassName` |
| `MatFormField` | `mat-form-field--has-value` | `HasValueClassName` |
| `MatOption` | `mat-option--selected` | `SelectedClassName` |
| `MatOption` | `mat-option--disabled` | `DisabledClassName` |

Class name constants are declared as `public const string {Role}ClassName` in each control class.

---

## Token Naming Scheme

```
--mat-{component}-{role}-{property}

Examples:
--mat-button-primary-color          ← background for primary raised button
--mat-button-primary-text-color     ← foreground text for primary raised button
--mat-button-primary-ripple         ← ripple/overlay color for primary raised
--mat-button-surface-bg             ← surface background (flat/stroked)
--mat-button-on-surface             ← text color on surface
--mat-button-hover-overlay          ← hover state layer (surface buttons)
--mat-button-focus-overlay          ← focus state layer
--mat-form-field-focus-color        ← label + underline/outline color when focused/open
--mat-form-field-label-color        ← floating label color at rest
--mat-select-panel-background       ← dropdown panel background
--mat-option-selected-state-label-text-color  ← selected option text
```

---

## Angular Material MDC State Layer Model

| State | Overlay opacity | How it appears in USS |
|-------|----------------|----------------------|
| Hover | 8% of on-surface | `--mat-button-{role}-hover-bg` (pre-blended) |
| Focus | 24% of on-surface | `--mat-button-{role}-focus-bg` (pre-blended) |
| Active/Pressed | 24% | same as focus |
| Disabled text | 38% opacity | `--mat-button-disabled-color: rgba(..., 0.38)` |

Unity does not support CSS `::after` pseudo-elements as overlay layers. State layers are implemented as pre-blended background-color tokens.

---

## MatSelect Architecture

```
MatSelect (VisualElement)
└── MatFormField (_formField)
    ├── Infix slot
    │   ├── .mat-select__trigger (_trigger)
    │   │   ├── .mat-select__value (.mat-select__value-text)
    │   │   └── .mat-select__arrow-wrapper → .mat-select__arrow
    │   └── (label, underline, outline managed by MatFormField)
    └── .mat-form-field__subscript (_subscript)

Overlay panel (_panel: VisualElement, appended to _overlayRoot)
└── MatOption × N
```

Key implementation facts:
- Click subscription is on `_formField`, not `_trigger` — to capture the full form-field surface.
- `mat-form-field__subscript` clicks are excluded via ancestry walk in `OnFormFieldPointerDown`.
- `_overlayRoot` is found by `FindThemedOverlayRoot()` — first element in `visualTree` with `styleSheets.count > 0`.
- Panel position: `_overlayRoot.WorldToLocal(new Vector2(wb.x, wb.yMax))` where `wb = _trigger.worldBound`.

---

## Common Failure Patterns (Quick Reference)

| Symptom | Root Cause | File to Fix | Agent |
|---------|-----------|-------------|-------|
| Color ignores theme in editor window | Token missing from active runtime theme USS | `Runtime/Themes/Mat*Theme.uss` | `@material-theming` |
| Open-state label color doesn't change | `.mat-select.mat-select--open` selector missing in `MatSelectStyles.uss`, or `--mat-form-field-focus-color` missing from theme | `MatSelectStyles.uss` + theme | `@material-theming` |
| Hover/focus bg ignores theme | `opacity`-based hover rule; `--mat-button-{role}-hover-bg` token missing from theme | theme + `MatButtonColors.uss` | `@material-theming` |
| `cursor: default` USS parse warning | `default` not a valid Unity USS cursor value | component `*Styles.uss` | `@material-theming` |
| `Enum while reading Keyword` error | `cursor: inherit` in USS | component `*Styles.uss` | `@material-theming` |
| MatSelect options invisible (zero height) | Panel is `ScrollView`; `contentViewport` collapses under `position: Absolute` | `MatSelect.cs` + `MatSelectStyles.uss` | `@material-controls` + `@material-theming` |
| MatSelect click only on arrow icon | Pointer subscription on `_trigger` not `_formField` | `MatSelect.cs` | `@material-controls` |
| MatSelect panel misaligned | Wrong coordinate transform; use `WorldToLocal` not `ChangeCoordinatesTo` | `MatSelect.cs` | `@material-controls` |
| Wrong surface color on dark theme | `--mat-button-on-surface` not declared for dark in theme file; fallback `rgba(0,0,0,0.87)` used instead of white | theme file | `@material-theming` |
