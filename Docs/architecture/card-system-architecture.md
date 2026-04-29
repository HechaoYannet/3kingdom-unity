---
status: reverse-documented
date: 2026-04-09
context: Card system architecture decisions discovered from existing implementation
decision: Record architectural patterns used in Card system
consequences: Provides documentation for maintenance and future development
---

# Card System Architecture Decisions

## Status
Reverse-documented from existing implementation

## Context
The Card System is a core component of a turn-based card game built in Unity/Tuanjie Engine 1.5.3. The system was implemented without formal architecture documentation. This ADR captures the architectural decisions discovered through code analysis.

## Decision

### 1. Singleton Pattern for Global Managers
**Pattern**: Singleton
**Location**: `CardManager.cs:10-16`
```csharp
public static CardManager instance;
private void Awake() {
    instance = this;
}
```

**Rationale**: 
- Provides global access to card management functions
- Simplifies card operations across different systems
- Follows Unity common pattern for manager classes

**Consequences**:
- ✅ Easy access from anywhere in codebase
- ✅ Simple initialization
- ❌ Difficult to test (global state)
- ❌ Tight coupling to CardManager.instance
- ❌ Not thread-safe (though Unity is single-threaded)

### 2. Abstract Base Class for Card Types
**Pattern**: Template Method / Abstract Base Class
**Location**: `Card.cs:5-57`
```csharp
public abstract class Card : MonoBehaviour {
    public abstract string cardName { get; protected set; }
    public abstract void DoCardsAction(Player user, Player target);
    // ...
}
```

**Rationale**:
- Defines common interface for all card types
- Enforces consistent property implementation
- Allows polymorphic card handling

**Consequences**:
- ✅ Consistent card interface
- ✅ Easy to add new card types
- ✅ Polymorphic card handling
- ❌ Requires concrete implementation for each card type
- ❌ Abstract methods must be implemented by subclasses

### 3. Component-Based 3D Visualization
**Pattern**: Component Architecture / Separation of Concerns
**Location**: `CardOnDrawer/` directory
- `Card3D.cs`: Drag interaction and game logic
- `CardView3D.cs`: Visual representation and UI
- `HandManager3D.cs`: Layout management
- `PlayZone3D.cs`: Zone detection

**Rationale**:
- Separates interaction logic from visual representation
- Allows independent optimization of visual and logic components
- Follows Unity's component-based architecture

**Consequences**:
- ✅ Clear separation of concerns
- ✅ Independent optimization possible
- ✅ Reusable components
- ❌ More complex object hierarchy
- ❌ Communication overhead between components

### 4. State Machine for Round Management
**Pattern**: State Machine
**Location**: `RoundManager.cs:14-99`
```csharp
public GRoundState RoundState { get; private set; }
public enum GRoundState {
    Preparing, Checking, GettingCard, Battling, RefusingCard, Ending
}
```

**Rationale**:
- Clear definition of game round phases
- Sequential execution of round steps
- Easy to debug and trace round flow

**Consequences**:
- ✅ Clear round progression
- ✅ Easy to add/remove round states
- ✅ Debuggable state transitions
- ❌ Linear progression (no branching)
- ❌ State transitions are hardcoded in coroutine

### 5. Generic Resource Loading Utility
**Pattern**: Generic Utility Class
**Location**: `ResourcesLoad.cs:13-25`
```csharp
public class ResourcesLoad<T> : MonoBehaviour where T:Object {
    public static T LoadRes(string path) {
        return Resources.Load<T>(path);
    }
}
```

**Rationale**:
- Type-safe resource loading
- Reusable across different resource types
- Simple static interface

**Consequences**:
- ✅ Type safety
- ✅ Reusable utility
- ✅ Simple API
- ❌ Uses `Resources.Load()` instead of Addressables
- ❌ Singleton pattern for instance (unused)

### 6. Event-Based Card Updates
**Pattern**: Observer Pattern / Events
**Location**: `Card3D.cs:31,54,166`
```csharp
public event System.Action OnCardDataChanged;
public void Initialize(Card card) {
    cardData = card;
    OnCardDataChanged?.Invoke();
}
```

**Rationale**:
- Decouples card data changes from UI updates
- Allows multiple systems to react to card changes
- Follows Unity event pattern

**Consequences**:
- ✅ Loose coupling between data and UI
- ✅ Multiple subscribers possible
- ✅ Clean separation of concerns
- ❌ Event subscription/unsubscription management
- ❌ Potential memory leaks if not unsubscribed

## Alternatives Considered

### For Singleton Pattern:
- **Dependency Injection**: More testable but more complex setup
- **Service Locator**: Similar issues to singleton but more flexible
- **ScriptableObject Events**: Event-driven communication (not used)

### For Resource Loading:
- **Addressables**: Better for large projects but more complex
- **AssetBundles**: Manual bundle management (referenced in comments)
- **Direct Resources.Load**: Simple but not scalable (current choice)

### For 3D Visualization:
- **Single Monolithic Component**: Simpler but less maintainable
- **Entity-Component-System (ECS)**: More performant but complex
- **Current Component Split**: Balanced approach chosen

## Compliance with Project Standards

### Adheres to:
- Unity component-based architecture
- C# naming conventions (PascalCase classes, _camelCase private fields)
- MonoBehaviour lifecycle patterns

### Deviations from Best Practices:
1. **Singleton Usage**: Conflicts with testability standards
2. **Resources.Load**: Should use Addressables for production
3. **Hardcoded Values**: Magic numbers in card distribution
4. **Commented Code**: Significant commented-out functionality

## Related Decisions

### Connected Systems:
1. **Player System**: Uses `Player.GetCard()` and `currentCards` list
2. **Role System**: Referenced but architecture unclear
3. **UI System**: Interacts with card selection and display
4. **Audio System**: Card sound effects (commented out)

### Dependencies:
- Unity Engine 2022 LTS (Tuanjie Engine 1.5.3)
- DG.Tweening (for card effects, referenced)
- TMPro (for 3D text, used in CardView3D)

## Verification

### Architecture Validation:
- [x] Singleton pattern identified and documented
- [x] Component separation validated
- [x] State machine pattern confirmed
- [x] Event system usage documented

### Code Quality Issues:
- [ ] Singleton creates tight coupling
- [ ] Resource loading not production-ready
- [ ] Incomplete method implementations
- [ ] Significant technical debt in commented code

## Revision History
- 2026-04-09: Initial reverse-documentation from existing code