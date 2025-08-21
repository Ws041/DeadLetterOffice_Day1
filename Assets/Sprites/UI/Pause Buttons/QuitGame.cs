using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class QuitGame : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private AudioSourcePool _audioSourcePool;
    private TMP_Text _text;
    private void Awake()
    {
        _audioSourcePool = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<AudioSourcePool>();
        _text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        _text.color = Color.white;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _audioSourcePool.SFX_ButtonPress.Play();
        Application.Quit();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _text.color = new Color32(195, 190, 150, 225);
        _audioSourcePool.SFX_PaperFold.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _text.color = Color.white;
    }
}
