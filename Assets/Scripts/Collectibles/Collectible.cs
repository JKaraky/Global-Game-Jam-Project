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
    }

    public void CollectCollectible()
    {
        ReleaseCollectible();
    }

    public void FadeCollectible()
    {
        ReleaseCollectible();
    }

    private void ReleaseCollectible()
    {
        poolOfCollectible.pool.Release(this);
    }
    #endregion

    #region Collision Handling
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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
