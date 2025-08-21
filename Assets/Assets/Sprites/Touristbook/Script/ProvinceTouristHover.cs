using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProvinceTouristHover : MonoBehaviour
{
    private TMP_Text _myText;
    private Color32 _ogColor;
    private Color32 _hoverColor;
    private void Awake()
    {
        _myText = GetComponent<TMP_Text>();
        _ogColor = new Color32(188, 183, 132, 255);
        _hoverColor = new Color32(165, 150, 57, 255);

    }

    private void Start()
    {
        _myText.color = _ogColor;
    }

    private void OnMouseEnter()
    {
        _myText.color = _hoverColor;
    }
    private void OnMouseExit()
    {
        _myText.color = _ogColor;
    }
}
