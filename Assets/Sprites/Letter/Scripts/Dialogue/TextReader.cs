using System.Collections;
using System.Collections.Generic;
using interactObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DialogueSystem
{
    //This TextReader is for the text at the beginning of the game that establishes setting:
    //8:00 AM, JULY 12TH, 1989
    //REPUBLIKA DEAD LETTER DEPARTMENT
    //A WEEK INTO THE GRESCHNOVA-KASTAVYE CONFLICT
    //[...] (So on so on, you get the drift. You'll find these texts in UI Canvas > Intro in the scene)

    //Separate script from DialogueTextReader, which does a similar function but for coworker dialogue in the beginning of game
    //DialogueTextReader also has some special stuff like creating textboxes,
    //so I made two seperate scripts instead since they're so different.

    public class TextReader : BaseDialogue
    {
        [SerializeField] UnityEvent eventsToEnable; //Enabling UnityEvent in case
        [SerializeField] private bool _beInactiveAfterClick = true; //If true, user can click on screen and text disappears after last line is shone
        //For the intro text (see example above), it's set to true in the inspector. 

        private TMP_Text _textHolder;
        private AudioSourcePool _audioSourcePool;

        [Header("Text Options")] //Put your dialogue here in inspector
        public List<string> DialogueList;

        [Header("Time Parameters")]
        [SerializeField] private float _delay = 0.03f;
        private AudioSource _typeSound;

        private void Awake()
        {
            _audioSourcePool = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<AudioSourcePool>();

            //Change SFX based on the gameobject, since intro and letterboxtext has different SFX (for aesthetic purposes)
            if (gameObject.CompareTag("IntroText")) _typeSound = _audioSourcePool.SFX_Typewriter1; 
            else if (gameObject.CompareTag("LetterboxText")) _typeSound = _audioSourcePool.SFX_LetterboxText;

            //Start dialogue! Yay!
            InitializeDialogue();
        }

        public void InitializeDialogue()
        {
            //Some stuff before the actual typing of the dialogue (because you need to set active the game object, etc.)
            gameObject.SetActive(true);
            _textHolder = GetComponent<TMP_Text>();
            StartCoroutine(_startNewDialogue(_typeSound));
        }

        //The actual typing of the dialogue and transitioning from one line to the next
        private IEnumerator _startNewDialogue(AudioSource typeSound)
        {
            int i = 0;
            foreach (string line in DialogueList)
            {
                //Go through each line in DialogueList and type them out
                yield return StartCoroutine(WriteText(line, _textHolder, _delay, typeSound));
                i++;
            }

            //Do other auxillary stuff (like disappearing text or enabling some events) when dialogue ends
            if (_beInactiveAfterClick) gameObject.SetActive(false);
            if (eventsToEnable != null) eventsToEnable.Invoke();
        }

    }
}
