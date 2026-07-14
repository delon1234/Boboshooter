// [NEW] PoolManager.cs
// Location: Assets/Scripts/Core/PoolManager.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    // Generic Object Pool Manager to allow pooling of bullets, and other things
    public static PoolManager Instance { get; private set; }

    // Store ObjectPools casted as System.Object
    private readonly Dictionary<GameObject, object> poolDictionary = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public ObjectPool<T> GetPool<T>(GameObject prefab) where T : Component
    {
        if (prefab == null) return null;

        if (!poolDictionary.TryGetValue(prefab, out var poolObj))
        {
            // Optional: Create a parent object for visual cleanliness in the inspector
            Transform poolContainer = new GameObject($"Pool_{prefab.name}").transform;
            poolContainer.SetParent(transform);

            var pool = new ObjectPool<T>(
                createFunc: () => {
                    GameObject go = Instantiate(prefab, poolContainer);
                    T comp = go.GetComponent<T>();
                    if (comp == null) comp = go.AddComponent<T>();
                    return comp;
                },
                actionOnGet: component => component.gameObject.SetActive(true),
                actionOnRelease: component => component.gameObject.SetActive(false),
                actionOnDestroy: component => Destroy(component.gameObject),
                collectionCheck: false,
                defaultCapacity: 50,
                maxSize: 100
            );

            poolDictionary.Add(prefab, pool);
            return pool;
        }

        return (ObjectPool<T>)poolObj;
    }
}
