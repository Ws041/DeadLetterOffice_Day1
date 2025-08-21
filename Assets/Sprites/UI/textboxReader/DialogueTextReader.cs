using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace DialogueSystem
{
    //This TextReader is for coworker dialogue
    //This is seperate from the script TextReader, which is for the text at the beginning of the game (where it sets up the world and conflict)

    //DialogueTextReader has some special stuff like creating textboxes for NPC dialogue,
    //so I made two seperate scripts instead since they're so different and it can get messy.
    public class DialogueTextReader : BaseDialogue
    {

        [SerializeField] private byte _red = 195, _green = 190, _blue = 150; //RGB values for color of the dialogue text

        [SerializeField] UnityEvent eventsToEnable;
        private TMP_Text _myText;
        private float _ogDialogueWidth, _ogDialogueXPos;
        private RectTransform _textbox;
        [Multiline] public string[] DialogueList;
        [SerializeField] private float _paddingWidth = 100f, _paddingHeight = 25f; //For textbox padding
        [SerializeField] private float _delay = 0.03f;
        private float _maxWidth;
        private AudioSourcePool _audioSourcePool;

        private void Start()
        {
            StartCoroutine(_startNewDialogue());
        }

        private void Awake()
        {
            _audioSourcePool = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<AudioSourcePool>();
            _myText = GetComponent<TMP_Text>();
            _textbox = GameObject.FindGameObjectWithTag("textbox")?.GetComponent<RectTransform>();
            _maxWidth = GetComponent<RectTransform>().rect.width;

            //Get size and position of textbox already set prior to running the game
            _ogDialogueWidth = _textbox.sizeDelta.x;
            _ogDialogueXPos = _textbox.transform.position.x;
        }

        private void _setUpTextboxComponents(RectTransform textbox, TMP_Text text, string line)
        {
            //
            _changeTextColorAlpha(0);
            text.text = line;
            text.ForceMeshUpdate(); //Textbox will fit snugly with textbox (no padding)

            //Get the width and height of text (not textbox)
            //Because a textbox shouldn't go off screen but go down one line
            float textWidth = text.GetPreferredValues().x >= _maxWidth ? _maxWidth : _myText.GetPreferredValues().x;
            float textHeight = text.GetPreferredValues().y;
            //Add padding
            textbox.sizeDelta = new(textWidth + _paddingWidth, textHeight + _paddingHeight);
            //Move textbox to position of the text
            textbox.transform.position = transform.position;
            text.text = "";

            /*Calculate the position of the textbox
            Depending on the length of the text (and the text is right-aligned), the textbox's right will stay the same
            But the textbox's left will extend or withdrawl to match the text length
            The _og variables are constants (already set during Awake() when game starts)
            The _textbox.sizeDelta.x changes with every new line of dialogue

            Find the distance between the half new width and half the old width
            That value is how much the textbox must shift to maintain illusion that the textbox's right anchor remains in one place
            A positive (_textbox.sizeDelta.x / 2 - _ogDialogueWidth / 2) value means box moves left, 
            while a negative value means box move right (since two negs make a pos) */
            textbox.position = new Vector2(_ogDialogueXPos - (_textbox.sizeDelta.x / 2 - _ogDialogueWidth / 2), 0f);

        }

        //Prepare for textbox before each new line
        //Such as make textbox appear, change size of textbox, make text visible, etc. etc.
        private IEnumerator _preDialogue(string line)
        {
            _textbox.gameObject.SetActive(true);
            _setUpTextboxComponents(_textbox.GetComponent<RectTransform>(), _myText, line);
            yield return new WaitForSeconds(1f);
            _changeTextColorAlpha(255);
        }

        //TPrepare the next line in a dialogue and then type it out
        private IEnumerator _startNewDialogue()
        {
            foreach (string line in DialogueList) {
                yield return _preDialogue(line);
                yield return WriteText(line, _myText, _delay, _audioSourcePool.SFX_Coworker);
                yield return _moveToNextText();
            }
            if (eventsToEnable != null) eventsToEnable.Invoke();
            transform.parent.gameObject.SetActive(false); //Textbox disappears once dialogue is finished
            
        }

        private void _changeTextColorAlpha(byte alpha)
        {
            _myText.color = new Color32(_red, _green, _blue, alpha);
        }

        /*Textbox will travel downwards, fading out of the screen ("parentboxExit" animation)
        Then textbox will move back to the top (above screen), then go down again ("parentboxDefault" animation)
        Illusion of a new textbox going down and out everytime
        Reuse and recycle them dialogue boxes!*/
        private IEnumerator _moveToNextText() {

            Animator parentAnim = transform.parent.GetComponent<Animator>();
            parentAnim.Play("parentboxExit");
            yield return new WaitForSeconds(1f);
            _changeTextColorAlpha(0);
            parentAnim.Play("parentboxDefault");
            yield return null;
        
        }


    }
}