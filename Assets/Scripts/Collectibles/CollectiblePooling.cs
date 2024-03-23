using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CollectiblePooling : MonoBehaviour
{
    #region Variables
    public ObjectPool<Collectible> pool;
    [SerializeField] private Collectible pooledObject;
    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        pool = new ObjectPool<Collectible>(CreateObject, ActivateObject, DeactivateObject, DestroyObject, true, 10, 15);
    }

    #region Pool Methods
    private Collectible CreateObject()
    {
        Collectible collectible = Instantiate(pooledObject);
        collectible.PoolOfCollectible = this;
        return collectible;
    }

    private void ActivateObject(Collectible pooled)
    {
        pooled.gameObject.SetActive(true);
    }

    private void DeactivateObject(Collectible pooled)
    {
        pooled.gameObject.SetActive(false);
    }

    private void DestroyObject(Collectible pooled)
    {
        Destroy(pooled.gameObject);
    }
    #endregion
}
