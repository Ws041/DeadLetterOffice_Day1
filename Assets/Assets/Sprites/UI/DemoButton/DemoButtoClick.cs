using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DemoButtoClick : MonoBehaviour, IPointerDownHandler
{
    private AudioSourcePool _audioSourcePool;
    private void Awake()
    {
        _audioSourcePool = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<AudioSourcePool>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _audioSourcePool.SFX_ButtonPress.Play();
        SceneManager.LoadScene(0);
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
