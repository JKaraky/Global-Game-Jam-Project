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
}
