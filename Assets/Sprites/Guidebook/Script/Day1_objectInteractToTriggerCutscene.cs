using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Day1_objectInteractToTriggerCutscene : MonoBehaviour
{
    private bool hasClicked = true;

    private void Awake()
    {
        hasClicked = true;
}
    private void OnMouseDown()
    {
        if (!hasClicked)
        {
            GameObject.FindGameObjectWithTag("T_FirstDialogueOfDay").GetComponent<PlayableDirector>().Play();
            hasClicked = true;
        }
    }
    [HideInInspector] public void ChangeHasClickedBool()
    {
        hasClicked = false;
    }
        
    
}
