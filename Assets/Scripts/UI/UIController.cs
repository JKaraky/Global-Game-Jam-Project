using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    [Header("Players Slot Indicators")]
    [SerializeField]
    private GameObject[] indicator1;
    [SerializeField]
    private GameObject[] indicator2;

    [Header("Station 1 Control Points")]
    [SerializeField]
    private TextMeshProUGUI p1PtInStation1;
    [SerializeField]
    private TextMeshProUGUI p2PtInStation1;

    [Header("Station 2 Control Points")]
    [SerializeField]
    private TextMeshProUGUI p1PtInStation2;
    [SerializeField]
    private TextMeshProUGUI p2PtInStation2;

    [Header("Station 3 Control Points")]
    [SerializeField]
    private TextMeshProUGUI p1PtInStation3;
    [SerializeField]
    private TextMeshProUGUI p2PtInStation3;

    [Header("Pause Menu")]
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private InputActionReference pauseButton;

    [Header("Avatars")]
    [SerializeField]
    private Avatar[] avatars;

    private Dictionary<int, TextMeshProUGUI[]> avatarTextDictionary;
    private int player1EnergyBarsActive = 1;
    private int player2EnergyBarsActive = 1;
    private void Start()
    {
        avatarTextDictionary = new Dictionary<int, TextMeshProUGUI[]>
        {
            {0, new TextMeshProUGUI[2] {p1PtInStation1, p2PtInStation1 } },
            {1, new TextMeshProUGUI[2] { p1PtInStation2, p2PtInStation2 } },
            {2, new TextMeshProUGUI[2] { p1PtInStation3, p2PtInStation3 } }
        };
    }
    private void UpdateEnergyBar(int player, float energy, float maxEnergy)
    {
        float barMax;
        Debug.Log("Current energy: " + energy + " , max energy: " + maxEnergy);
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

    private void UpdateControlPointSlot(int slot, int player)
    {
        GameObject[] indicators = player == 0 ? indicator1 : indicator2;
        for (int i = 0; i < 3; i++)
        {
            if (i == slot)
                indicators[i].SetActive(true);
            else
                indicators[i].SetActive(false);
        }
    }

    private void UpdateAvatarPoints(int avatar, int player)
    {
        int[] ptsInAvatar = avatars[avatar].PlayersPoints;
        if (avatarTextDictionary.TryGetValue(avatar, out TextMeshProUGUI[] texts))
        {
            texts[0].text = ptsInAvatar[0] + "";
            texts[1].text = ptsInAvatar[1] + "";
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

    private void PauseMenu(InputAction.CallbackContext context)
    {
        // If it's on, it gets turned off and vice versa
        Time.timeScale = 0f;
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    private void OnEnable()
    {
        AvatarController.ControlSlotToggleTrigger += UpdateControlPointSlot;
        AvatarController.RefreshEnergyBarTrigger += UpdateEnergyBar;
        //Avatar.IncreasePoint += UpdateAvatarPoints;
        //Avatar.DecreasePoint += UpdateAvatarPoints;
        Avatar.GainedControl += UpdateEnergyBarNumber;
        pauseButton.action.performed += PauseMenu;
    }

    private void OnDisable()
    {
        AvatarController.ControlSlotToggleTrigger -= UpdateControlPointSlot;
        AvatarController.RefreshEnergyBarTrigger -= UpdateEnergyBar;
        //Avatar.IncreasePoint -= UpdateAvatarPoints;
        //Avatar.DecreasePoint -= UpdateAvatarPoints;
        pauseButton.action.performed -= PauseMenu;
    }
}
