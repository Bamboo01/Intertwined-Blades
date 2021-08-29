using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Pool> tagToPool;
    public Dictionary<string, Queue<GameObject>> poolDirectory;

    static public ObjectPool Instance
    {
        get;
        private set;
    }
    public void Awake()
    {
        if (Instance)
        {
            Debug.LogWarning("Event Manager instance already created. Deleting it and instantiating a new instance...");
            Destroy(Instance);
            Instance = this;
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        poolDirectory = new Dictionary<string, Queue<GameObject>>();
        tagToPool = new Dictionary<string, Pool>();

        foreach (Pool pool in pools)
        {
            tagToPool.Add(pool.tag, pool);
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
                obj.transform.SetParent(this.gameObject.transform);
            }
            poolDirectory.Add(pool.tag, objectPool);
        }
    }

    void Update()
    {
        transform.localPosition = new Vector3(0, 0, 0);
    }

    public GameObject spawnFromPool(string tag, int count = 0)
    {
        if (count == tagToPool[tag].size)
        {
            GameObject obj = Instantiate(tagToPool[tag].prefab);
            obj.SetActive(true);
            obj.transform.SetParent(this.gameObject.transform);
            poolDirectory[tag].Enqueue(obj);
            return obj;
        }

        GameObject poolobject = poolDirectory[tag].Dequeue();
        if (poolobject.activeSelf)
        {
            poolDirectory[tag].Enqueue(poolobject);
            return spawnFromPool(tag, ++count);
        }
        else
        {
            poolobject.SetActive(true);
            poolDirectory[tag].Enqueue(poolobject);
            return poolobject;
        }   
    }
}
