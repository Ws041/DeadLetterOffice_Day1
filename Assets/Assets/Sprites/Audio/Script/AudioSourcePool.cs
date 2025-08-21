using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool : MonoBehaviour
{
    //Anything private is _var
    //Anything public is Var
    //Only parameters is var (no underscore)

    //Declaring Audiosources
    public AudioSource SFX_PaperFlip;
    public AudioSource SFX_PaperUnFold;
    public AudioSource SFX_PaperFold;
    public AudioSource SFX_PaperPickUp;
    public AudioSource SFX_PaperSlide;

    public AudioSource SFX_ButtonPullup;
    public AudioSource SFX_ButtonPress;

    public AudioSource SFX_CupClink;
    public AudioSource SFX_CupSlide1;
    public AudioSource SFX_CupSlide2;

    public AudioSource SFX_Coworker;
    public AudioSource SFX_Typewriter1;

    public AudioSource SFX_DoorKnock1;
    public AudioSource SFX_DoorKnock2;
    public AudioSource SFX_DoorOpen;
    public AudioSource SFX_DoorOpenClose;

    public AudioSource SFX_LetterboxText;
    public AudioSource SFX_PrintTicket;

    public AudioSource AmbianceBG;

    //Declaring max volumes (adjustable in Inspector)
    private float _maxPaperFlip;
    private float _maxPaperUnfold;
    private float _maxPaperFold;
    private float _maxPaperPickup;
    private float _maxPaperSlide;
    private float _maxButtonPullup;
    private float _maxButtonPress;
    private float _maxCupClink;
    private float _maxCupSlide1;
    private float _maxCupSlide2;
    private float _maxCoworker;
    private float _maxTypewriter1;
    private float _maxDoorKnock1;
    private float _maxDoorKnock2;
    private float _maxDoorOpen;
    private float _maxLetterboxText;
    private float _maxPrintTicket;
    

    private float _maxAmbianceBG;

    // Tuple array for efficient volume updates
    private (AudioSource source, float maxVolume)[] _sfxPairs;
    [HideInInspector] public float CurrentVolumeFractionSFX = 1f, CurrentVolumeFractionBG = 1f;


    private void Start()
    {
        _setDefaultVolumes();
    }

    //Set default (aka max) volumes by changing them in the inspector (serializefields)
    private void _setDefaultVolumes()
    {
        // Set initial max volumes
        _maxPaperFlip = SFX_PaperFlip.volume;
        _maxPaperUnfold = SFX_PaperUnFold.volume;
        _maxPaperFold = SFX_PaperFold.volume;
        _maxPaperPickup = SFX_PaperPickUp.volume;
        _maxPaperSlide = SFX_PaperSlide.volume;
        _maxButtonPullup = SFX_ButtonPullup.volume;
        _maxButtonPress = SFX_ButtonPress.volume;
        _maxCupClink = SFX_CupClink.volume;
        _maxCupSlide1 = SFX_CupSlide1.volume;
        _maxCupSlide2 = SFX_CupSlide2.volume;
        _maxCoworker = SFX_Coworker.volume;
        _maxTypewriter1 = SFX_Typewriter1.volume;
        _maxDoorKnock1 = SFX_DoorKnock1.volume;
        _maxDoorKnock2 = SFX_DoorKnock2.volume;
        _maxDoorOpen = SFX_DoorOpen.volume;
        _maxLetterboxText = SFX_LetterboxText.volume;
        _maxPrintTicket = SFX_PrintTicket.volume;
        _maxAmbianceBG = AmbianceBG.volume;

        // Initialize the tuple array (pairs each AudioSource with its max volume)
        _sfxPairs = new (AudioSource, float)[]
        {
            (SFX_PaperFlip, _maxPaperFlip),
            (SFX_PaperUnFold, _maxPaperUnfold),
            (SFX_PaperFold, _maxPaperFold),
            (SFX_PaperPickUp, _maxPaperPickup),
            (SFX_PaperSlide, _maxPaperSlide),
            (SFX_ButtonPullup, _maxButtonPullup),
            (SFX_ButtonPress, _maxButtonPress),
            (SFX_CupClink, _maxCupClink),
            (SFX_CupSlide1, _maxCupSlide1),
            (SFX_CupSlide2, _maxCupSlide2),
            (SFX_Coworker, _maxCoworker),
            (SFX_Typewriter1, _maxTypewriter1),
            (SFX_DoorKnock1, _maxDoorKnock1),
            (SFX_DoorKnock2, _maxDoorKnock2),
            (SFX_DoorOpen, _maxDoorOpen),
            (SFX_LetterboxText, _maxLetterboxText),
            (SFX_PrintTicket, _maxPrintTicket)
        };
    }

    //Change SFX sound affects when screen is pause. 
    public void ChangeSFXVolume(float fractionVolume) //fractionVolume 0-1 range
    {
        foreach (var (source, maxVolume) in _sfxPairs)
        {
            source.volume = fractionVolume * maxVolume;
        }
    }

    //Change BG sound ambiance when screen is pause. 
    public void ChangeBGVolume(float fractionVolume)  //fractionVolume 0-1 range
    {
        AmbianceBG.volume = fractionVolume * _maxAmbianceBG;
    }

    private void Awake()
    {
        if (gameObject.CompareTag("AudioPool")) DontDestroyOnLoad(this);
        Screen.SetResolution(1920, 1080, true);
    }

    //Methods below are used for when cutscenes want to produce SFX
    //They also need to be public methods, because the cutscenes are on other gameobjects.
    [HideInInspector] public void PlayDoorKnock1()
    {
        SFX_DoorKnock1.Play();
    }

    [HideInInspector]
    public void PlayDoorKnock2()
    {
        SFX_DoorKnock2.Play();
    }

    [HideInInspector]
    public void PlayDoorOpen()
    {
        SFX_DoorOpen.Play();
    }


    [HideInInspector]
    public void PlayDoorOpenClose() {
        SFX_DoorOpenClose.Play();
    }
}
