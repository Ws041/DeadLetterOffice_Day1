using mailGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class ScoreTracker : MonoBehaviour
{
    public int MailGoal = 2;
    [HideInInspector] public int MailCorrect = 0;
    [HideInInspector] public int MailIncorrect = 0;
    public bool IsStartDay = false;
    public int DayNum = 1;
    private int _mailCounter = 0;
    public bool IsDay1Tutorial = false; //If day 1, set to true in Inspector

    private MailGenerator _mailGenerator;
    public int MailCounter {
        get => _mailCounter;
        set {
            if (value > MailGoal) //Check the NEW value, not old one
            {
                _mailCounter = MailGoal;
                playTimeline("T_EndDay");
                
                Debug.Log($"Number of correct mails: {MailCorrect}");
                Debug.Log($"Number of incorrect mails: {MailIncorrect}");
                return;
            }
            _mailCounter = value;
        }
    
    }

    [HideInInspector] public void ChangeIsStartDayBool()
    {
        StartCoroutine(ChangeIsStartDayBool_Coroutine());
    }

    private IEnumerator ChangeIsStartDayBool_Coroutine()
    {
        yield return new WaitForSeconds(3.5f);
        IsStartDay = true;
    }


    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true);
        cacheData();
    }

    private void cacheData()
    {
        _mailGenerator = GameObject.FindGameObjectWithTag("MailGenerator").GetComponent<MailGenerator>();

    }

    [HideInInspector] public void setMoney()
    {
        TMP_Text money = GameObject.FindGameObjectWithTag("UI_Money").GetComponent<TMP_Text>();
        TMP_Text totalMoney = GameObject.FindGameObjectWithTag("UI_TotalMoney").GetComponent<TMP_Text>();
        TMP_Text contents = GameObject.FindGameObjectWithTag("UI_Contents").GetComponent<TMP_Text>();

        int salary = MailCorrect * 10;
        int penalty = MailIncorrect * 5;
        if (MailIncorrect == 1) penalty = 0;
        int sumMoney = salary - (penalty + 5 * 3);

        string displaySumMoney = sumMoney >= 0 ? $"${sumMoney}" : $"-${Mathf.Abs(sumMoney)}";
        totalMoney.text = displaySumMoney;

        evaluateTextColor(contents, money, salary, penalty);
        

    }

    private void evaluateTextColor(TMP_Text contents, TMP_Text money, int salary, int penalty)
    {
        if (MailIncorrect >= 1)
        {
            contents.text = $"SALARY ({MailCorrect})\r\n" +
            $"PENALTY ({MailIncorrect})\r\n" +
            "<color=#5D6F38>RENT</color>\r\n" +
            "<color=#5D6F38>FOOD</color>\r\n" +
            "<color=#5D6F38>HEAT</color>";

            money.text =
            $"{salary}\r\n" +                              //MailCorrect (Salary)
            $"-{penalty}\r\n" +                            //MailIncorrect (Penalty)
            $"<color=#5D6F38>-5</color>\r\n" +             //Rent
            $"<color=#5D6F38>-5</color>\r\n" +             //Food
            $"<color=#5D6F38>-5</color>\r\n"               //Heat
            ;

            return;
        }
        contents.text = $"SALARY ({MailCorrect})\r\n" +
                $"<color=#5D6F38>PENALTY ({MailIncorrect})</color>\r\n" +
                "<color=#5D6F38>RENT</color>\r\n" +
                "<color=#5D6F38>FOOD</color>\r\n" +
                "<color=#5D6F38>HEAT</color>";

        money.text =
            $"{salary}\r\n" +                              //MailCorrect (Salary)
            $"<color=#5D6F38>-{penalty}</color>\r\n" +     //MailIncorrect (Penalty)
            $"<color=#5D6F38>-5</color>\r\n" +             //Rent
            $"<color=#5D6F38>-5</color>\r\n" +             //Food
            $"<color=#5D6F38>-5</color>\r\n"               //Heat
            ;
    }

    [HideInInspector] public void playTimeline(string tagName)
    {
        PlayableDirector timeline = GameObject.FindGameObjectWithTag(tagName).GetComponent<PlayableDirector>();
        if (timeline != null)
        {
            timeline.Play();
        }
        else Debug.Log($"Timeline {tagName} component is null");
    }

    [HideInInspector]
    public void ResetDay1TutorialBool()
    {
        IsDay1Tutorial = false;
    }

}
