using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private CollectiblePooling poolOfCollectible;
    [SerializeField]
    private static float lifespan = 20;

    public static event Action<Vector3> PlayDestroyEffect;

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

    public void ReleaseCollectible()
    {
        PlayDestroyEffect?.Invoke(transform.position);
        poolOfCollectible.pool.Release(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ReleaseCollectible();
        }
    }

    private IEnumerator LifeSpan()
    {
        yield return new WaitForSeconds(lifespan);
        ReleaseCollectible();
    }
}
