using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum ParticlesToDeploy { Destroy, Fade, Collect };
public class ParticlesPooling : MonoBehaviour
{
    #region Variables
    public ObjectPool<CollectibleParticles> pool;

    [SerializeField] 
    private CollectibleParticles pooledParticles;

    [SerializeField]
    private ParticlesToDeploy particleToDeploy;

    private Vector3 spawningPosition;
    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        pool = new ObjectPool<CollectibleParticles>(CreateParticle, ActivateParticle, DeactivateParticle, DestroyParticle, true, 10, 15);
    }

    #region Pool Methods
    private CollectibleParticles CreateParticle()
    {
        CollectibleParticles collectibleParticle = Instantiate(pooledParticles, spawningPosition, Quaternion.identity);
        collectibleParticle.PoolOfParticle = this;
        return collectibleParticle;
    }

    private void ActivateParticle(CollectibleParticles pooledParticle)
    {
        pooledParticle.transform.position = spawningPosition;
        pooledParticle.gameObject.SetActive(true);
    }

    private void DeactivateParticle(CollectibleParticles pooledParticles)
    {
        pooledParticles.gameObject.SetActive(false);
    }

    private void DestroyParticle(CollectibleParticles pooledParticle)
    {
        Destroy(pooledParticle.gameObject);
    }

    public void TriggerPool(Vector3 spawnPosition)
    {
        spawningPosition = spawnPosition;
        this.pool.Get();
    }
    #endregion

    #region OnEnable and OnDisable
    private void OnEnable()
    {
        if(particleToDeploy == ParticlesToDeploy.Destroy)
        {
            Collectible.CollectibleDestroyed += TriggerPool;
        }
        else if(particleToDeploy == ParticlesToDeploy.Fade)
        {
            Collectible.CollectibleFaded += TriggerPool;
        }
        else if(particleToDeploy == ParticlesToDeploy.Collect)
        {
            Collectible.CollectibleCollected += TriggerPool;
        }
    }

    private void OnDisable()
    {
        if (particleToDeploy == ParticlesToDeploy.Destroy)
        {
            Collectible.CollectibleDestroyed -= TriggerPool;
        }
        else if (particleToDeploy == ParticlesToDeploy.Fade)
        {
            Collectible.CollectibleFaded -= TriggerPool;
        }
        else if (particleToDeploy == ParticlesToDeploy.Collect)
        {
            Collectible.CollectibleCollected -= TriggerPool;
        }
    }
    #endregion
}
