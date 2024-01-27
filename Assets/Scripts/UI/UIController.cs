using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Energy Bars")]
    [SerializeField]
    private Image energyBar1;
    [SerializeField]
    private Image energyBar2;

    [Header("Players Slot Indicators")]
    [SerializeField]
    private GameObject[] indicator1;
    [SerializeField]
    private GameObject[] indicator2;

    [Header("Duck Control Points")]
    [SerializeField]
    private TextMeshProUGUI duckPtInDuck;
    [SerializeField]
    private TextMeshProUGUI wormPtInDuck;

    [Header("Worm Control Points")]
    [SerializeField]
    private TextMeshProUGUI duckPtInWorm;
    [SerializeField]
    private TextMeshProUGUI wormPtInWorm;

    [Header("Robot Control Points")]
    [SerializeField]
    private TextMeshProUGUI duckPtInRobot;
    [SerializeField]
    private TextMeshProUGUI wormPtInRobot;

    [Header("Avatars")]
    [SerializeField]
    private Avatar[] avatars;

    private Dictionary<int, TextMeshProUGUI[]> avatarTextDictionary;
    private void Start()
    {
        avatarTextDictionary = new Dictionary<int, TextMeshProUGUI[]>
        {
            {0, new TextMeshProUGUI[2] {duckPtInDuck, wormPtInDuck } },
            {1, new TextMeshProUGUI[2] { duckPtInWorm, wormPtInWorm } },
            {2, new TextMeshProUGUI[2] { duckPtInRobot, wormPtInRobot } }
        };
    }
    private void UpdateEnergyBar(int player, float energy, float maxEnergy)
    {
        if (player == 0)
        {
            energyBar1.fillAmount = energy/ maxEnergy;
        }
        else if (player == 1)
        {
            energyBar2.fillAmount = energy/ maxEnergy;
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
        Debug.Log("Updating points");
        int[] ptsInAvatar = avatars[avatar].PlayersPoints;
        if (avatarTextDictionary.TryGetValue(avatar, out TextMeshProUGUI[] texts))
        {
            texts[0].text = ptsInAvatar[0] + "";
            texts[1].text = ptsInAvatar[1] + "";
        }
}

    private void OnEnable()
    {
        AvatarController.ControlSlotToggleTrigger += UpdateControlPointSlot;
        AvatarController.RefreshEnergyBarTrigger += UpdateEnergyBar;
        Avatar.IncreasePoint += UpdateAvatarPoints;
        Avatar.DecreasePoint += UpdateAvatarPoints;
    }
    private void OnDisable()
    {
        AvatarController.ControlSlotToggleTrigger -= UpdateControlPointSlot;
        AvatarController.RefreshEnergyBarTrigger -= UpdateEnergyBar;
        Avatar.IncreasePoint -= UpdateAvatarPoints;
        Avatar.DecreasePoint -= UpdateAvatarPoints;
    }
}
