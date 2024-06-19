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
    private SimplifiedInput playerOneInput;
    [SerializeField]
    private SimplifiedInput playerTwoInput;
    [SerializeField]
    private InputActionReference pauseButtonPlayerOne;
    [SerializeField]
    private InputActionReference pauseButtonPlayerTwo;
    [SerializeField]
    private GameObject resumeButton;

    private Image humanCannonImage, humanJamImage;
    private Image robotCannonImage, robotJamImage;
    // To switch on Boost or Jam when selected
    private GameObject humanJamIcon, humanBoostIcon, robotJamIcon, robotBoostIcon;

    private Color humanEnergyBarColor;
    private Color robotEnergyBarColor;

    private int player1EnergyBarsActive = 1;
    private int player2EnergyBarsActive = 1;
    
    private Action<InputAction.CallbackContext> pauseTrigger;
    private void Awake()
    {
        pauseTrigger = (ctx) => PauseMenu();

        humanEnergyBarColor = player1EnergyBar1.color;
        robotEnergyBarColor = player2EnergyBar1.color;

        humanCannonImage = humanCannon.transform.GetChild(0).GetComponent<Image>();
        humanJamIcon = humanJam.transform.GetChild(0).gameObject;
        humanJamImage = humanJamIcon.GetComponent<Image>();
        humanBoostIcon = humanJam.transform.GetChild(1).gameObject;

        robotCannonImage = robotCannon.transform.GetChild(0).GetComponent<Image>();
        robotJamIcon = robotJam.transform.GetChild(0).gameObject;
        robotJamImage = robotJamIcon.GetComponent<Image>();
        robotBoostIcon = robotJam.transform.GetChild(1).gameObject;
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
            // Paused 

            Time.timeScale = 0f;

            playerOneInput.enabled = false;
            playerTwoInput.enabled = false;

            pauseMenu.SetActive(true);

            Cursor.visible = true;
            EventSystem.current.SetSelectedGameObject(resumeButton);
        }
        else
        {
            // Unpaused

            Time.timeScale = 1f;

            playerOneInput.enabled = true;
            playerTwoInput.enabled = true;

            EventSystem.current.SetSelectedGameObject(null);

            pauseMenu.SetActive(false);

            Cursor.visible = false;
        }
    }
    private void GreyOutUI(int player, bool toggle)
    {
        // True means grey out, false back to normal
        if (toggle)
        {
            if (player == 0)
            {
                player1EnergyBar1.color = Color.gray;
                humanCannonImage.color = Color.gray;
                humanJamImage.color = Color.gray;
            }
            else
            {
                player2EnergyBar1.color = Color.gray;
                robotCannonImage.color = Color.gray;
                robotJamImage.color = Color.gray;
            }
        }
        else
        {
            if (player == 0)
            {
                player1EnergyBar1.color = humanEnergyBarColor;
                humanCannonImage.color = Color.white;
                humanJamImage.color = Color.white;
            }
            else
            {
                player2EnergyBar1.color = robotEnergyBarColor;
                robotCannonImage.color = Color.white;
                robotJamImage.color = Color.white;
            }
        }
    }

    private void SetAbilityIcon(bool abilityHuman, bool abilityRobot)
    {
        // ability true => Jam selected
        // ability false => Boost selected

        humanJamIcon.SetActive(abilityHuman);
        humanBoostIcon.SetActive(!abilityHuman);

        robotJamIcon.SetActive(abilityRobot);
        robotBoostIcon.SetActive(!abilityRobot);
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
        AvatarController.toggleSecondaryActionIcon += ToggleJamIcon;
        AvatarController.greyOutUI += GreyOutUI;
        pauseButtonPlayerOne.action.performed += pauseTrigger;
        pauseButtonPlayerTwo.action.performed += pauseTrigger;
        DeviceCheck.SetPlayerAbility += SetAbilityIcon;
    }

    private void OnDisable()
    {
        AvatarController.RefreshEnergyBarTrigger -= UpdateEnergyBar;
        AvatarController.toggleCannonIcon -= ToggleCannonIcon;
        AvatarController.toggleSecondaryActionIcon -= ToggleJamIcon;
        AvatarController.greyOutUI -= GreyOutUI;
        pauseButtonPlayerOne.action.performed -= pauseTrigger;
        pauseButtonPlayerTwo.action.performed -= pauseTrigger;
        DeviceCheck.SetPlayerAbility -= SetAbilityIcon;
    }
}
