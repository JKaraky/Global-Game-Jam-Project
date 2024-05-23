using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

    private void Start()
    {
        LoadSettings();
    }

    void LoadSettings()
    {
        FieldInfo[] fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

        foreach (var field in fields)
        {
            string key = ToCamelCase(field.Name);
            if (PlayerPrefs.HasKey(key))
            {
                if (field.FieldType == typeof(float))
                {
                    field.SetValue(this, PlayerPrefs.GetFloat(key));
                }
                else if (field.FieldType == typeof(int))
                {
                    field.SetValue(this, PlayerPrefs.GetInt(key));
                }
            }
        }
    }

    string ToCamelCase(string str)
    {
        if (string.IsNullOrEmpty(str) || char.IsUpper(str[0]))
        {
            return str;
        }

        string camelCase = char.ToUpper(str[0]).ToString();
        if (str.Length > 1)
        {
            camelCase += str.Substring(1);
        }
        return camelCase;
    }
}
