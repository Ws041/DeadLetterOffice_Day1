using interactObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UI;

//Destroy a black letterbox after player finishes reading the letter

//This script is no longer in current playtesting, as I've switched to putting word contents on the letter itself instead of 
//generating a black box beneath and writing out the letter's content.
//However, you can still see this script in action in the first itch.io demo!

//I keep this in case
public class DestroyLetterBoxAfterRead : MonoBehaviour
{

    private GameObject _gameObjectToCheck;
    private ReviewSheetSpawner _reviewSheetSpawner;
    private LetterReader _letter;
    private ScoreTracker _scoreTracker;

    public void Start()
    {
        _gameObjectToCheck = transform.parent.Find("Text").gameObject;
        _reviewSheetSpawner = GameObject.FindGameObjectWithTag("ReviewSheetSpawner").GetComponent<ReviewSheetSpawner>();
        _letter = GameObject.FindGameObjectWithTag("Letter").GetComponent<LetterReader>();
        _scoreTracker = GameObject.FindGameObjectWithTag("ScoreTracker").GetComponent<ScoreTracker>();
    }

    private void Update()
    {
        DestroyParent();
    }

    public void DestroyParent()
    {
        if (_gameObjectToCheck.activeInHierarchy) return;
        if (_scoreTracker.IsDay1Tutorial) 
            //Relates to cutscene "Spawn Review Sheet", when player finishes reading letter
            //Then play the cutscene where coworker gives you Review Sheet
        {
            GameObject.FindGameObjectWithTag("T_D1SpawnReviewSheet")?.GetComponent<PlayableDirector>().Play(); 
            //Play cutscene of coworker giving you review sheet
        }
        else
        {
            _reviewSheetSpawner.GenerateReviewSheet();
        }

        _letter.IsLetterBeingRead = false;
        // Find the Letterbox object by tag and destroy it, closing the UI
        Destroy(GameObject.FindGameObjectWithTag("Letterbox"));
        return;
    }

}
