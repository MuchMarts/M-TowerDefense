using UnityEngine;
using UnityEngine.Pool;

public class GameObjectPool
{
    GameObject prefab;
    ObjectPool<GameObject> pool;
    int defaultSize;
    int maxSize;

    // Our class's constructor. Takes the prefab to spawn as an argument.
    public GameObjectPool(GameObject prefab, int defaultSize = 20,
        int maxSize = 100)
    {
        this.prefab = prefab;  // The prefab to spawn.
        this.defaultSize = defaultSize;  // Pool's starting number of objects.
        this.maxSize = maxSize;  // Max size for our pool.
        // Initializing our pool.
        pool = new ObjectPool<GameObject>(
            CreatePooledObject,
            OnGetFromPool,
            OnReturnToPool,
            OnDestroyPooledObject,
            true,
            defaultSize,
            maxSize
            );
    }

    // Wrapper function for pool.Get. Gets object and sets position.
    // Also resets rigidbody's velocity if it has one.
    public GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        GameObject obj = pool.Get();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        if(obj.TryGetComponent<Rigidbody> (out Rigidbody rb))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        return obj;
    }

    // Wrapper function for pool.Release
    public void ReleaseObject(GameObject obj)
    {
        pool.Release(obj);
    }

    GameObject CreatePooledObject()
    {
        GameObject newObject = GameObject.Instantiate(prefab);
        return newObject;
    }

    // When an object is taken from the pool, activate it.
    void OnGetFromPool(GameObject pooledObject)
    {
        pooledObject.SetActive(true);
    }

    // When an object is returned to the pool, deactivate it.
    void OnReturnToPool(GameObject pooledObject)
    {
        pooledObject.SetActive(false);
    }

    // When the pool discards an object, destroy the GameObject.
    void OnDestroyPooledObject(GameObject pooledObject)
    {
        GameObject.Destroy(pooledObject);
    }
}