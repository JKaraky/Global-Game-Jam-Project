using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class ChangeFont : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> HumanText;
    [SerializeField]
    private List<TextMeshProUGUI> RobotText;
    [SerializeField]
    private TMP_FontAsset humanFont;
    [SerializeField]
    private TMP_FontAsset robotFont;
    void Awake()
    {
        FontChange();
    }

    // Update is called once per frame
    void Update()
    {
        FontChange();
    }

    private void FontChange()
    {
        foreach (TextMeshProUGUI text in HumanText)
        {
            text.font = humanFont;
        }
        foreach (TextMeshProUGUI text in RobotText)
        {
            text.font = robotFont;
        }
    }
}
