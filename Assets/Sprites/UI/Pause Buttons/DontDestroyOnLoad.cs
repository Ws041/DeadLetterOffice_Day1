using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Assigned to Audiopool and Pause Screen UI
public class DontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
