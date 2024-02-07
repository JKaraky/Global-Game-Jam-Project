using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleParticles : MonoBehaviour
{
    #region Variables
    private ParticlesPooling poolOfParticle;

    public ParticlesPooling PoolOfParticle
    {
        get
        {
            return poolOfParticle;
        }
        set
        {
            poolOfParticle = value;
        }
    }
    #endregion

    private void OnParticleSystemStopped()
    {
        this.gameObject.SetActive(false);
        poolOfParticle.pool.Release(this);
    }
}
