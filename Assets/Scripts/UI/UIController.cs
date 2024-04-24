using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Energy Bars")]
    [SerializeField]
    private GameObject player1EnergyBarEmpty;
    [SerializeField]
    private Image player1EnergyBar1;
    [SerializeField]
    private Image player1EnergyBar2;
    [SerializeField]
    private GameObject player2EnergyBarEmpty;
    [SerializeField]
    private Image player2EnergyBar1;
    [SerializeField]
    private Image player2EnergyBar2;

    [Header("Abilities Icons")]
    [SerializeField]
    private GameObject humanJam;
    [SerializeField]
    private GameObject humanCannon;
    [SerializeField]
    private GameObject robotJam;
    [SerializeField]
    private GameObject robotCannon;

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

    [Header("Pause Menu")]
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private InputActionReference pauseButton;
    [SerializeField]
    private GameObject resumeButton;

    private int player1EnergyBarsActive = 1;
    private int player2EnergyBarsActive = 1;
    
    private Action<InputAction.CallbackContext> pauseTrigger;
    private void Awake()
    {
        pauseTrigger = (ctx) => PauseMenu();
    }
    private void UpdateEnergyBar(int player, float energy, float maxEnergy)
    {
        float barMax;
        //Debug.Log("Current energy: " + energy + " , max energy: " + maxEnergy);
        if (player == 0)
        {
            barMax = maxEnergy / player1EnergyBarsActive;
            if (energy > barMax)
            {
                player1EnergyBar2.fillAmount = (energy - barMax) / barMax;
            }
            else
            {
                player1EnergyBar1.fillAmount = energy / barMax;
            }
        }
        else if (player == 1)
        {
            barMax = maxEnergy / player2EnergyBarsActive;
            if (energy > barMax)
            {
                player2EnergyBar2.fillAmount = (energy - barMax) / barMax;
            }
            else
            {
                player2EnergyBar1.fillAmount = energy / barMax;
            }
        }
    }
    private void UpdateEnergyBarNumber(int avatar, int player)
    {
        if (player == 0)
        {
            player1EnergyBarsActive = 2;
            player1EnergyBarEmpty.SetActive(true);

            player2EnergyBarsActive = 1;
            player2EnergyBarEmpty.SetActive(false);
        }
        else
        {
            player2EnergyBarsActive = 2;
            player2EnergyBarEmpty.SetActive(true);

            player1EnergyBarsActive = 1;
            player1EnergyBarEmpty.SetActive(false);
        }
    }

    private void ToggleJamIcon(int player, bool enabled)
    {
        if (enabled)
        {
            if(player == 0)
            {
                humanJam.SetActive(true);
            }
            else
            {
                robotJam.SetActive(true);
            }
        }
        else
        {
            if(player == 0)
            {
                humanJam.SetActive(false);
            }
            else
            {
                robotJam.SetActive(false);
            }
        }
    }

    private void ToggleCannonIcon(int player, bool enabled)
    {
        if (enabled)
        {
            if (player == 0) 
            { 
                humanCannon.SetActive(true); 
            }
            else
            {
                robotCannon.SetActive(true);
            } 
        }
        else
        {
            if(player == 0)
            {
                humanCannon.SetActive(false);
            }
            else
            {
                robotCannon.SetActive(false);
            }
        }
    }

    public void PauseMenu()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(resumeButton);

            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);

            Cursor.visible = false;
        }
    }
    public void OnDeviceChange(string humanScheme, string robotScheme)
    {
        if (humanScheme == "keyboard")
        {
            foreach (GameObject item in humanGamepadActions)
            {
                item.SetActive(false);
            }
            foreach (GameObject item in humanKeyboardActions)
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

    private void OnEnable()
    {
        AvatarController.RefreshEnergyBarTrigger += UpdateEnergyBar;
        AvatarController.toggleCannonIcon += ToggleCannonIcon;
        AvatarController.toggleJamIcon += ToggleJamIcon;
        pauseButton.action.performed += pauseTrigger;
    }

    private void OnDisable()
    {
        AvatarController.RefreshEnergyBarTrigger -= UpdateEnergyBar;
        AvatarController.toggleCannonIcon -= ToggleCannonIcon;
        AvatarController.toggleJamIcon -= ToggleJamIcon;
        pauseButton.action.performed -= pauseTrigger;
    }
}
