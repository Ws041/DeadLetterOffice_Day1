using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace DialogueSystem
{
    //Class purely for emulating a character-by-character dialogue style
    //Isn't assigned to any gameobjects
    //Used for inheritance
    public class BaseDialogue : MonoBehaviour
    {
        protected IEnumerator WriteText(string input, TMP_Text textHolder, float delay, AudioSource soundSFX = null)
        {
            //New line
            textHolder.text = "";
            for (int i = 0; i < input.Length; i++)
            {
                //Add new character and play sound (stop before next character 
                //because often the delay between characters are smaller than the audio length
                textHolder.text += input[i];
                if (soundSFX != null) soundSFX.Play();
                yield return new WaitForSeconds(delay);
                if (soundSFX != null) soundSFX.Stop();
            }
            //finish a line & wait for user input (GetMouseButtonDown)
            yield return new WaitForSeconds(0.25f);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }
    }
}