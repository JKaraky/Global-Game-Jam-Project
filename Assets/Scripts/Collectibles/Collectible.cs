using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private CollectiblePooling poolOfCollectible;
    private static float lifespan = 20;

    [SerializeField]
    private ParticleSystem destroyEffect, fadeEffect, collectEffect;

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
    private void OnEnable()
    {
        StartCoroutine(LifeSpan());
    }

    public void DestroyCollectible()
    {
        destroyEffect.Play();
        ReleaseCollectible();
    }

    public void CollectCollectible()
    {
        collectEffect.Play();
        ReleaseCollectible();
    }

    public void FadeCollectible()
    {
        fadeEffect.Play();
        ReleaseCollectible();
    }

    private void ReleaseCollectible()
    {
        poolOfCollectible.pool.Release(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CollectCollectible();
        }
    }

    private IEnumerator LifeSpan()
    {
        yield return new WaitForSeconds(lifespan);
        FadeCollectible();
    }
}
