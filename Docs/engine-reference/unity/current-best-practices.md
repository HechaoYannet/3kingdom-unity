# Unity 2022 LTS — Current Best Practices

**Last verified:** 2026-04-09

**Note:** Unity 2022 LTS is within the LLM's training data (cutoff May 2025). These best practices are already known to the model but documented here for completeness.

---

## Project Setup

### Use Unity 2022 LTS for Production
- **LTS (Long Term Support)**: Stable, 2-year support cycle
- **Tech Stream**: Newer features but less stable (not recommended for production)

### Choose the Right Render Pipeline
- **URP (Universal Render Pipeline)**: Recommended for most projects, good performance, mobile-friendly
- **HDRP (High Definition Render Pipeline)**: High-end graphics, PC/console focused
- **Built-in Render Pipeline**: Legacy, still supported but limited features

---

## Scripting

### C# Version
Unity 2022 LTS uses C# 8.0 (.NET Standard 2.1). Key features:
- Async/await support
- Nullable reference types (enable in project settings)
- Pattern matching enhancements
- Using declarations

### Async/Await for Asset Loading
```csharp
// Recommended pattern for async loading
public async Task<GameObject> LoadAssetAsync(string path)
{
    var request = Resources.LoadAsync<GameObject>(path);
    await request; // Unity's custom awaitable
    return (GameObject)request.asset;
}
```

### Addressables for Asset Management
For larger projects, use Addressables instead of Resources:
```csharp
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public async Task<GameObject> LoadAddressableAsync(string key)
{
    var handle = Addressables.LoadAssetAsync<GameObject>(key);
    return await handle.Task;
}
```

---

## Performance

### Use Object Pooling
Reuse GameObjects instead of Instantiate/Destroy:
```csharp
public class ObjectPool : MonoBehaviour
{
    private Queue<GameObject> pool = new Queue<GameObject>();
    
    public GameObject GetObject()
    {
        if (pool.Count > 0) return pool.Dequeue();
        return Instantiate(prefab);
    }
    
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
```

### Optimize Update Methods
- Use `Update`, `FixedUpdate`, `LateUpdate` appropriately
- Consider coroutines or async for non-frame-critical tasks
- Use event-driven patterns instead of polling

---

## UI

### UGUI (uGUI) for Runtime UI
Unity 2022 LTS uses UGUI as the primary runtime UI system:
- Use Canvas and RectTransform for layout
- Implement `IPointer*Handler` interfaces for input
- Use `EventSystem` for input handling

### TextMeshPro for Text
Always use TextMeshPro instead of legacy Text component:
- Better rendering quality
- More features (font assets, styling)
- Better performance

---

## Tuanjie Engine Considerations

Tuanjie Engine 1.5.3 maintains Unity 2022 LTS compatibility:
- All standard Unity best practices apply
- China-specific optimizations may be present
- Follow Unity 2022 LTS documentation for API usage