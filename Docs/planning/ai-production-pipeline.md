# AI-First Production Pipeline

**Status**: Draft
**Created**: 2026-04-27
**Purpose**: Define how AI should accelerate this project without becoming the authoritative runtime owner of deterministic combat behavior.

## Principles

1. AI is for acceleration, synthesis, and content support.
2. Deterministic gameplay remains in Unity/C# code.
3. Every AI-generated artifact must have a human review owner.
4. AI outputs should land in explicit files, not stay trapped in chat.

## Authority Boundary

### AI Is Allowed To Drive
- concept art ideation
- card art exploration
- UI mock asset generation
- shot ideation and storyboard drafts
- VFX style exploration
- document retrieval and summarization
- test-case draft generation
- bug/log summarization
- content naming and metadata assistance

### AI Is Not Allowed To Authoritatively Decide
- card legality
- target validity
- damage/heal results
- turn order
- replay-critical battle state
- network-authoritative combat state

## Recommended OpenAI Stack

### 1. Responses API
Use as the primary integration surface for project assistants:
- design assistant
- programming assistant
- producer/backlog assistant
- QA/report assistant

Why:
- multi-turn workflows
- tool use
- function calling
- conversation-state support

### 2. File Search / Retrieval
Use for internal project knowledge retrieval across:
- GDDs
- ADRs
- architecture traceability
- project stage reports
- sprint docs
- code summaries
- reverse-documentation outputs

Recommended metadata filters:
- system
- document type
- status
- date
- owner

### 3. Image Generation
Use for:
- character concept exploration
- battlefield moodboards
- skill-shot ideation
- VFX and color-language exploration
- UI mockups and paintovers

Note:
- generated images are ideation assets, not automatic final production assets
- art direction still needs a review pass

### 4. Agents SDK / Workflow Agents
Use for orchestrated internal helpers:
- document QA assistants
- sprint and backlog assistants
- build triage helpers
- patch-note and milestone summary bots

### 5. Evals / Trace Review
Use for:
- checking doc assistant quality
- regression-checking design summaries
- validating QA helper outputs

## Project-Specific AI Roles

### Design Assistant
- reads all current design docs
- proposes missing system links
- drafts acceptance criteria and tuning notes

### Battle Shot Assistant
- turns a card or skill description into:
  - shot list
  - timing beats
  - VFX references
  - camera emphasis suggestions

### Retrieval Assistant
- answers questions like:
  - where is a role skill defined?
  - which docs mention hit timing?
  - what is still undocumented?

### QA Assistant
- summarizes logs
- expands smoke-check notes
- drafts regression test suggestions

## Integration Order

### Phase 1: Immediate
- use AI for planning, retrieval, and art/look-dev ideation
- use AI to expand battle-presentation documentation and backlog tasks

### Phase 2: Pre-Implementation Support
- build retrieval-ready document metadata
- define structured schemas for action definitions and test suggestions

### Phase 3: Production Automation
- add internal assistants for QA, backlog updates, and doc review
- evaluate computer-use or workflow agents only in isolated environments and with review gates

## Review Workflow

1. AI produces a draft
2. Human owner accepts, edits, or rejects
3. Accepted output is written to repo files
4. Checklist and backlog are updated
5. If production-significant, checkpoint commit is created

## Risks

- Over-trusting AI-generated runtime logic
- Producing too many drafts without integration discipline
- Letting generated art bypass visual direction review
- Treating retrieval summaries as source-of-truth without checking the source docs

## Acceptance Criteria

- [ ] AI usage categories are documented and understood by the team.
- [ ] Deterministic runtime ownership remains in Unity/C# systems.
- [ ] At least one retrieval-oriented workflow is defined for project docs.
- [ ] At least one AI-assisted content-ideation workflow is defined for battle presentation.
- [ ] Every AI-generated production artifact has a human review owner.
