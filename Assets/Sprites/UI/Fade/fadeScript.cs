using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeScript : MonoBehaviour
{
    [HideInInspector] public void setAsLasttSibling()
    {
        gameObject.transform.SetAsLastSibling();
    }
    [HideInInspector] public void setAsFirstSibling()
    {
        gameObject.transform.SetAsFirstSibling();
    }
}
