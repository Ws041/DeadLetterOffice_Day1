using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipTouristBookToPage : MonoBehaviour
{
    [SerializeField] private int _pageIndexTarget;
    private TouristbookFlip _touristbookFlip;
    private AudioSourcePool _audioSourcePool;
    private PauseScreen _pauseScreen;
    private void Awake()
    {
        _touristbookFlip = GameObject.FindGameObjectWithTag("TouristBookFlipRight")?.GetComponent<TouristbookFlip>();
        _audioSourcePool = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<AudioSourcePool>();
        _pauseScreen = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<PauseScreen>();
    }
    private void OnMouseDown()
    {
        if (_pauseScreen.IsGamePaused) return;
        _audioSourcePool.SFX_PaperFlip.Play();
        _touristbookFlip.CurrentPageIndex = _pageIndexTarget;
        _touristbookFlip.UpdateTouristBook();
    }

}
