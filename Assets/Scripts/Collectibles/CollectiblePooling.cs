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

    [SerializeField] float maxBorder;
    // Start is called before the first frame update
    void Start()
    {
        pool = new ObjectPool<Collectible>(CreateObject, ActivateObject, DeactivateObject, DestroyObject, true, 10, 15);
    }

    #region Pool Methods
    private Collectible CreateObject()
    {
        Collectible collectible = Instantiate(pooledObject, RandomPosition(), Quaternion.identity);
        collectible.GetComponent<FollowPlayer>().Target = GameManager.Instance.Player;
        collectible.PoolOfCollectible = this;
        return collectible;
    }

    private void ActivateObject(Collectible pooled)
    {
        pooled.transform.position = RandomPosition();
        pooled.GetComponent<FollowPlayer>().Target = GameManager.Instance.Player;
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

    #region Randomization Method
    private Vector3 RandomPosition()
    {
        int wall = UnityEngine.Random.Range(0, 4);
        float spawnPosition = UnityEngine.Random.Range(-maxBorder, maxBorder);

        if(wall == 0)
        {
            return new Vector3(-maxBorder, 1, spawnPosition);
        }
        else if (wall == 1)
        {
            return new Vector3(maxBorder, 1, spawnPosition);
        }
        else if(wall == 2)
        {
            return new Vector3(spawnPosition, 1, maxBorder);
        }
        else
        {
            return new Vector3(spawnPosition, 1, -maxBorder);
        }
    }
    #endregion
}
