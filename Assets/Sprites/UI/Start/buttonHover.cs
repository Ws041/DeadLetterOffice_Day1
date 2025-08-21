using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class buttonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<TMP_Text>().color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<TMP_Text>().color = new Color32(195, 190, 150, 225);
    }


}
