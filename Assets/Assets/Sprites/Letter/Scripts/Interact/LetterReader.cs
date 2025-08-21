using System.Collections;
using UnityEngine;
using TMPro;
using DialogueSystem;
using mailGenerator;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

namespace interactObjects
{
    // Handles letter interaction, content generation, and review sheet spawning.
    // Inherits from PaperInteractTabAnimation (base class for when you tab something and it does a boink animation).
    public class LetterReader : PaperInteractTabAnimation
    {
        // --- Inspector Fields ---
        [Header("UI References")]
        public GameObject LetterTextbox;       // Prefab for the letter text display

        [Header("Data References")]
        public MainDataset MainDataset;       // Central dataset for mail content
        private MailProperties _mailProperties;
        private RepublikaData _republikaData;
        private GreschnovaData _greschnovaData;
        private KastavyeData _kastavyeData;
        private GameObject _guidanceText;

        // --- Protected Fields ---
        protected LetterContentsData _letterContentsData;          // Holds narrative content (names, dialogues)
        protected string _letterContentDialogueList; //Dialogue list (get from mainDataset

        // --- Public Fields ---
        public bool IsLetterBeingRead = false;             // Tracks if letter is currently being read

        // --- Static Fields ---
        [HideInInspector] public string SenderName;   // Generated sender name (e.g., "To John Doe,")
        [HideInInspector] public string ReceiverName; // Generated receiver name

        private ScoreTracker _scoreTracker;
        private AudioSourcePool _audioSourcePool;

        // --- Unity Methods ---

        private void Awake()
        {
            //Get components
            _mailProperties = GameObject.FindGameObjectWithTag("Mail").GetComponent<MailProperties>();
            _republikaData = MainDataset.RepublikaData;
            _greschnovaData = MainDataset.GreschnovaData;
            _kastavyeData = MainDataset.KastavyeData;
            _guidanceText = GameObject.FindGameObjectWithTag("GuidanceText");
            _scoreTracker = GameObject.FindGameObjectWithTag("ScoreTracker").GetComponent<ScoreTracker>();
            _audioSourcePool = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<AudioSourcePool>();
        }

        private void Start()
        {
            // Initialize references
            _letterContentsData = MainDataset.LetterContentsData;

            SenderName = _mailProperties.Local_senderName;
            ReceiverName = _mailProperties.Local_receiverName;

            // Generate and assign names
            //_checkSpecialNames();
            _setNamesOnLetterClone(gameObject, $"To {ReceiverName}", 0);    // Set reciever name on letter UI
            _setNamesOnLetterClone(gameObject, $"<i>Sincerely,</i>\r\n{SenderName}", 2);

            if (_isSpecialContents()) return;
            _generateRandomContents();
            _setNamesOnLetterClone(gameObject, _letterContentDialogueList, 1);


            bool _isSpecialContents()
            {
                if (_scoreTracker.DayNum == 1 && _scoreTracker.MailCounter == _scoreTracker.MailGoal)
                {
                    _letterContentDialogueList = _greschnovaData.Day1_SpecialGreschnovaLetter;
                    _setNamesOnLetterClone(gameObject, _letterContentDialogueList, 1);
                    return true;

                }
                return false;
            }

            
        }

        private void _generateRandomContents()
        {
            string receiverProvinceName = _mailProperties.Local_receiverProvinceName;
            var dictionaryReference = _republikaData.LetterContentsDialogueDictionary_Day1;
            int letterContentListIndex;

            _checkForDiscardMatches(dictionaryReference, ref receiverProvinceName);

            // Get random dialogue
            var provinceDialogues = dictionaryReference[receiverProvinceName];
            letterContentListIndex = Random.Range(0, provinceDialogues.Count);

            if (provinceDialogues.Count > 0) // Always check this first
            {
                letterContentListIndex = Random.Range(0, provinceDialogues.Count);
                // Now it's guaranteed to be safe to use this index
                _letterContentDialogueList = provinceDialogues[letterContentListIndex];
                // ... use the letter ...
            }
            else
            {
                Debug.LogError("No letters available for this province!");
                // Handle the error case
            }
            // Handle null case
            if (_letterContentDialogueList == null)
            {
                _changeProvinceDialogueWhenError(dictionaryReference, ref receiverProvinceName, ref letterContentListIndex);
            }

            // Remove used dialogue
            provinceDialogues.RemoveAt(letterContentListIndex);
        }
        private void _changeProvinceDialogueWhenError(Dictionary<string, List<string>> dictionaryReference,
            ref string receiverProvinceName, ref int letterContentListIndex)
            {
            while (_letterContentDialogueList == null) {
                Debug.Log("letterContents 1");
                _mailProperties.Local_receiverProvinceName = _republikaData.RepublikaRandomProvince();
                Debug.Log("letterContents 2");
                receiverProvinceName = _mailProperties.Local_receiverProvinceName;
                Debug.Log("letterContents 3");
                var provinceDialogues = dictionaryReference[receiverProvinceName];
                Debug.Log("letterContents 4");
                letterContentListIndex = Random.Range(0, provinceDialogues.Count);
                Debug.Log("letterContents 5");
                _letterContentDialogueList = provinceDialogues[letterContentListIndex];
            }
        }

        private void _checkForDiscardMatches(Dictionary<string, List<string>> dictionaryReference,
            ref string receiverProvinceName)
        {
            // Handle sender discard case
            if (_mailProperties.Local_senderProvinceName == "Discard")
            {
                _mailProperties.Local_receiverProvinceName = "Discard";
                receiverProvinceName = "Discard";
                return;
            }

            // Handle receiver discard case when sender isn't a discard
            if (_mailProperties.Local_receiverProvinceName == "Discard")
            {
                while (_mailProperties.Local_receiverProvinceName == "Discard") {
                    _mailProperties.Local_receiverProvinceName = _republikaData.RepublikaRandomProvince();
                }
                receiverProvinceName = _mailProperties.Local_receiverProvinceName;
                return;
            }
        }
        // --- Core Functionality ---

        //NO LONGER AVAILABLE IN PLAYTESTING
        //I've since switched how the player can interact with the letter
        //Instead of pressing tab while the mouse is over it and generating a black textbox that tells you what the letter contain,
        //I've put the word contents on the letter itself, removing need for [Tab] and black textbox. 

        //Handles TAB key interaction to open/read letters
        /*private void OnMouseOver()
        {
            OnTab(); // Handle tab key interaction
        }

        private void OnTab()
        {
            if (IsLetterBeingRead || _isPrototypeLetter) return;

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _audioSourcePool.SFX_PaperFold.Play();
                _instantiateLetterbox();                   // Create letter text display
                StartCoroutine(_letterInteracted());      // Trigger interaction effects
                IsLetterBeingRead = true;
                if (_guidanceText != null) _guidanceText.SetActive(false);

                return;
            }
        }*/


        // Updates name text on a letter UI clone
        private void _setNamesOnLetterClone(GameObject letterClone, string name, int childNum)
        {
            letterClone.transform.GetChild(childNum).GetComponent<TMP_Text>().SetText(name);
        }

        // Instantiates and configures the letter textbox
        /*private void _instantiateLetterbox()
        {
            Vector3 _textbox_pos = new Vector3(0, -4.4f, 0f);
            GameObject _letterTextboxClone = Instantiate(LetterTextbox, _textbox_pos, Quaternion.identity);
            _letterTextboxClone.name = "Letterbox";

            GameObject _letterText = _letterTextboxClone.transform.GetChild(0).gameObject;
            _startLetterboxReader(_letterText);
        }

        /// Initializes dialogue content in the letter textbox
        private void _startLetterboxReader(GameObject letterText)
        {
            // Configure dialogue reader component
            TextReader reader = letterText.GetComponent<TextReader>();

            if (_checkSpecialDialogue()) return;


            bool _checkSpecialDialogue()
            {
                if (_scoreTracker.DayNum == 1 && _scoreTracker.MailCounter == _scoreTracker.MailGoal) {
                    reader.DialogueList = _greschnovaData.Day1_SpecialGreschnovaLetter;
                    reader.initializeDialogue();
                    return true;
                }
                return false;
            }

            //reader.DialogueList = _letterContentDialogueList;
            reader.initializeDialogue();
            
        }*/

    }
}