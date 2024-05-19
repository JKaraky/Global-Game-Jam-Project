using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SfxManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private AudioClip buttonSound;
    private AudioSource audioSource;
    #endregion

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void PlayButtonAudio()
    {
        audioSource.PlayOneShot(buttonSound);
    }

    private void OnEnable()
    {
        ChangeText.ButtonSelected += PlayButtonAudio;
    }

    private void OnDisable()
    {
        ChangeText.ButtonSelected -= PlayButtonAudio;
    }
}
