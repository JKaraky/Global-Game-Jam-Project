using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Variables
    [Header("Audio when a collectible is collected")]
    [SerializeField]
    private AudioClip collectibleOne;
    [SerializeField]
    private AudioClip collectibleTwo;
    [SerializeField]
    private AudioClip collectibleThree;

    [Header("Audio when players take actions")]
    [SerializeField]
    private AudioClip jamCannon;
    [SerializeField]
    private AudioClip cannonShot;
    [SerializeField]
    private AudioClip boostSpeed;

    private AudioSource audioSource;
    #endregion

    #region Singleton Setup
    // To setup Singleton
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                SetupInstance();
            }

            return instance;
        }
    }
    #endregion

    #region Method to Create Singleton Instance
    private static void SetupInstance()
    {
        instance = FindObjectOfType<AudioManager>();

        if (instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = "AudioManager";
            instance = gameObj.AddComponent<AudioManager>();
        }
    }
    #endregion

    #region Start
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    #endregion

    #region Audio Methods
    private void CollectiblesAudio(Collectibles collectible)
    {
        if(collectible == Collectibles.One)
        {
            PlayAudio(collectibleOne);
        }
        else if(collectible == Collectibles.Two)
        {
            PlayAudio(collectibleTwo);
        }
        else
        {
            PlayAudio(collectibleThree);
        }
    }

    private void JamAudio()
    {
        PlayAudio(jamCannon);
    }

    private void FireAudio()
    {
        PlayAudio(cannonShot);
    }

    private void BoostAudio()
    {
        PlayAudio(boostSpeed);
    }

    private void PlayAudio(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    #endregion

    private void OnEnable()
    {
        Points.CollectibleCollected += CollectiblesAudio;
        AvatarController.JammedCannon += JamAudio;
        AvatarController.FiredCannon += FireAudio;
        AvatarController.BoostSpeed += BoostAudio;
    }

    private void OnDisable()
    {
        Points.CollectibleCollected -= CollectiblesAudio;
        AvatarController.JammedCannon -= JamAudio;
        AvatarController.FiredCannon -= FireAudio;
        AvatarController.BoostSpeed -= BoostAudio;
    }
}
