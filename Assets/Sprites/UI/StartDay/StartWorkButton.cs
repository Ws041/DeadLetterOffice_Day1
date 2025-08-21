using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StartWorkButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    public UnityEvent EventsToEnable;
    private AudioSourcePool _audioSourcePool;
    private void Awake()
    {
        _audioSourcePool = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<AudioSourcePool>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (EventsToEnable!= null) EventsToEnable.Invoke();
        _audioSourcePool.SFX_ButtonPress.Play();
    }

    private void Start()
    {
        _audioSourcePool.SFX_ButtonPress.Play();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _audioSourcePool.SFX_PaperFold.Play();
    }
}
