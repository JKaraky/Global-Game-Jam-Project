using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Variables
    [SerializeField]
    private TextMeshProUGUI humanText;
    [SerializeField]
    private TextMeshProUGUI robotText;
    #endregion

    #region Methods
    public void OnPointerEnter(PointerEventData eventData)
    {
        humanText.gameObject.SetActive(false);
        robotText.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        humanText.gameObject.SetActive(true);
        robotText.gameObject.SetActive(false);
    }
    #endregion


}
