using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class nextDay : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
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
        _audioSourcePool.SFX_ButtonPress.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _audioSourcePool.SFX_PaperFold.Play();
    }
}
