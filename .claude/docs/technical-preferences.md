# Technical Preferences

<!-- Populated by /setup-engine. Updated as the user makes decisions throughout development. -->
<!-- All agents reference this file for project-specific standards and conventions. -->

## Engine & Language

- **Engine**: Unity `2022.3.62t7` + Tuanjie Engine `1.8.5`
- **Language**: C#
- **Rendering**: URP (`com.unity.render-pipelines.universal` `14.1.0`) is active for the current battle-presentation vertical slice
- **Physics**: Unity built-in 3D physics, no custom project-wide physics route documented yet

## Input & Platform

<!-- Written by /setup-engine. Read by /ux-design, /ux-review, /test-setup, /team-ui, and /dev-story -->
<!-- to scope interaction specs, test helpers, and implementation to the correct input methods. -->

- **Target Platforms**: PC, Mobile
- **Input Methods**: Keyboard/Mouse, Touch
- **Primary Input**: Mixed (Keyboard/Mouse for PC, Touch for Mobile)
- **Gamepad Support**: Partial (recommended for PC)
- **Touch Support**: Full (for Mobile)
- **Platform Notes**: UI must adapt to both touch and mouse input. Mobile requires touch-friendly controls and responsive design.

## Naming Conventions

- **Classes**: PascalCase (e.g., `PlayerController`)
- **Public fields/properties**: PascalCase (e.g., `MoveSpeed`)
- **Private fields**: _camelCase (e.g., `_moveSpeed`)
- **Methods**: PascalCase (e.g., `TakeDamage()`)
- **Files**: PascalCase matching class (e.g., `PlayerController.cs`)
- **Scenes/Prefabs**: PascalCase (e.g., `MainMenu.unity`, `Player.prefab`)
- **Constants**: PascalCase or UPPER_SNAKE_CASE

## Performance Budgets

- **Target Framerate**: [TO BE CONFIGURED]
- **Frame Budget**: [TO BE CONFIGURED]
- **Draw Calls**: [TO BE CONFIGURED]
- **Memory Ceiling**: [TO BE CONFIGURED]

## Testing

- **Framework**: NUnit (Unity Test Framework)
- **Minimum Coverage**: [TO BE CONFIGURED]
- **Required Tests**: Balance formulas, deterministic gameplay systems, battle action pipeline boundaries, networking (if applicable)

## Forbidden Patterns

- AI-authored or runtime-nondeterministic systems must not own authoritative combat outcomes
- Do not couple new battle-presentation logic directly to scene-only side effects when a deterministic result payload can be emitted instead

## Allowed Libraries / Addons

- `com.unity.render-pipelines.universal` `14.1.0`
- `com.unity.cinemachine` `2.10.7`
- `com.unity.timeline` `1.7.7`
- `com.unity.visualeffectgraph` `14.1.0`
- `cn.tuanjie.ai.graph` `1.0.6`

## Architecture Decisions Log

- `Docs/architecture/card-system-architecture.md`
- `Docs/architecture/character-system-architecture.md`
- `Docs/architecture/battle-presentation-first-architecture.md`
- `Docs/architecture/battle-action-schema.md`

## Engine Specialists

<!-- Written by /setup-engine when engine is configured. -->
<!-- Read by /code-review, /architecture-decision, /architecture-review, and team skills -->
<!-- to know which specialist to spawn for engine-specific validation. -->

- **Primary**: unity-specialist
- **Language/Code Specialist**: unity-specialist
- **Shader Specialist**: unity-shader-specialist
- **UI Specialist**: unity-ui-specialist
- **Additional Specialists**: unity-dots-specialist, unity-addressables-specialist
- **Routing Notes**: Invoke primary for architecture and general C# code review. Invoke DOTS specialist for ECS/Jobs/Burst work. Invoke shader specialist for rendering and visual effects. Invoke UI specialist for interface implementation. Invoke Addressables specialist for asset management systems.

### File Extension Routing

| File Extension / Type | Specialist to Spawn |
|-----------------------|---------------------|
| Game code (.cs files) | unity-specialist |
| Shader / material files (.shader, .shadergraph, .mat) | unity-shader-specialist |
| UI / screen files (.uxml, .uss, Canvas prefabs) | unity-ui-specialist |
| Scene / prefab / level files (.unity, .prefab) | unity-specialist |
| Native extension / plugin files (.dll, native plugins) | unity-specialist |
| General architecture review | unity-specialist |
