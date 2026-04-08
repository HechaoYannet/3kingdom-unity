# Code Summary - Assets/Scripts

**Total Files**: 54 C# files

## 1. Asset Bundle Framework
**Location**: `Assets/Scripts/AssetBundleFramework/`
**Purpose**: Load game assets from bundles
**Key Files**:
- `AssetBundleManager.cs` - Main manager for asset bundles
- `MultiABManager.cs` - Manages multiple asset bundles
- `SingleABLoader.cs` - Loads single asset bundle
- `ABManifestLoader.cs` - Reads bundle manifest files
- `AssetLoader.cs` - Base class for asset loading
- `ABRelation.cs` - Tracks bundle dependencies
- `PathTools.cs` - Path utilities for asset bundles
- `AbDefine.cs` - Constants and definitions
- `AutoSetLabels.cs` (Editor) - Auto-labels assets for bundling
- `BuildAssetBundle.cs` (Editor) - Builds asset bundles
- `TestClass_ABMgr.cs` - Test class for asset bundle manager

## 2. Card System
**Location**: `Assets/Scripts/Card/`, `Assets/Scripts/CardManage/`, `Assets/Scripts/CardOnDrawer/`
**Purpose**: Card game mechanics and management
**Key Files**:
- `Card.cs` - Base card class
- `CardData.cs` - Card data structure
- `Cards/Sha.cs` - Specific card type "Sha" (kill card)
- `CardManager.cs` - Manages all cards in game
- `RoundManager.cs` - Manages game rounds
- `ResourcesLoad.cs` - Loads card resources
- `Card3D.cs` - 3D card representation
- `CardView3D.cs` - 3D card view controller

## 3. Character System
**Location**: `Assets/Scripts/Character/`
**Purpose**: Player and enemy characters
**Key Files**:
- `Player.cs` - Player character
- `EnemyAI.cs` - Enemy AI controller
- `PlayerManager.cs` - Manages player instances

## 4. Role System
**Location**: `Assets/Scripts/Role/`
**Purpose**: Game roles with special abilities
**Key Files**:
- `Role.cs` - Base role class
- `HuangGai.cs` - Specific role "HuangGai"
- Other role files (likely more specific roles)

## 5. UI System
**Location**: `Assets/Scripts/UI/`
**Purpose**: User interface management
**Key Files**:
- `UIManager.cs` - Main UI manager
- `UIBase.cs` - Base UI class
- Other UI component files

## 6. Audio System
**Location**: `Assets/Scripts/Audio/`
**Purpose**: Sound management
**Key Files**:
- `AudioManage.cs` - Audio manager

## 7. Data Management
**Location**: `Assets/Scripts/Data/`
**Purpose**: Game data handling
**Key Files**:
- `DataManage.cs` - Data manager
- `Singleton.cs` - Singleton pattern base class
- `IOData.cs` - Input/output data handling
- Other data utility files

## 8. Event System
**Location**: `Assets/Scripts/Event/`
**Purpose**: Game events
**Key Files**:
- `EventManager.cs` - Event manager

## 9. Tools
**Location**: `Assets/Scripts/Tools/`
**Purpose**: Utility classes
**Key Files**:
- Various utility classes for game development

## System Dependencies
1. **Asset Bundle Framework** → All systems (provides assets)
2. **Card System** ↔ **Character System** (cards affect characters)
3. **Role System** → **Character System** (roles modify characters)
4. **UI System** → All systems (displays game state)
5. **Audio System** → All systems (plays sounds)
6. **Data Management** → All systems (stores/loads data)
7. **Event System** → All systems (handles game events)

## Game Flow
1. Load assets (Asset Bundle Framework)
2. Initialize characters and roles
3. Start round (RoundManager)
4. Players draw/play cards (CardManager)
5. UI updates game state
6. Events trigger audio/effects
7. Data saved between sessions

## Notes
- Uses Singleton pattern for managers
- Asset bundles for performance
- 3D card visualization
- Role-based special abilities
- Event-driven architecture