using System;
using System.Collections;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    #region Variables
    private CollectiblePooling poolOfCollectible;
    private static float lifespan = 30;
    [SerializeField] private bool fade = true;

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
        if (fade)
        {
            lifespan = GameManager.Instance.SpawnLifeSpan;
            StartCoroutine(LifeSpan());
        }
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
        if(poolOfCollectible != null)
        {
            poolOfCollectible.pool.Release(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
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
