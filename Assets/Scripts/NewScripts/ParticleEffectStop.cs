using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectStop : MonoBehaviour
{
    [SerializeField]
    private GameObject particlesHolder;
    private void OnParticleSystemStopped()
    {
        particlesHolder.SetActive(false);
    }
}
