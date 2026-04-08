---
status: reverse-documented
date: 2026-04-09
context: Character system architecture decisions discovered from existing implementation
decision: Record architectural patterns used in Character system
consequences: Provides documentation for maintenance and future development
---

# Character System Architecture Decisions

## Status
Reverse-documented from existing implementation

## Context
The Character System manages player entities, AI opponents, and role integration in a turn-based Three Kingdoms card game. The system was implemented without formal architecture documentation. This ADR captures the architectural decisions discovered through code analysis.

## Decision

### 1. Inheritance-Based Entity System
**Pattern**: Class Inheritance / Polymorphism
**Location**: `EnemyAI.cs:6-7`
```csharp
public class EnemyAI : Player {
    // Inherits all Player functionality
    // Overrides specific methods for AI behavior
}
```

**Rationale**:
- Shared behavior between human and AI players
- Code reuse for card management, health, turn states
- Polymorphic handling in game systems (both are `Player` objects)
- Easy to add new player types (e.g., different AI personalities)

**Consequences**:
- ✅ Maximum code reuse between human and AI players
- ✅ Polymorphic handling in game systems
- ✅ Easy to extend with new player types
- ❌ Tight coupling between base and derived classes
- ❌ Can't mix and match behaviors independently (inheritance hierarchy is rigid)
- ❌ Base class changes affect all derived classes

### 2. Singleton Player Manager
**Pattern**: Singleton / Service Locator
**Location**: `PlayerManager.cs:7,14-17`
```csharp
public static PlayerManager Instance { get; set; }
private void Awake() {
    Instance = this;
}
```

**Rationale**:
- Centralized access to all player instances
- Simple player lookup by ID
- Global game state management
- Follows Unity pattern for manager classes

**Consequences**:
- ✅ Easy access from anywhere in codebase
- ✅ Simple player ID lookup system
- ✅ Centralized player management
- ❌ Global state makes testing difficult
- ❌ Tight coupling to `PlayerManager.Instance`
- ❌ Not thread-safe (though Unity is single-threaded)

### 3. Coroutine-Based Timing System
**Pattern**: Coroutine / Async Pattern
**Location**: Throughout `Player.cs` and `EnemyAI.cs`
```csharp
public virtual IEnumerator EnterRound() {
    yield return StartCoroutine(UpdateTime());
}
protected override IEnumerator UpdateTime() {
    yield return new WaitForSeconds(Random.Range(2, 3f));
}
```

**Rationale**:
- Natural fit for Unity's coroutine system
- Easy to implement timed behaviors (AI delays, turn timers)
- Non-blocking gameplay during waits
- Simple sequential flow with `yield return`

**Consequences**:
- ✅ Natural Unity integration
- ✅ Easy timed behavior implementation
- ✅ Non-blocking gameplay
- ❌ Coroutine management complexity (start/stop)
- ❌ State can be hard to debug (coroutine execution flow)
- ❌ Potential for coroutine leaks if not properly stopped

### 4. State Flag Pattern for Turn Management
**Pattern**: Boolean State Flags
**Location**: `Player.cs:40-49`
```csharp
protected bool isInitiativeRound = false;
protected bool isResponsingRound = false;
public bool IsSubRound { get; set; } = false;
protected bool isWaiting;
```

**Rationale**:
- Simple boolean checks for game state
- Easy to understand and debug
- Minimal performance overhead
- Natural for turn-based game states

**Consequences**:
- ✅ Simple and fast state checking
- ✅ Easy to add new state flags
- ✅ Clear in code what each flag means
- ❌ State validation complexity (invalid combinations possible)
- ❌ No compile-time checking for state transitions
- ❌ Can lead to state explosion with many flags

### 5. Composition with Role System
**Pattern**: Composition / Has-A Relationship
**Location**: `Player.cs:16-17`
```csharp
public List<Role> roles = new List<Role>();//no used
public Role CurrentRole { get; protected set; }
```

**Rationale**:
- Separates character identity from player mechanics
- Allows role switching without changing player object
- Roles can be designed independently
- Follows composition-over-inheritance principle

**Consequences**:
- ✅ Flexible role assignment and switching
- ✅ Roles can be designed/tested independently
- ✅ Clean separation of concerns
- ❌ Additional indirection (player → role → abilities)
- ❌ Role-player communication overhead
- ❌ Need to manage role lifecycle alongside player

### 6. Enum-Based UI Communication
**Pattern**: Enum Message Passing
**Location**: `Player.cs:405-410`
```csharp
public enum UIOperation {
    NONE, OK, CANCEL
}
public UIOperation PlayerUIOperation { get; set; }
```

**Rationale**:
- Simple communication between UI and game logic
- Type-safe operation definitions
- Easy to extend with new operations
- Minimal overhead compared to events/delegates

**Consequences**:
- ✅ Simple and lightweight
- ✅ Type-safe operation handling
- ✅ Easy to add new operations
- ❌ Limited to predefined operations
- ❌ No payload data (just operation type)
- ❌ Polling required to check operation state

## Alternatives Considered

### For Inheritance vs Composition:
- **Pure Composition**: Each player has components for human/AI behavior (more flexible but more complex)
- **Strategy Pattern**: Inject behavior strategies at runtime (more testable but more setup)
- **Current Inheritance**: Simple hierarchy with overrides (chosen for simplicity)

### For Player Management:
- **Dependency Injection**: Inject player references where needed (more testable but verbose)
- **Event Bus**: Players emit events, systems subscribe (decoupled but harder to trace)
- **Current Singleton**: Simple global access (chosen for ease of use)

### For Timing System:
- **Update() polling**: Check time each frame (simpler but less efficient)
- **Unity's Invoke()**: Scheduled method calls (less flexible than coroutines)
- **Current Coroutines**: Natural for sequenced timed behaviors (chosen for Unity integration)

### For State Management:
- **State Pattern**: Separate state classes (more organized but more classes)
- **Enum State Machine**: Single state enum with switch statements (cleaner but less flexible)
- **Current Boolean Flags**: Simple direct checks (chosen for simplicity)

## Compliance with Project Standards

### Adheres to:
- Unity MonoBehaviour patterns
- C# naming conventions
- Composition over inheritance (for Role system)
- Coroutine-based async patterns (Unity standard)

### Deviations from Best Practices:
1. **Singleton Usage**: Conflicts with testability standards
2. **Boolean State Flags**: Can lead to state explosion and validation issues
3. **Hardcoded Player IDs**: Magic numbers (0=deck, 1=discard, 2+=players)
4. **Tight Coupling**: `EnemyAI` tightly coupled to `Player` base class

## Related Decisions

### Connected Systems:
1. **Card System**: Players interact with cards via `currentCards` and card methods
2. **Role System**: Composition relationship for character abilities
3. **UI System**: `UIOperation` enum for UI-game communication
4. **Round Manager**: Players participate in round state machine

### Dependencies:
- Unity Engine (coroutines, MonoBehaviour)
- Card System (`Card` class, card checking methods)
- Role System (`Role` class, faction system)

## Verification

### Architecture Validation:
- [x] Inheritance hierarchy identified and documented
- [x] Singleton pattern usage confirmed
- [x] Coroutine timing system analyzed
- [x] Composition with Role system validated
- [x] State flag pattern documented

### Code Quality Issues:
- [ ] Singleton creates global state and tight coupling
- [ ] Boolean state flags lack validation
- [ ] Hardcoded PlayerID meanings (magic numbers)
- [ ] AI behavior is simplistic (random delays only)
- [ ] Health/damage mechanics not implemented

## Revision History
- 2026-04-09: Initial reverse-documentation from existing code