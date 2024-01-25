using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private CollectiblePooling poolOfCollectible;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            poolOfCollectible.pool.Release(this);
        }
    }
}
