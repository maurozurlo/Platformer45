# Technical Preferences

<!-- Populated by /setup-engine. Updated as the user makes decisions throughout development. -->
<!-- All agents reference this file for project-specific standards and conventions. -->

## Engine & Language

- **Engine**: Unity 2022.3.62f3 (LTS)
- **Language**: C#
- **Rendering**: Built-in Render Pipeline (URP/HDRP not installed)
- **Physics**: Built-in 3D physics (PhysX); 2D physics module also available. Player uses Rigidbody (see `PlayerPlatformerRB.cs`).

## Input & Platform

<!-- Written by /setup-engine. Read by /ux-design, /ux-review, /test-setup, /team-ui, and /dev-story -->
<!-- to scope interaction specs, test helpers, and implementation to the correct input methods. -->

- **Target Platforms**: PC (Steam / itch.io)
- **Input Methods**: Keyboard/Mouse
- **Primary Input**: Keyboard/Mouse
- **Gamepad Support**: Partial (recommended — not yet implemented; current code is keyboard-only via legacy Input Manager)
- **Touch Support**: None
- **Platform Notes**: Current input uses the **legacy Input Manager** (`Input.GetAxisRaw`, `Input.GetButtonDown`, `KeyCode`). The new Input System package is not installed. Adding gamepad support later means either extending Input Manager axis/button bindings or migrating to the new Input System.

## Naming Conventions

<!-- Unity C# defaults. -->
- **Classes**: PascalCase (e.g., `PlayerController`) — file name matches class
- **Public fields/properties**: PascalCase (e.g., `MoveSpeed`)
- **Private fields**: `_camelCase` (e.g., `_moveSpeed`)
- **Methods**: PascalCase (e.g., `TakeDamage()`)
- **Events/Delegates**: PascalCase (e.g., `HealthChanged`)
- **Files**: PascalCase matching class (e.g., `PlayerController.cs`)
- **Scenes/Prefabs**: PascalCase (e.g., `PlayerController.prefab`)
- **Constants**: PascalCase or UPPER_SNAKE_CASE

## Performance Budgets

- **Target Framerate**: 60 FPS
- **Frame Budget**: 16.6 ms
- **Draw Calls**: < 1000 (desktop; tighten if targeting low-end hardware)
- **Memory Ceiling**: [TO BE CONFIGURED — set once target hardware is known]

## Testing

- **Framework**: Unity Test Framework (NUnit) — `com.unity.test-framework` 1.1.33 (installed)
- **Minimum Coverage**: [TO BE CONFIGURED]
- **Required Tests**: Balance formulas, gameplay systems, networking (if applicable)

## Forbidden Patterns

<!-- Add patterns that should never appear in this project's codebase -->
- [None configured yet — add as architectural decisions are made]

## Allowed Libraries / Addons

<!-- Add approved third-party dependencies here -->
- [None configured yet — add as dependencies are approved]

## Architecture Decisions Log

<!-- Quick reference linking to full ADRs in docs/architecture/ -->
- [No ADRs yet — use /architecture-decision to create one]

## Engine Specialists

<!-- Written by /setup-engine when engine is configured. -->
<!-- Read by /code-review, /architecture-decision, /architecture-review, and team skills -->
<!-- to know which specialist to spawn for engine-specific validation. -->

- **Primary**: unity-specialist
- **Language/Code Specialist**: unity-specialist (C# review — primary covers it)
- **Shader Specialist**: unity-shader-specialist (Shader Graph, HLSL, URP/HDRP materials)
- **UI Specialist**: unity-ui-specialist (UI Toolkit UXML/USS, UGUI Canvas, runtime UI)
- **Additional Specialists**: unity-dots-specialist (ECS, Jobs system, Burst compiler), unity-addressables-specialist (asset loading, memory management, content catalogs)
- **Routing Notes**: Invoke primary for architecture and general C# code review. Invoke DOTS specialist for any ECS/Jobs/Burst code (note: DOTS not currently installed). Invoke shader specialist for rendering and visual effects. Invoke UI specialist for all interface implementation (this project uses UGUI + TextMeshPro, not UI Toolkit). Invoke Addressables specialist for asset management systems (note: Addressables not currently installed).

### File Extension Routing

<!-- Skills use this table to select the right specialist per file type. -->
<!-- If a row says [TO BE CONFIGURED], fall back to Primary for that file type. -->

| File Extension / Type | Specialist to Spawn |
|-----------------------|---------------------|
| Game code (.cs files) | unity-specialist |
| Shader / material files (.shader, .shadergraph, .mat) | unity-shader-specialist |
| UI / screen files (.uxml, .uss, Canvas prefabs) | unity-ui-specialist |
| Scene / prefab / level files (.unity, .prefab) | unity-specialist |
| Native extension / plugin files (.dll, native plugins) | unity-specialist |
| General architecture review | unity-specialist |
