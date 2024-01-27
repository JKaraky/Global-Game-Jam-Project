using System.Collections;
using System.Collections.Generic;
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


    public void UpdateEnergyBar(int player, float energy, float maxEnergy)
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

    public void UpdateControlPointSlot(int slot, int player)
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

    private void OnEnable()
    {
        AvatarController.ControlSlotToggleTrigger += UpdateControlPointSlot;
        AvatarController.RefreshEnergyBarTrigger += UpdateEnergyBar;
    }
    private void OnDisable()
    {
        AvatarController.ControlSlotToggleTrigger -= UpdateControlPointSlot;
        AvatarController.RefreshEnergyBarTrigger -= UpdateEnergyBar;
    }
}
