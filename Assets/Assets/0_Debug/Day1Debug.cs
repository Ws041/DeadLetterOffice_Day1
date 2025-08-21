using mailGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A debugging tool because I can't keep replaying the same day over and over to catch small mistakes
//Skips all the intro and immediately spawns mail and review sheet
public class Day1Debug : MonoBehaviour
{
    [SerializeField] private bool _skipDay1Intro, _isDebugSpawnMailPos;
    private ScoreTracker _scoreTracker;
    [SerializeField] private GameObject[] _enableForDebug; //Some gameobjects (like "Enable for Individual Scene Debugging")
                                                           //Need to be enabled, or else scripts can't access things like
                                                           //Audiopool and Pause Screen UI
                                                           //Disable "Enable for Individual Scene Debugging" when playing start to finish
    private MailGenerator _mailGenerator;
    private ReviewSheetSpawner _reviewSheetSpawner;


    private void Awake()
    {
        _mailGenerator = GameObject.FindGameObjectWithTag("MailGenerator").GetComponent<MailGenerator>();
        _scoreTracker = GameObject.FindGameObjectWithTag("ScoreTracker").GetComponent<ScoreTracker>();
        _reviewSheetSpawner = GameObject.FindGameObjectWithTag("ReviewSheetSpawner").GetComponent<ReviewSheetSpawner>();
    }

    private void Start()
    {
        if (_isDebugSpawnMailPos) _debugSpawnMailPos();
        if (_skipDay1Intro) _skipIntro();
    }

    private void _skipIntro()
    {
        //Disable all timeline gameobjects via tags
        string[] timelineTagNames = {"T_Intro", "T_FirstDialogueOfDay", "T_D1SpawnReviewSheet", "T_D1FinalDialogue", "T_D1FinalDialoguePenalty"};
        foreach (string tagName in timelineTagNames) {
            GameObject.FindGameObjectWithTag(tagName).gameObject.SetActive(false);
        }
        foreach (GameObject obj in _enableForDebug)
        {
            obj.SetActive(true);
        }
        
        //Set true to these bools because they let player drag and drop objects (this is disabled when the black screen intro is played)
        _scoreTracker.IsStartDay = true;
        _scoreTracker.IsDay1Tutorial = false;
        _scoreTracker.MailCounter = 1;

        _mailGenerator.GenerateMail();
        _reviewSheetSpawner.GenerateReviewSheet();

    }

    private void _debugSpawnMailPos()
    {
        _mailGenerator.ChangeMailSpawnPos();
        _reviewSheetSpawner.ChangeReviewSpawnPos();
    }
}
