using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject learnMenu;

    [Header ("Audio Settings")]
    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private AudioSource musicSource;
    #endregion

    private void Start()
    {
        SetSettingsSliderFromPrefs();
    }
    public void StartGame()
    {
        Cursor.visible = false;

        SceneManager.LoadScene("Arena", LoadSceneMode.Single);
    }

    public void GoToLearn()
    {
        learnMenu.SetActive(true);
    }

    public void GoBackFromLearn()
    {
        learnMenu.SetActive(false);
    }
    #region Audio Methods
    public void ChangeMusicVolume()
    {
        musicSource.volume = musicSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }

    public void ChangeSfxVolume()
    {
        sfxSource.volume = sfxSlider.value;
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
    }

    public void SetSettingsSliderFromPrefs()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            musicSource.volume = musicSlider.value;
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
            sfxSource.volume = sfxSlider.value;
        }
    }
    #endregion

    public void QuitGame()
    {
        Application.Quit();
    }
}
