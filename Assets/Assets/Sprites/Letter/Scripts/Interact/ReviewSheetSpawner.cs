using System;
using System.Collections;
using System.Collections.Generic;
using mailGenerator;
using UnityEngine;

public class ReviewSheetSpawner : MonoBehaviour
{
    [Header("Data References")]
    public MainDataset mainDataset;
    [SerializeField] private GameObject _reviewSheet;
    [SerializeField] private Transform _reviewSpawnPos;

    public bool HasGeneratedReviewSheet = false;
    private AudioSourcePool _audioSourcePool;

    private void Awake()
    {
        HasGeneratedReviewSheet = false;
        _audioSourcePool = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<AudioSourcePool>();
    }

    /// <summary>
    /// Generates a review sheet for the current letter
    /// </summary>
    [HideInInspector] public void GenerateReviewSheet()
    {

        if (HasGeneratedReviewSheet) return;
        if (GameObject.FindGameObjectWithTag("Mail") == null) return;

        GameObject _reviewSheetClone = Instantiate(_reviewSheet, _reviewSpawnPos.position, Quaternion.identity);
        _audioSourcePool.SFX_PaperSlide.Play();

        // Scale and parent the review sheet
        float scale = mainDataset.universalSize;
        GameObject _parentObject = GameObject.FindGameObjectWithTag("MoveableArea");
        _reviewSheetClone.transform.localScale = new Vector3(scale, scale, scale);
        _reviewSheetClone.transform.SetParent(_parentObject.transform);
        _reviewSheetClone.name = "ReviewSheet";

        HasGeneratedReviewSheet = true;
    }

    public void DisableTimeline()
    {
        GameObject.FindGameObjectWithTag("T_D1SpawnReviewSheet")?.SetActive(false);
    }

    [HideInInspector]
    public void ChangeReviewSpawnPos()
    {
        _reviewSpawnPos.position = new(3.3f, _reviewSpawnPos.position.y);
    }

}
