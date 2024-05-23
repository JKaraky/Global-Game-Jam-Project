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

    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
        prefName = slider.name;
        valueText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        if (PlayerPrefs.HasKey(prefName) && slider.wholeNumbers)
        {
            slider.value = PlayerPrefs.GetInt(prefName);
        }
        else if (PlayerPrefs.HasKey(prefName))
        {
            slider.value = PlayerPrefs.GetFloat(prefName);
        }

        valueText.text = slider.value.ToString();

        slider.onValueChanged.AddListener (delegate { SetSliderToPrefs(); });
    }
    public void SetSliderToPrefs ()
    {
        valueText.text = slider.value.ToString();

        if (slider.wholeNumbers)
        {
            PlayerPrefs.SetInt(prefName, (int)slider.value);
        }
        else
        {
            PlayerPrefs.SetFloat(prefName, slider.value);
        }
    }
}
