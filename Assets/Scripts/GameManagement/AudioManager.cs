using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] whenDuckWins;

    [SerializeField]
    private AudioClip[] whenWormWins;



    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void PlayAudio(int player)
    {
        if(player == 0)
        {
            int audioToPlay = Random.Range(0, whenDuckWins.Length);
            audioSource.PlayOneShot(whenDuckWins[audioToPlay]);
        }
        else if(player == 1)
        {
            int audioToPlay = Random.Range(0, whenWormWins.Length);
            audioSource.PlayOneShot(whenWormWins[audioToPlay]);
        }
        else
        {
            Debug.LogError("Audio manager got an int that is out of range");
        }
    }

    private void OnEnable()
    {
        PointsManager.GainedPoint += PlayAudio;
    }

    private void OnDisable()
    {
        PointsManager.GainedPoint -= PlayAudio;
    }
}
