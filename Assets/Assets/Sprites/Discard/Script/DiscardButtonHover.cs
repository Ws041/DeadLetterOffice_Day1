using mailGenerator;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DiscardButtonClick : MonoBehaviour
{
    private ScoreTracker _scoreTracker;
    private bool _shouldSpawn = true;
    private AudioSourcePool _audioSourcePool;

    private void Start()
    {
        _audioSourcePool.SFX_ButtonPullup.Play();
    }

    private void Awake()
    {
        _scoreTracker = GameObject.FindGameObjectWithTag("ScoreTracker").GetComponent<ScoreTracker>();
        _audioSourcePool = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<AudioSourcePool>();
    }
    private void OnMouseDown()
    {
        _prepareNextBatch();
        _audioSourcePool.SFX_ButtonPress.Play();
        if (_scoreTracker.MailCounter + 1 > _scoreTracker.MailGoal)
        {
            _shouldSpawn = false;
            _scoreTracker.MailCounter++;
            return;
        }
        else if (_shouldSpawn) StartCoroutine(_generatNextMail());

        
    }

    private void _prepareNextBatch()
    {
        GameObject spawnReviewSheet = GameObject.FindGameObjectWithTag("ReviewSheetSpawner");
        spawnReviewSheet.GetComponent<ReviewSheetSpawner>().HasGeneratedReviewSheet = false;

        List<GameObject> objectsToDestroy = new();
        // Add multiple tagged objects (with null-safe filtering)
        objectsToDestroy.AddRange(GameObject.FindGameObjectsWithTag("Mail").Where(x => x != null));
        objectsToDestroy.AddRange(GameObject.FindGameObjectsWithTag("Letter").Where(x => x != null));

        // Add single tagged object (with null check)
        var reviewSheet = GameObject.FindGameObjectWithTag("ReviewSheet");
        if (reviewSheet != null) objectsToDestroy.Add(reviewSheet);

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
        MailGenerator _generateMail = GameObject.FindGameObjectWithTag("MailGenerator").GetComponent<MailGenerator>();
        _generateMail.GenerateMail();
        ReviewSheetSpawner _reviewSheetSpawner = GameObject.FindGameObjectWithTag("ReviewSheetSpawner").GetComponent<ReviewSheetSpawner>();
        _reviewSheetSpawner.GenerateReviewSheet();
    }

    private void _removeAnimComponent()
    {
        Destroy(GetComponent<Animator>());
    }
}
