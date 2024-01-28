using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private CollectiblePooling poolOfCollectible;
    [SerializeField]
    private static float lifespan = 20;

    [SerializeField]
    private ParticleSystem particleEffect;

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
        particleEffect.Play();
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
