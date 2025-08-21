using UnityEngine;
using mailGenerator;
using System.Runtime.CompilerServices;
using System.Collections;

namespace interactObjects
{
    // Handles mail interaction behavior, including letter instantiation on key press.
    // Inherits from papersInteract for shared paper-like object functionality.
    public class MailOpener : PaperInteractTabAnimation
    {
        // --- Inspector-Assigned References ---
        [SerializeField] private MainDataset _mainDataset;  // Central data repository for mail properties
        private RepublikaData _republikaData;
        private GreschnovaData _greschnovaData;
        private KastavyeData _kastavyeData;
        private ScoreTracker _scoreTracker;
        private GameObject _letterPrefab;
        [SerializeField] private bool _day1GreschnovaMail;
        private AudioSourcePool _audioSourcePool;


        // --- State Variables ---
        private bool _isMailOpened = false;  // Tracks if mail has been opened
        private bool _isMouseOver = false;   // Tracks mouse hover state

        private void Awake()
        {
            _audioSourcePool = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<AudioSourcePool>();
        }

        private void Start()
        {
            _republikaData = _mainDataset.RepublikaData;
            _greschnovaData = _mainDataset.GreschnovaData;
            _kastavyeData = _mainDataset.KastavyeData;
            _scoreTracker = GameObject.FindGameObjectWithTag("ScoreTracker").GetComponent<ScoreTracker>();
        }

        // --- Unity Lifecycle Methods ---

        private void Update()
        {
            OnTab();  // Check for shift key interaction each frame
        }

        // --- Mouse Interaction ---

        /// Sets mouseOver flag when mouse enters the object's collider
        private void OnMouseEnter() => _isMouseOver = true;

        /// Clears mouseOver flag when mouse exits the object's collider
        private void OnMouseExit() => _isMouseOver = false;

        // --- Core Functionality ---

        // Handles TAB key interaction to open mail
        private void OnTab()
        {
            // Only proceed if TAB is pressed, mail isn't already opened, and mouse is over object
            if (!Input.GetKeyDown(KeyCode.Tab)) return;

            if (_isMailOpened || !_isMouseOver) return;
            _audioSourcePool.SFX_PaperFold.Play();
            _isMailOpened = true;
            _instantiateLetter();          // Create letter instance
            StartCoroutine(_letterInteracted());  // Trigger interaction effects
            if (_scoreTracker.DayNum == 1 && _scoreTracker.MailCounter == 1)
            {
                StartCoroutine(_playT_D1SpawnReviewSheet());
            }
        }

        private IEnumerator _playT_D1SpawnReviewSheet()
        {
            yield return new WaitForSeconds(6f);
            _scoreTracker.playTimeline("T_D1SpawnReviewSheet");
        }

        // Instantiates a letter prefab and configures its basic properties
        private void _instantiateLetter()
        {
            string nationName = transform.GetComponent<MailProperties>().Local_senderNationName;

            /*if (_isPrototypeLetter)
            {
                _letterPrefab = _prototypeLetter;
                _setupLetterClone(_letterPrefab);
                //_letterPrefab.transform.SetParent(transform.parent);
                //_setLetterCloneProperties(_letterPrefab);
                return;
            }*/

            if (_checkSpecialMail()) return;

            bool _checkSpecialMail()
            {
                //Check if mail is special, so can instantiate special letter
                if (_scoreTracker.DayNum == 1 && _scoreTracker.MailCounter == _scoreTracker.MailGoal)
                {
                    _letterPrefab = _greschnovaData.GreschnovaLetters_torn[0];
                    _setupLetterClone(_letterPrefab);
                    return true;
                }
                return false;
            }

            //------ if _checkSpecialMail() is false
            _letterPrefab = nationName switch
            {
                //_letterPrefab is gameObject letter, gets random letter from list below
                "Republika" => _republikaData.RepublikaLetterRandom(),
                "Greschnova" => _greschnovaData.GreschnovaLetterRandom(),
                "Kastavye" => _kastavyeData.KastavyeLetterRandom(),
                _ => null
            };
            _setupLetterClone(_letterPrefab);

        }
        private void _setupLetterClone(GameObject letterPrefab)
        {
            GameObject letterClone = Instantiate(letterPrefab, transform.position, Quaternion.identity);
            _setLetterCloneProperties(letterClone);  // Configure scale and hierarchy
        }


        // Configures the scale, parent, and z-order of the letter clone
        private void _setLetterCloneProperties(GameObject letterClone)
        {
            letterClone.name = "Letter";// Set clear identifier

            letterClone.transform.position = new Vector3(letterClone.transform.position.x, letterClone.transform.position.y, -20);

            // Apply universal scale from dataset
            float scale = _mainDataset.universalSize;
            letterClone.transform.localScale = new Vector3(scale, scale, scale);

            // Set parent to maintain hierarchy organization
            letterClone.transform.SetParent(transform.parent);

            // Bring to front in rendering order
            letterClone.transform.SetAsLastSibling();
        }
    }
}