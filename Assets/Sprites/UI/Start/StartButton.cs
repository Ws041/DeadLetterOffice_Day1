using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler
{
    [SerializeField] private GameObject startGameTimeline;
    private AudioSourcePool _audioSourcePool;
    private void Awake()
    {
        _audioSourcePool = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<AudioSourcePool>();
    }
    private void Start()
    {
        _audioSourcePool.SFX_ButtonPress.Play();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //SceneManager.LoadScene("1_Day1");
        //
    }

    [HideInInspector]
    public void startGame()
    {
        SceneManager.LoadScene("1_Day1");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _audioSourcePool.SFX_ButtonPress.Play();
        startGameTimeline.GetComponent<PlayableDirector>().Play();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        _audioSourcePool.SFX_PaperFold.Play();
    }

}
