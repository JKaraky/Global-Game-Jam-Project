using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Collectible : MonoBehaviour
{
    #region Variables
    private CollectiblePooling poolOfCollectible;
    private static float lifespan = 20;

    public CollectiblePooling PoolOfCollectible
    {
        get
        {
            return poolOfCollectible;
        }
        set
        {
            poolOfCollectible = value;
        }
    }

    public static event Action<Vector3> CollectibleCollected;
    public static event Action<Vector3> CollectibleDestroyed;
    public static event Action<Vector3> CollectibleFaded;
    #endregion

    #region OnEnable
    private void OnEnable()
    {
        StartCoroutine(LifeSpan());
    }
    #endregion

    #region Handling Collectible Removal From Level
    public void DestroyCollectible()
    {
        ReleaseCollectible();
        CollectibleDestroyed?.Invoke(transform.position);
    }

    public void CollectCollectible()
    {
        ReleaseCollectible();
        CollectibleCollected?.Invoke(transform.position);
    }

    public void FadeCollectible()
    {
        ReleaseCollectible();
        CollectibleFaded?.Invoke(transform.position);
    }

    private void ReleaseCollectible()
    {
        poolOfCollectible.pool.Release(this);
    }
    #endregion

    #region Collision Handling
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CollectCollectible();
        }
    }
    #endregion

    #region LifeSpan Handling
    private IEnumerator LifeSpan()
    {
        yield return new WaitForSeconds(lifespan);
        FadeCollectible();
    }
    #endregion
}
