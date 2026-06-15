# AI Framework Agents

This folder is the project-owned home for reusable coding agents and path-scoped instructions.

## Structure
- `ai/agents/` — task entry points and domain specialists
- `ai/instructions/` — coding conventions and `applyTo` path scopes

## Framework entry points
- `faciem-framework` — Sim.Faciem core, controls, editor, and internal editor
- `material-framework` — Sim.Faciem.Material controls, theming, editor, diagnostics, and demo
- `ugui-framework` — Sim.Faciem.uGUI runtime binding and editor tooling

## Specialist agents
Use the narrower specialist when the task is clearly inside one assembly or subdomain.

This `ai/` folder is the authoritative location for framework guidance in this repository.

The older `.github/agents` and `.github/instructions` files are kept as compatibility copies for now.
