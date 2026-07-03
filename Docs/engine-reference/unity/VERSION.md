# Unity Engine — Version Reference

| Field | Value |
|-------|-------|
| **Engine Version** | Unity 2022.3.62f3 (LTS) |
| **Project Pinned** | 2026-07-03 |
| **LLM Knowledge Cutoff** | May 2025 |
| **Risk Level** | LOW — version is within LLM training data |

## Note

Unity 2022.3 LTS is within the LLM's training data. Engine reference docs are
optional but can be added later if agents suggest incorrect APIs.

Run `/setup-engine refresh` to populate full reference docs at any time, or
`/setup-engine upgrade 2022.3.62f3 <new-version>` if the project migrates to a
newer Unity release (e.g. Unity 6.x), which WOULD introduce a knowledge gap.

## Project Facts (verified against project files, 2026-07-03)

- **Input**: Legacy Input Manager (`Input.GetAxisRaw`, `Input.GetButtonDown`,
  `KeyCode`). The new Input System package is NOT installed.
- **UI**: uGUI + TextMeshPro (`com.unity.ugui`, `com.unity.textmeshpro`). UI
  Toolkit is not used for runtime UI.
- **Camera**: Cinemachine **2.10.3** (`com.unity.cinemachine`) — 2.x API, NOT
  the 3.0+ API line introduced with Unity 6.
- **2D**: `com.unity.2d.sprite`, `com.unity.2d.tilemap`.
- **Navigation**: `com.unity.ai.navigation` 1.1.6.
- **Not installed**: Addressables, DOTS/Entities, URP/HDRP (built-in render
  pipeline), new Input System.
- **Tooling**: `com.unity.test-framework` 1.1.33 (NUnit-based), Unity MCP
  bridge (`com.coplaydev.unity-mcp`).

## Verified Sources

- Official manual (2022.3 LTS): https://docs.unity3d.com/2022.3/Documentation/Manual/index.html
- Script reference (2022.3 LTS): https://docs.unity3d.com/2022.3/Documentation/ScriptReference/index.html
