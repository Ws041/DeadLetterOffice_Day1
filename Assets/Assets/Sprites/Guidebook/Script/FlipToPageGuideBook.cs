using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipToPageGuideBook : MonoBehaviour
{
    [SerializeField] private int _pageIndexTarget;
    [SerializeField] private GuideBookFlip _guideBookFlip;
    private AudioSourcePool _audioSourcePool;
    private PauseScreen _pauseScreen;

    private void Awake()
    {
        _audioSourcePool = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<AudioSourcePool>();
        _pauseScreen = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<PauseScreen>();
    }

    private void OnMouseDown()
    {
        if (_pauseScreen.IsGamePaused) return;
        _audioSourcePool.SFX_PaperFlip.Play();
        _guideBookFlip.CurrentPageIndex = _pageIndexTarget;
        _guideBookFlip.UpdateGuidebookPage();

    }

}
