using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerValues : MonoBehaviour
{
    [SerializeField]
    private PlayerValues playerValues;
    [SerializeField]
    private PlayerValues defaultPlayerValues;

    public static event Action ResetSliderValues;

    public void ResetValues()
    {
        playerValues.maxEnergy = defaultPlayerValues.maxEnergy;
        playerValues.moveEnergyConsumption = defaultPlayerValues.moveEnergyConsumption;
        playerValues.energyCooldown = defaultPlayerValues.energyCooldown;
        playerValues.energyRegenerationRate = defaultPlayerValues.energyRegenerationRate;

        playerValues.speed = defaultPlayerValues.speed;
        playerValues.gravityMultiplier = defaultPlayerValues.gravityMultiplier;

        playerValues.destructionRadius = defaultPlayerValues.destructionRadius;
        playerValues.destructionEnergyConsumptionRatio = defaultPlayerValues.destructionEnergyConsumptionRatio;

        playerValues.secondaryActionCooldown = defaultPlayerValues.secondaryActionCooldown;
        playerValues.sAEnergyConsumptionRatio = defaultPlayerValues.sAEnergyConsumptionRatio;

        playerValues.speedBoostMultiplier = defaultPlayerValues.speedBoostMultiplier;

        playerValues.spawnLifeSpan = defaultPlayerValues.spawnLifeSpan;

        ResetSliderValues?.Invoke();
    }
}
