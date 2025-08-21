using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Accessed when Pause UI is enabled
//Changes volume when player pauses game and adjust settings
public class ChangeVolume : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private TMP_Text _text;
    [SerializeField] private Image _fill;
    private float _currentAmount = 1f; 
    private string _parentName;

    public float MaxAmount = 1f;

    //Public so it can be accessed by either < or > gameobjects, so they can update each other's CurrentAmount
    public float CurrentAmount {
        get => _currentAmount;
        set {
            if (value <= 0f)
            {
                _currentAmount = 0f;
                return;
            }
            if (value >= MaxAmount)
            {
                _currentAmount = MaxAmount;
                return;
            }
            _currentAmount = value;
        }
    }

    public ChangeVolume ChangeVolume_script;
    private float _fillAmount;
    private AudioSourcePool _audioSourcePool;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _text.color = Color.white;
        _audioSourcePool = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<AudioSourcePool>();
        

        _parentName = transform.parent.gameObject.name;
        MaxAmount = 1f;
        CurrentAmount = 1f; //Max volume from the start
        
    }
    private void Start()
    {
        _text.color = Color.white;
        _updateFillAmount();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _audioSourcePool.SFX_PaperFold.Play();
        _text.color = new Color32(195, 190, 150, 225);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _text.color = Color.white;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _audioSourcePool.SFX_ButtonPress.Play();
        if (gameObject.name == "<")
        {
            CurrentAmount -= 0.1f;
            
        }
        else if (gameObject.name == ">")
        {
            CurrentAmount += 0.1f;
        }
        _updateFillAmount();

        _changeSFXVolume();
        _changeBGVolume();
    }

    // Updates the visual fill bar and synchronizes the value with the sibling script
    private void _updateFillAmount()
    {
        ChangeVolume_script.CurrentAmount = CurrentAmount;
        _fillAmount = CurrentAmount / MaxAmount;
        _fill.fillAmount = _fillAmount;
    }

    //Two different types of volumes (SFX and BG ambiance) cause players like it seperated
    private void _changeSFXVolume() {
        if (_parentName != "SFX") return;
        _audioSourcePool.ChangeSFXVolume(_fillAmount);
    }

    private void _changeBGVolume()
    {
        if (_parentName != "BG Noise") return;
        _audioSourcePool.ChangeBGVolume(_fillAmount);
    }
}
