using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,ISelectHandler, IDeselectHandler
{
    private GameObject textBubble;
    private void Start()
    {
        textBubble = transform.GetChild(0).gameObject;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        textBubble.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textBubble.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (textBubble != null)
        {
            textBubble.SetActive(true);
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        textBubble.SetActive(false);
    }
}
