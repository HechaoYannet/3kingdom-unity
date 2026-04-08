# Unity 2022 LTS — Breaking Changes

**Last verified:** 2026-04-09

**Note:** Unity 2022 LTS is within the LLM's training data (cutoff May 2025). This document exists for completeness but contains minimal breaking changes since the model already knows this version.

## Known Breaking Changes (2022 LTS vs earlier versions)

### Input System (Optional Package)
The new Input System package is available but not required. Legacy Input Manager (`Input.*`) is still fully supported in Unity 2022 LTS.

### Entities/DOTS (Optional Package)
DOTS/Entities is available as a package but not integrated into core Unity. The GameObjectEntity pattern is still supported for those using DOTS.

### URP/HDRP
Render Pipeline improvements are backward compatible. Custom render passes using the old `ScriptableRenderPass.Execute` signature still work.

## Tuanjie Engine Note
Tuanjie Engine 1.5.3 maintains full Unity 2022 LTS API compatibility. No additional breaking changes are expected beyond standard Unity 2022 LTS behavior.