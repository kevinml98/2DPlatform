using System.Collections.Generic;
using UnityEngine;

/**
 * ObjectPool class.
 */
public class ObjectPool : MonoBehaviour {

    [Header("Pool data")]
    [SerializeField] private GameObject prefabObject;
    [SerializeField] private int poolDepth;
    [SerializeField] private bool canGrow;
    private readonly List<GameObject> pool = new List<GameObject>();

    private void Awake() {
        for (int i = 0; i < poolDepth; i++) {
            AddPooledObject();
        }
    }

    public GameObject GetAvaliableObject() {
        for (int i = 0; i < pool.Count; i++) {
            if (pool[i].activeInHierarchy == false) return pool[i];
        }

        if (canGrow) {
            return AddPooledObject();
        }

        return null;
    }

    private GameObject AddPooledObject() {
        GameObject pooledObject = Instantiate(prefabObject);
        pooledObject.SetActive(false);
        pool.Add(pooledObject);
        return pooledObject;
    }
}
