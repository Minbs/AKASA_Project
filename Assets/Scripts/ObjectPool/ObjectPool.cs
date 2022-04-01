using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    public List<PooledObject> objectPool = new List<PooledObject>();

    private void Awake()
    {
        for (int ix = 0; ix < objectPool.Count; ++ix)
        {
            objectPool[ix].Initialize(transform);
        }
    }

    public void CreatePoolObject(string itemName, GameObject item, int count,Transform parent = null)
    {
        PooledObject poolObject = new PooledObject();

        poolObject.poolCount = count;
        poolObject.poolItemName = itemName;
        poolObject.prefab = item;

        for (int i = 0; i < count; i++)
        {
            GameObject poolItem = Object.Instantiate(item) as GameObject;
            poolItem.name = itemName;
            poolItem.SetActive(false);
            poolItem.transform.SetParent(parent);
            poolObject.poolList.Add(poolItem);
        }

        objectPool.Add(poolObject);
    }

    public bool PushToPool(string itemName, GameObject item, Transform parent = null)
    {
        PooledObject pool = GetPoolItem(itemName);

        if (pool == null)
        {
            return false;
        }



        pool.PushToPool(item, parent == null ? transform : parent);



        return true;
    }

    public GameObject PopFromPool(string itemName, Transform parent = null)
    {
        PooledObject pool = GetPoolItem(itemName);
        if (pool == null)
            return null;

        return pool.PopFromPool(parent);
    }
    PooledObject GetPoolItem(string itemName)
    {
        for (int ix = 0; ix < objectPool.Count; ++ix)
        {
            if (objectPool[ix].poolItemName.Equals(itemName))
                return objectPool[ix];
        }
        Debug.LogWarning("맞는 풀 리스트가 없습니다.");
        return null;
    }
}
