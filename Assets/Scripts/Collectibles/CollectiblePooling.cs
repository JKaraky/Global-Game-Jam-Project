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
    [SerializeField] private GameObject spawnPointsHolder; 
    private static Transform[] spawnPoints;
    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        pool = new ObjectPool<Collectible>(CreateObject, ActivateObject, DeactivateObject, DestroyObject, true, 10, 15);
        spawnPoints = spawnPointsHolder.GetComponentsInChildren<Transform>();
        Debug.Log(spawnPoints.Length);
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
        int spawnPoint = UnityEngine.Random.Range(0, spawnPoints.Length);
        Transform pointTransform = spawnPoints[spawnPoint];
        return new Vector3(pointTransform.position.x, pointTransform.position.y, pointTransform.position.z);
    }
    #endregion
}
