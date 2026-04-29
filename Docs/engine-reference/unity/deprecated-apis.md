# Unity 2022 LTS — Deprecated APIs

**Last verified:** 2026-04-09

**Note:** Unity 2022 LTS is within the LLM's training data (cutoff May 2025). Most deprecated APIs listed here are already known to the model.

Quick lookup table for deprecated APIs and their replacements.
Format: **Don't use X** → **Use Y instead**

---

## Input

| Deprecated | Replacement | Notes |
|------------|-------------|-------|
| Legacy `OnMouse*` events | New Input System or `IPointer*Handler` | Better cross-platform support |
| `Input.GetKey()` (for complex input) | New Input System `InputAction` | More flexible input handling |

**Note:** Legacy Input Manager (`Input.*`) is still fully supported in Unity 2022 LTS. New Input System is optional.

---

## UI

| Deprecated | Replacement | Notes |
|------------|-------------|-------|
| Legacy `GUI` system (IMGUI) | UGUI or UI Toolkit | IMGUI is for editor tools only |
| `Text` component (for complex text) | `TextMeshPro` | Better rendering, more features |

**Note:** UGUI is fully supported and recommended for runtime UI in Unity 2022 LTS. UI Toolkit is available but not yet production-ready for all runtime use cases.

---

## DOTS/Entities (Optional Package)

| Deprecated | Replacement | Notes |
|------------|-------------|-------|
| Early DOTS preview APIs | Entities 0.51+ stable APIs | DOTS was in preview before 2022 LTS |

**Note:** DOTS/Entities is an optional package in Unity 2022 LTS, not integrated into core Unity.

---

## Tuanjie Engine Note

Tuanjie Engine 1.5.3 maintains full Unity 2022 LTS API compatibility. No additional API deprecations beyond standard Unity 2022 LTS.