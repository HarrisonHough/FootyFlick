using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField]
    private GameObject assetToPool;
    [SerializeField]
    private int initialPoolSize = 50;

    private Queue<PoolMember> objectsQueue;
    private bool isPoolReady;

    public bool IsPoolReady => isPoolReady;

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        objectsQueue = new Queue<PoolMember>();

        for (var i = 0; i < initialPoolSize; i++)
        {
            var poolMember = AddPoolMember();
            objectsQueue.Enqueue(poolMember);
        }

        isPoolReady = true;
    }

    private PoolMember AddPoolMember()
    {
        var pooledObject = Instantiate(assetToPool, transform);
        pooledObject.SetActive(false);
        var poolMember = pooledObject.AddComponent<PoolMember>();
        poolMember.SetPool(this);
        return poolMember;
    }

    public GameObject GetObject(Vector3 position, Quaternion rotation, bool setActive = true)
    {
        if (objectsQueue.Count == 0)
        {
            ExpandPool();
        }

        var pooledObject = objectsQueue.Dequeue();
        pooledObject.transform.SetPositionAndRotation(position, rotation);

        if (setActive)
        {
            pooledObject.gameObject.SetActive(true);
        }

        return pooledObject.gameObject;
    }

    public void ReturnObject(PoolMember pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
        pooledObject.transform.SetParent(transform);
        if (objectsQueue.Contains(pooledObject)) return;
        objectsQueue.Enqueue(pooledObject);
    }

    private void ExpandPool()
    {
        var poolMember = AddPoolMember();
        objectsQueue.Enqueue(poolMember);
    }
}