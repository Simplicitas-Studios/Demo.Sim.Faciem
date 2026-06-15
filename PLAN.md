# Plan: framework-specific coding agents

## Context
- The demo project contains three framework packages developed side-by-side as Git submodules under `Packages/`: `Sim.Faciem`, `Sim.Faciem.Material`, and `Sim.Faciem.uGUI`.
- The repository already has agent/instruction markdown under `.github/agents/` and `.github/instructions/`, but you want the useful skills/agents moved into a dedicated folder structure owned by this project instead of keeping them under `.github`.
- Current coverage is uneven:
  - `Sim.Faciem` already has `faciem-core`, `faciem-controls`, `faciem-editor`, and `faciem-internal-editor` guidance.
  - `Sim.Faciem.Material` already has `material-controls`, `material-editor`, `material-theming`, `material-demo`, and `material-diagnostics` guidance.
  - `Sim.Faciem.uGUI` currently has no matching agent/instruction guidance.
- Goal: create a uniform, project-owned agent/skill structure for all three frameworks.

## Approach
- Create a **project-owned folder structure** for reusable AI guidance instead of extending `.github/` further.
- Recommended structure:
  - `ai/agents/` for agent entry points
  - `ai/instructions/` for path-scoped coding conventions / domain guidance
- Migrate or duplicate the currently useful `.github` guidance into that new structure, then normalize it around **one top-level framework agent per package** plus specialist agents where finer routing is helpful.
- Use the actual package/assembly boundaries as the source of truth:
  - `Packages/Sim.Faciem`: runtime, shared, editor, internal editor, and controls asmdefs.
  - `Packages/Sim.Faciem.Material`: runtime, editor, themes/styles, diagnostics, and samples asmdefs/folders.
  - `Packages/Sim.Faciem.uGUI`: runtime and editor asmdefs.
- Recommended end state:
  - Framework umbrellas for `Sim.Faciem`, `Sim.Faciem.Material`, and `Sim.Faciem.uGUI`.
  - Preserved specialist coverage for Faciem and Material, relocated into the new `ai/` structure.
  - New uGUI specialist coverage added in the same structure.

## Files to modify
- `PLAN.md`
- New project-owned guidance root:
  - `ai/agents/`
  - `ai/instructions/`
- Framework umbrella guidance:
  - `ai/agents/faciem-framework.agent.md`
  - `ai/instructions/faciem-framework.instructions.md`
  - `ai/agents/material-framework.agent.md`
  - `ai/instructions/material-framework.instructions.md`
  - `ai/agents/ugui-framework.agent.md`
  - `ai/instructions/ugui-framework.instructions.md`
- Relocated or recreated specialist guidance in the new structure:
  - `ai/agents/faciem-core.agent.md`
  - `ai/agents/faciem-controls.agent.md`
  - `ai/agents/faciem-editor.agent.md`
  - `ai/agents/faciem-internal-editor.agent.md`
  - `ai/agents/material-controls.agent.md`
  - `ai/agents/material-editor.agent.md`
  - `ai/agents/material-theming.agent.md` (if you want an agent entry point matching the existing instruction set)
  - `ai/agents/material-demo.agent.md`
  - `ai/agents/material-diagnostics.agent.md`
  - `ai/agents/ugui-runtime.agent.md`
  - `ai/agents/ugui-editor.agent.md`
  - matching `ai/instructions/*.instructions.md` files for each active specialist
- Existing `.github` markdown may also need cleanup, deprecation notes, or removal after the new `ai/` structure is in place.

## Reuse
- Existing agent format in `.github/agents/faciem-core.agent.md`, `.github/agents/material-controls.agent.md`, `.github/agents/material-editor.agent.md`, and `.github/agents/material-diagnostics.agent.md`
- Existing instruction format in `.github/instructions/faciem-core.instructions.md`, `.github/instructions/material-controls.instructions.md`, `.github/instructions/material-editor.instructions.md`, and `.github/instructions/material-diagnostics.instructions.md`
- Existing `applyTo` scoping patterns in `.github/instructions/*.md`
- `Packages/Sim.Faciem.uGUI/README.md` for package-specific concepts and terminology
- uGUI implementation anchors such as:
  - `Packages/Sim.Faciem.uGUI/Runtime/SimAutoBindingComponent.cs`
  - `Packages/Sim.Faciem.uGUI/Runtime/SimDataSourceMonoBehaviour.cs`
  - `Packages/Sim.Faciem.uGUI/Runtime/Binding/SimBindingFactory.cs`
  - `Packages/Sim.Faciem.uGUI/Editor/BindingWindow/ViewModel/BindingWindowViewModel.cs`

## Steps
- [x] Inventory current framework/package structure and existing guidance files.
- [x] Identify missing or mismatched framework coverage.
- [x] Decide on uniform framework entry points plus specialists.
- [x] Choose a project-owned home for AI guidance (`ai/agents` + `ai/instructions`).
- [x] Migrate existing useful Faciem and Material guidance from `.github/` into `ai/`, preserving content but normalizing naming and cross-references.
- [x] Implement framework umbrella guidance:
  - [x] `faciem-framework` should cover package-level routing across core, controls, editor, and internal editor.
  - [x] `material-framework` should cover routing across controls, theming, editor, demo, and diagnostics.
  - [x] `ugui-framework` should cover routing across runtime binding infrastructure and editor tooling.
- [x] Implement uGUI specialists:
  - [x] `ugui-runtime` for `Packages/Sim.Faciem.uGUI/Runtime/**`
  - [x] `ugui-editor` for `Packages/Sim.Faciem.uGUI/Editor/**`
- [x] Add matching `ai/instructions/*.instructions.md` files with `applyTo` scopes for every active specialist.
- [x] Decide whether `.github/agents` and `.github/instructions` should be deleted, left as compatibility copies, or replaced with short pointers to the new `ai/` location.
- [x] Verify naming, scopes, `applyTo` patterns, and constraints align with actual package boundaries.

## Verification
- Confirm the new `ai/` folder contains the active guidance set and is organized predictably by `agents/` and `instructions/`.
- Confirm each framework has a consistent top-level entry point:
  - `faciem-framework`
  - `material-framework`
  - `ugui-framework`
- Confirm any uGUI guidance correctly separates runtime (`Packages/Sim.Faciem.uGUI/Runtime/**`) from editor (`Packages/Sim.Faciem.uGUI/Editor/**`) concerns.
- Confirm umbrella agents do not blur assembly boundaries; they should route and constrain work, while specialists remain responsible for subdomains.
- Confirm referenced paths/assemblies match the package structure in `Packages/`.
- Confirm any retained `.github` copies are clearly marked as compatibility/deprecated so there is one authoritative source of truth. ✅ Added `.github/agents/README.md` and `.github/instructions/README.md` pointing to `ai/`.
