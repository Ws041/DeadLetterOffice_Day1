using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class guidanceText : MonoBehaviour
{
    private TMP_Text guideText;
    private void Awake()
    {
        guideText = GetComponent<TMP_Text>();
    }

    [HideInInspector] public void onTabGuide()
    {
        guideText.text = "Press [Tab] to open and read mails";
    }

    [HideInInspector]
    public void clickCoffeeGuide_SubmitReviewSheet()
    {
        guideText.text = "When done, click on coffee to submit the documents";
    }
}
