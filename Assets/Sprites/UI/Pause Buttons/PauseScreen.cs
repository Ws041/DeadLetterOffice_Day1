using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A script...to pause the game...I couldn't have guessed!
public class PauseScreen : MonoBehaviour
{
    public bool IsGamePaused = false;
    [SerializeField] private GameObject _pauseUI;
    private AudioSourcePool _audioSourcePool;

    private void Awake()
    {
        IsGamePaused = false;
        _audioSourcePool = GetComponent<AudioSourcePool>();
    }
    private void Start()
    {
        _resumeGame(); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //Escape key's the key!
        {
            _audioSourcePool.SFX_ButtonPullup.Play();
            if (IsGamePaused)
            {
                _resumeGame();
            }
            else
            {
                _pauseGame();
            }
        }
    }

    private void _pauseGame()
    {
        Time.timeScale = 0f;
        _pauseUI.SetActive(true);
        IsGamePaused = true;
    }

    private void _resumeGame()
    {
        Time.timeScale = 1f;
        _pauseUI.SetActive(false);
        IsGamePaused = false;
    }
}
