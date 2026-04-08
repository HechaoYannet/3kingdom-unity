# Unity 2022 LTS — Optional Packages & Systems

**Last verified:** 2026-04-09

This document indexes **optional packages and systems** available in Unity 2022 LTS.
These are NOT part of the core engine but are commonly used for specific game types.

---

## How to Use This Guide

**✅ Detailed Documentation Available** - See `plugins/` directory for comprehensive guides
**🟡 Brief Overview Only** - Links to official docs, use WebSearch for details
**⚠️ Preview** - May have breaking changes in future versions
**📦 Package Required** - Install via Package Manager

---

## Production-Ready Packages

### ✅ Cinemachine
- **Purpose:** Virtual camera system (dynamic cameras, cutscenes, camera blending)
- **When to use:** 3rd person games, cinematics, complex camera behavior
- **Version:** Cinemachine 2.8.x (in Unity 2022 LTS)
- **Status:** Production-Ready
- **Package:** `com.unity.cinemachine` (Package Manager)
- **Official:** https://docs.unity3d.com/Packages/com.unity.cinemachine@2.8/manual/index.html

---

### ✅ Addressables
- **Purpose:** Advanced asset management (async loading, remote content, memory control)
- **When to use:** Large projects, DLC, remote content delivery
- **Version:** Addressables 1.19+ (in Unity 2022 LTS)
- **Status:** Production-Ready
- **Package:** `com.unity.addressables` (Package Manager)
- **Official:** https://docs.unity3d.com/Packages/com.unity.addressables@1.19/manual/index.html

---

### ✅ New Input System
- **Purpose:** Modern input handling system
- **When to use:** Complex input needs, multiple input devices, rebindable controls
- **Version:** Input System 1.4+ (in Unity 2022 LTS)
- **Status:** Production-Ready (but optional)
- **Package:** `com.unity.inputsystem` (Package Manager)
- **Note:** Legacy Input Manager is still fully supported
- **Official:** https://docs.unity3d.com/Packages/com.unity.inputsystem@1.4/manual/index.html

---

### ⚠️ DOTS / Entities (ECS)
- **Purpose:** Data-Oriented Technology Stack (high-performance ECS)
- **When to use:** Games with 1000s of entities, RTS, simulations
- **Version:** Entities 0.51+ (in Unity 2022 LTS)
- **Status:** Preview (not production-ready in 2022 LTS)
- **Package:** `com.unity.entities` (Package Manager)
- **Note:** Considered preview, major changes expected in future versions
- **Official:** https://docs.unity3d.com/Packages/com.unity.entities@0.51/manual/index.html

---

### ✅ 2D Packages
- **2D Animation:** `com.unity.2d.animation`
- **2D Pixel Perfect:** `com.unity.2d.pixel-perfect`
- **2D Tilemap Editor:** `com.unity.2d.tilemap`
- **Status:** Production-Ready for 2D games
- **Package:** Install via Package Manager

---

### ✅ UI Toolkit
- **Purpose:** Modern UI system (originally for Editor, now for runtime)
- **When to use:** Complex UI needs, designer-friendly workflow
- **Version:** UI Toolkit 1.0+ (in Unity 2022 LTS)
- **Status:** Production-Ready for Editor, Preview for runtime
- **Package:** Built-in (no separate package)
- **Note:** UGUI is still primary runtime UI system in 2022 LTS
- **Official:** https://docs.unity3d.com/Manual/UIElements.html

---

## Tuanjie Engine Note

Tuanjie Engine 1.5.3 includes all standard Unity 2022 LTS packages. China-specific packages or optimizations may also be available.