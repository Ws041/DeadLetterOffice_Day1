using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mailGenerator
{
    //A debug tool that generates mails whenever you click its assigned game object (which is a baige-colored square)
    public class GenerateMailClicker : MonoBehaviour
    {
        private MailGenerator _mailGenerator;
        private ReviewSheetSpawner _reviewSheetSpawner;

        private void Awake()
        {
            _mailGenerator = GameObject.FindGameObjectWithTag("MailGenerator").GetComponent<MailGenerator>();
            _reviewSheetSpawner = GameObject.FindGameObjectWithTag("ReviewSheetSpawner").GetComponent<ReviewSheetSpawner>();
        }
        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Destroy(GameObject.FindGameObjectWithTag("Mail"));
                Destroy(GameObject.FindGameObjectWithTag("Letter"));
                Destroy(GameObject.FindGameObjectWithTag("ReviewSheet"));
                _reviewSheetSpawner.GenerateReviewSheet();
                _mailGenerator.GenerateMail();
                
            }
        }
    }
}