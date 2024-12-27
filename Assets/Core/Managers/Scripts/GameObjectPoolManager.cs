using System.Collections.Generic;
using UnityEngine;

public class GameObjectPoolManager : MonoBehaviour
{
    public static GameObjectPoolManager Instance;
    
    private Dictionary<ScriptableObject, GameObjectPool> pools = new ();
    void Awake()
    {
        Instance = this;
        Debug.Log("GameObjectPoolManager Awake");
    }

    private void RegisterPool(ScriptableObject key, GameObjectPool pool)
    {
        pools.Add(key, pool);
    }

    public GameObject GetObject(ScriptableObject key, GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(key))
        {
            Debug.LogWarning("No pool found for key: " + key);
            Debug.Log("Registering pool for key: " + key);
            RegisterPool(key, new GameObjectPool(prefab));
        }

        return pools[key].GetObject(position, rotation);
    }

    public void ReleaseObject(ScriptableObject key, GameObject obj)
    {
        if (!pools.ContainsKey(key))
        {
            Debug.LogError("No pool found for key: " + key);
            return;
        }

        pools[key].ReleaseObject(obj);
    }
}
