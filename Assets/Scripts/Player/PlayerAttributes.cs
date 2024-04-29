using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    #region Variables
    [Header("General Settings")]
    [SerializeField]
    public float maxEnergy = 40;
    [SerializeField]
    public float moveEnergyConsumption = 0.2f;
    [SerializeField]
    public float energyCooldown = 1.2f;
    [SerializeField]
    public float energyRegenerationRate = 0.5f;


    [Header("Movement")]
    [SerializeField]
    public float speed = 60;
    [SerializeField]
    public float gravityMultiplier = 2.5f;

    [Header("Destroying Enemy Powerup")]
    [SerializeField]
    public float destructionRadius = 30;
    [SerializeField]
    [Tooltip("The ratio of max energy that this action will use. For example, 1 is all energy, 2 is half, etc...")]
    public int destructionEnergyConsumptionRatio = 1;

    [Header("Jam Canon")]
    [SerializeField]
    public float jamCooldown = 2; // How long does a player stay hampered
    [SerializeField]
    [Tooltip("The ratio of max energy that this action will use. For example, 1 is all energy, 2 is half, etc...")]
    public int jamEnergyConsumptionRatio = 3;
    #endregion
}
