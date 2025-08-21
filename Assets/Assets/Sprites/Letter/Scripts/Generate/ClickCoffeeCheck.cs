using mailGenerator;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

//Player can check and submit their mail and reviewsheet once they're done, by clicking on the coffee cup
public class ClickCoffeeCheck : MonoBehaviour
{
    private CheckReviewAccuracy _checkReviewAccuracy;
    private ReviewSheetSpawner _reviewSheetSpawner;
    private bool _shouldSpawn = true;
    private ScoreTracker _scoreTracker;
    private AudioSourcePool _audioSourcePool;

    private void Awake()
    {
        _reviewSheetSpawner = GameObject.FindGameObjectWithTag("ReviewSheetSpawner").GetComponent<ReviewSheetSpawner>();
        _scoreTracker = GameObject.FindGameObjectWithTag("ScoreTracker").GetComponent<ScoreTracker>();
        _audioSourcePool = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<AudioSourcePool>();
    }

    private void OnMouseDown()
    {
        _checkReviewAccuracy = GameObject.FindGameObjectWithTag("ReviewSheet")?.gameObject.GetComponent<CheckReviewAccuracy>();
        if (_checkReviewAccuracy == null) return;
        _audioSourcePool.SFX_CupClink.Play();

        //Guidance text only appears in the initial stages of the game
        //Tells player to click on coffee to submit documents
        GameObject.FindGameObjectWithTag("GuidanceText")?.SetActive(false);

        _checkReviewAccuracy.checkAccuracy();
        _prepareNextBatch();

        if (_scoreTracker.MailCounter + 1 > _scoreTracker.MailGoal)
        {
            _shouldSpawn = false;
            _scoreTracker.MailCounter++;
        }

        else if (!_scoreTracker.IsDay1Tutorial && _shouldSpawn) {
            //Generate mail regularly if it's not day1 tutorial
            StartCoroutine(_generatNextMail());
        }
        else if (_scoreTracker.IsDay1Tutorial)
        {
            //If it's day1 tutorial, then play one of these two timelines
            //In this case, Day1Tutorial checks for after player processes first mail in cutscene
            if (_scoreTracker.MailIncorrect == 1)
            {
                _scoreTracker.playTimeline("T_D1FinalDialoguePenalty");
                return;
            }
            _scoreTracker.playTimeline("T_D1FinalDialogue");
        }

    }

    //Sends new batch of mail, letter, and review sheet
    private void _prepareNextBatch()
    {
        GameObject reviewSheetSpawner = GameObject.FindGameObjectWithTag("ReviewSheetSpawner");
        reviewSheetSpawner.GetComponent<ReviewSheetSpawner>().HasGeneratedReviewSheet = false;

        List<GameObject> objectsToDestroy = new();
        objectsToDestroy.AddRange(GameObject.FindGameObjectsWithTag("Mail").OfType<GameObject>().ToList());
        objectsToDestroy.AddRange(GameObject.FindGameObjectsWithTag("Letter").OfType<GameObject>().ToList());
        objectsToDestroy.Add(GameObject.FindGameObjectWithTag("ReviewSheet"));

        foreach (GameObject obj in objectsToDestroy)
        {
            obj.AddComponent<SlideAwayMovement>();
        }

        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj, 1.5f);
        }
    }

    private IEnumerator _generatNextMail()
    {
        yield return new WaitForSeconds(1.5f);
        MailGenerator mailGenerator = GameObject.FindGameObjectWithTag("MailGenerator").GetComponent<MailGenerator>();
        mailGenerator.GenerateMail(); //Accesses its random generator
        _reviewSheetSpawner.GenerateReviewSheet();
    }

    public void GenerateNextMail()
    {
        StartCoroutine(_generatNextMail());
    }

}
