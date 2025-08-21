using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailTornAppearance : MonoBehaviour
{
    public bool IsMailTornAppearnace = false;
    [SerializeField] private Sprite[] _mailAppearnace;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = IsMailTornAppearnace ? _mailAppearnace[1] : _mailAppearnace[0];
    }

}
