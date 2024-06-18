using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DevScript : MonoBehaviour
{
    private Slider slider;
    private TextMeshProUGUI valueText;
    private string prefName;
    private float defaultFloatValue;
    private int defaultIntValue;
    [SerializeField]
    private PlayerValues playerValues;
    [SerializeField]
    private PlayerValues defaultValues;

    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
        prefName = slider.name;
        valueText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        if (slider.wholeNumbers)
        {
            //slider.value = PlayerPrefs.GetInt(prefName);
            slider.value = playerValues.GetIntFromName(prefName);
            defaultIntValue = defaultValues.GetIntFromName(prefName);
        }
        else
        {
            //slider.value = PlayerPrefs.GetFloat(prefName);
            slider.value = playerValues.GetFloatFromName(prefName);
            defaultFloatValue = defaultValues.GetFloatFromName(prefName);
        }

        valueText.text = slider.value.ToString();

        slider.onValueChanged.AddListener (delegate { SetSliderToPrefs(); });
    }
    public void SetSliderToPrefs ()
    {
        valueText.text = slider.value.ToString();

        if (slider.wholeNumbers)
        {
            //PlayerPrefs.SetInt(prefName, (int)slider.value);
            playerValues.SetIntField(prefName, (int)slider.value);
        }
        else
        {
            //PlayerPrefs.SetFloat(prefName, slider.value);
            playerValues.SetFloatField(prefName, slider.value);
        }
    }

    private void ResetToDefault()
    {
        if (slider.wholeNumbers)
        {
            slider.value = defaultIntValue;
        }
        else
        {
            slider.value = defaultFloatValue;
        }

        valueText.text = slider.value.ToString();
    }

    private void OnEnable()
    {
        ResetPlayerValues.ResetSliderValues += ResetToDefault;
    }
    private void OnDisable()
    {
        ResetPlayerValues.ResetSliderValues -= ResetToDefault;
    }
}
