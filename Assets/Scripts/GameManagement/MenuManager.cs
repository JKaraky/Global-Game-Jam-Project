using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    #region Variables
    [Header ("Audio Settings")]
    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private AudioSource musicSource;

    [Header("Instrucitons Slides")]
    [SerializeField]
    private GameObject[] instructions;

    private LinkedList<GameObject> slides;
    private LinkedListNode<GameObject> _currentSlide;
    #endregion

    private void Start()
    {
        SetSettingsSliderFromPrefs();

        slides = new LinkedList<GameObject> (instructions);
        _currentSlide = slides.First;
    }
    public void StartGame()
    {
        Cursor.visible = false;

        SceneManager.LoadScene("Arena", LoadSceneMode.Single);
    }

    public void NextInstructionSlide()
    {
        _currentSlide.Value.SetActive(false);
        if (_currentSlide == slides.Last)
        {
            _currentSlide = slides.First;
        }
        else
        {
            _currentSlide = _currentSlide.Next;
        }
        _currentSlide.Value.SetActive(true);
    }

    public void PreviousInstructionSlide()
    {
        _currentSlide.Value.SetActive(false);
        if (_currentSlide == slides.First)
        {
            _currentSlide = slides.Last;
        }
        else
        {
            _currentSlide = _currentSlide.Previous;
        }
        _currentSlide.Value.SetActive(true);
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
