using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
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
    [SerializeField]
    private AudioMixer masterMixer;

    [Header("Tutorial")]
    [SerializeField]
    private Toggle tutorialToggle;

    [Header("Instrucitons Slides")]
    [SerializeField]
    private GameObject[] instructions;

    [Header("Credits Slides")]
    [SerializeField]
    private GameObject[] credits;

    [Header("Keyboard Actions")]
    [SerializeField]
    private List<GameObject> humanKeyboardActions;
    [SerializeField]
    private List<GameObject> robotKeyboardActions;

    [Header("Gamepad Actions")]
    [SerializeField]
    private List<GameObject> humanGamepadActions;
    [SerializeField]
    private List<GameObject> robotGamepadActions;

    private LinkedList<GameObject> instructionSlides;
    private LinkedList<GameObject> creditsSlides;
    private LinkedListNode<GameObject> _currentInstructionSlide;
    private LinkedListNode<GameObject> _currentCreditsSlide;
    private EventSystem _eventSystem;



    #endregion
    private void Start()
    {
        _eventSystem = EventSystem.current;

        if (!PlayerPrefs.HasKey("PlayTutorial"))
        {
            PlayerPrefs.SetInt("PlayTutorial", 1);
        }
        else
        {
            tutorialToggle.isOn = PlayerPrefs.GetInt("PlayTutorial") == 1 ? true : false;
        }
        SetSettingsSliderFromPrefs();

        instructionSlides = new LinkedList<GameObject> (instructions);
        _currentInstructionSlide = instructionSlides.First;

        creditsSlides = new LinkedList<GameObject>(credits);
        _currentCreditsSlide = creditsSlides.First;
    }
    public void StartGame(GameObject tutorialPanel)
    {
        if (!tutorialPanel.activeSelf && PlayerPrefs.GetInt("PlayTutorial") == 1)
        {
            tutorialPanel.SetActive(true);

            PlayerPrefs.SetInt("PlayTutorial", 0);
            tutorialToggle.isOn = false;
        }

        else
        {
            Cursor.visible = false;
            SceneManager.LoadScene("Arena", LoadSceneMode.Single);
        }
    }

    public void NextInstructionSlide()
    {
        _currentInstructionSlide.Value.SetActive(false);
        if (_currentInstructionSlide == instructionSlides.Last)
        {
            _currentInstructionSlide = instructionSlides.First;
        }
        else
        {
            _currentInstructionSlide = _currentInstructionSlide.Next;
        }
        _currentInstructionSlide.Value.SetActive(true);
    }

    public void PreviousInstructionSlide()
    {
        _currentInstructionSlide.Value.SetActive(false);
        if (_currentInstructionSlide == instructionSlides.First)
        {
            _currentInstructionSlide = instructionSlides.Last;
        }
        else
        {
            _currentInstructionSlide = _currentInstructionSlide.Previous;
        }
        _currentInstructionSlide.Value.SetActive(true);
    }

    public void NextCreditsSlide()
    {
        _currentCreditsSlide.Value.SetActive(false);
        if (_currentCreditsSlide == creditsSlides.Last)
        {
            _currentCreditsSlide = creditsSlides.First;
        }
        else
        {
            _currentCreditsSlide = _currentCreditsSlide.Next;
        }
        _currentCreditsSlide.Value.SetActive(true);
    }

    public void PreviousCreditsSlide()
    {
        _currentCreditsSlide.Value.SetActive(false);
        if (_currentCreditsSlide == creditsSlides.First)
        {
            _currentCreditsSlide = creditsSlides.Last;
        }
        else
        {
            _currentCreditsSlide = _currentCreditsSlide.Previous;
        }
        _currentCreditsSlide.Value.SetActive(true);
    }

    public void ResetTutorial (bool reset)
    {
        PlayerPrefs.SetInt("PlayTutorial", reset ? 1 : 0);
    }

    public void ChangeSelectedButton(GameObject button)
    {
        // Trigger deselect for currently selected object

        if (button.name != "Dummy Object")
        {
            _eventSystem.SetSelectedGameObject(button);
        }
        else
            _eventSystem.SetSelectedGameObject(null);


    }

    public void OnDeviceChange(string humanScheme, string robotScheme)
    {
        if (humanScheme == "keyboard")
        {
            foreach (GameObject item in humanGamepadActions)
            {
                item.SetActive(false);
            }
            foreach(GameObject item in humanKeyboardActions)
            {
                item.SetActive(true);
            }
        }
        else if (humanScheme == "controller")
        {
            foreach (GameObject item in humanGamepadActions)
            {
                item.SetActive(true);
            }
            foreach (GameObject item in humanKeyboardActions)
            {
                item.SetActive(false);
            }
        }
        if (robotScheme == "keyboard")
        {
            foreach (GameObject item in robotGamepadActions)
            {
                item.SetActive(false);
            }
            foreach (GameObject item in robotKeyboardActions)
            {
                item.SetActive(true);
            }
        }
        else if (robotScheme == "controller")
        {
            foreach (GameObject item in robotGamepadActions)
            {
                item.SetActive(true);
            }
            foreach (GameObject item in robotKeyboardActions)
            {
                item.SetActive(false);
            }
        }
    }
    #region Audio Methods
    public void ChangeMusicVolume(Single value)
    {
        VolumeChange(value, "Music");
    }

    public void ChangeSfxVolume(Single value)
    {
        VolumeChange(value, "SFX");
    }

    public void SetSettingsSliderFromPrefs()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            VolumeChange(musicSlider.value, "Music");
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
            VolumeChange(musicSlider.value, "SFX");
        }
    }

    private void VolumeChange (Single audioValue, string prefsText)
    {
        if (audioValue == 0) audioValue = 0.0001f;
        masterMixer.SetFloat(prefsText, Mathf.Log10(audioValue) * 20 );
        PlayerPrefs.SetFloat(prefsText + "Volume", audioValue);
    }
    #endregion

    public void QuitGame()
    {
        Application.Quit();
    }
}
