using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyObject : MonoBehaviour
{
    private void destroyTextbox()
    {
        GameObject.FindGameObjectWithTag("textbox").SetActive(false);
    }
}
