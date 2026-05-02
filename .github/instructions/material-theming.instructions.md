---
description: "Use when writing, editing, or reviewing USS theme files or control style sheets for Sim.Faciem.Material. Covers Material Design v3 token naming, theme scoping, USS variable conventions, and the pre-built theme catalogue."
applyTo: ["Packages/Sim.Faciem.Material/Runtime/Themes/**", "Packages/Sim.Faciem.Material/Runtime/Controls/Styles/**"]
---
# Material Theming — USS Conventions

## File Organization
- Theme files live in `Runtime/Themes/` and are named `Mat{ThemeName}Theme.uss`.
- Component-level styles live in `Runtime/Controls/Styles/` and are named `mat-{component}.uss`.
- Each theme file declares variables on both `:root` (for PanelSettings-wide scope) and `.mat-theme--{kebab-name}` (for scoped subtree usage).

## Variable Naming — `--mat-{component}-{role}-{property}`
```css
/* Component */    --mat-button-...
/* Role */         -primary- | -accent- | -warn- | -disabled- | -surface-
/* Property */     -color | -bg | -ripple | -text-color | -hover-overlay
```
- Always define the full triad: `-color`, `-ripple`, and `-text-color` for every role.
- Disabled state always provides both `-disabled-color` and `-disabled-bg`.
- Surface tokens (`-surface-bg`, `-on-surface`, `-hover-overlay`, `-focus-overlay`, `-active-overlay`) are required in every theme.

## Hard-coded Colors
- **Never** put literal hex or rgba values inside component USS files.
- Component USS files **must** reference variables only: `background-color: var(--mat-button-primary-color);`
- Literal values are only permitted inside theme files (the source-of-truth for the palette).

## Theme Scoping Pattern
```css
:root,
.mat-theme--{kebab-name} {
    --mat-button-primary-color: #HEX;
    /* ... */
}
```
- `:root` ensures the theme applies when attached at the PanelSettings level via `.tss`.
- `.mat-theme--{name}` enables scoped subtree theming without a full panel switch.

## Editor Themes
- Editor themes are named `MatEditor{Light|Dark}Theme.uss` and live in `Runtime/Themes/`.
- They extend the base token set with editor-specific surface colors (compatible with `EditorGUIUtility.isProSkin`).
- `MatEditorStyles.ApplyTo()` in the Editor assembly selects the correct file at runtime.

## TSS Files
- Each theme has a companion `.tss` file in the same folder that imports the `uss` and any control style sheets.
- Never import editor-only USS inside a runtime `.tss`.

## Do Not
- Do not write C# or UXML in theme files.
- Do not add `transition` or animation properties without a corresponding `prefers-reduced-motion` guard.
- Do not duplicate variable declarations across theme files — extend, don't repeat.
