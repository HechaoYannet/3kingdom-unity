# Adoption Plan

> **Generated**: 2026-04-27
> **Project phase**: Production
> **Engine**: Unity 2022 LTS (Tuanjie Engine 1.5.3 compatible)
> **Template version**: v1.0+

This plan reflects the repository state after the brownfield preparation pass completed on 2026-04-27. The highest-impact compatibility gaps have been reduced, but some workflow-critical artifacts still need follow-through.

---

## Step 1: Resolve Remaining High-Priority Gaps

### 1.1 Add missing foundation GDDs
Problem: Event, Data Management, and Asset Bundle systems still lack first-pass GDDs, which blocks full design coverage and story generation.
Fix: Run `/reverse-document design Assets/Scripts/AssetBundleFramework`, `/reverse-document design Assets/Scripts/Data`, and `/reverse-document design Assets/Scripts/Event`
Time: 1 session each
- [ ] Foundation GDDs created

### 1.2 Add missing Role and UI ADRs
Problem: Role and UI systems have GDDs but still lack dedicated ADR coverage.
Fix: Run `/architecture-decision` for role-system and ui-system architecture
Time: 1 session each
- [ ] Role ADR created
- [ ] UI ADR created

### 1.3 Turn scaffolded tests into real tests
Problem: Test infrastructure exists, but there are no executable EditMode/PlayMode tests yet.
Fix: Add at least three EditMode tests under `tests/EditMode/`
Time: 1 session
- [ ] First EditMode tests added

---

## Step 2: Bootstrap the Remaining Architecture Workflow

### 2.1 Populate the architecture registry from accepted ADRs
Problem: `Docs/registry/architecture.yaml` exists but does not yet encode the project's real stances.
Fix: After ADRs are reviewed, update the registry with accepted state ownership and interface contracts.
Time: 30 min
- [ ] Registry populated

### 2.2 Create the control manifest
Problem: `Docs/architecture/control-manifest.md` is still missing, so story generation and implementation constraints are not centralized.
Fix: Run `/create-control-manifest` after the key ADR set is in place
Time: 30 min
- [ ] Control manifest created

### 2.3 Build the master architecture document
Problem: ADRs exist in isolation; there is no whole-project architecture blueprint.
Fix: Run `/create-architecture`
Time: 1 session
- [ ] Master architecture document created

---

## Step 3: Convert Prepared Artifacts into Execution Artifacts

### 3.1 Move current GDDs from `In Review` to `Approved`
Problem: Existing GDDs are present but not yet approved, so they should not drive final story generation.
Fix: Run `/design-review` on each current GDD
Time: 30 min each
- [ ] Card GDD reviewed
- [ ] Character GDD reviewed
- [ ] Role GDD reviewed
- [ ] UI GDD reviewed

### 3.2 Create epics and stories
Problem: Sprint tracking exists, but there is no feature story tree yet.
Fix: Run `/create-epics`, then `/create-stories`
Time: 1-2 sessions
- [ ] First epic set created
- [ ] First story set created

---

## Step 4: Optional Improvements

### 4.1 Add a top-level game concept document
Problem: Systems are documented, but the higher-level player promise is not.
Fix: Capture `design/gdd/game-concept.md` and optionally a pillars document
Time: 1 session
- [ ] Game concept documented

### 4.2 Document the login/home flow as a separate UX or UI spec
Problem: Entry flow code exists but is not tracked as a design artifact.
Fix: Use `/ux-design` or `/reverse-document design` for the login/home flow
Time: 1 session
- [ ] Entry-flow spec documented

---

## Re-run

After completing Step 2, re-run the brownfield audit or a targeted workflow check to confirm the remaining high-priority gaps are resolved.
