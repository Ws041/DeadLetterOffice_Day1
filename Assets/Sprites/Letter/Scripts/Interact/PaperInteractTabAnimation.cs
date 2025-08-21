using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace interactObjects
{
    public class PaperInteractTabAnimation : MonoBehaviour
    {
        //When click tab on object, object does a boink (up-and-down) animation effect
        //Inherited by MailOpener and LetterReader
        protected IEnumerator _letterInteracted()
        {
            Vector3 targetPos = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            Vector3 ogPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.7f);
            yield return new WaitForSeconds(0.01f);
            transform.position = Vector3.Lerp(transform.position, ogPos, 0.7f);
        }
    }
}