using interactObjects;
using mailGenerator;
using TMPro;
using UnityEngine;

/// <summary>
/// Handles interactive dropdown text elements that cycle through options on click.
/// Used for nation/province/name selection UI.
/// </summary>
public class FillReviewSheet : MonoBehaviour // Changed to PascalCase naming convention
{
    // --- Component References ---
    private TMP_Text _textComponent;
    //private LetterReader _letterReader;
    private AudioSourcePool _audioSourcePool;
    private MailProperties _mailProperties;

    // --- State Variables ---
    private bool _mouseOver = false;
    private int _currentIndex = 0;
    private string[] _currentOptions;


    // --- Configuration Flags ---
    [SerializeField] private bool _isName = false;
    [SerializeField] private bool _isNation = false;
    [SerializeField] private bool _isProvince = false;
    //[SerializeField] private bool _isDistrict = false;

    // --- Data Collections ---
    private static string[] _nationList = new string[]
    {
        "Republika",
        "Greschnova",
        "Kastavye",
        "[Unidentified]"
    };

    private static string[][] _provinceList = new string[][]
    {
        new string[] { "Reziva", "Priena", "Volknau", "Daravoy" },  // Republika provinces
        new string[] { "Borcova", "St. Siesta", "Bokosnoy", "Parakivka" },  // Greschnova provinces
        new string[] { "Bel Antienne", "Vayenne" },  // Kastavye provinces
        new string[] { "[Unidentified]" }  // Default
    };

    private string[] _nameList = new string[] { "[Unidentified]" };

    // --- Unity Lifecycle Methods ---

    private void Start()
    {
        _textComponent = GetComponent<TMP_Text>();
        _textComponent.color = new Color32(84, 74, 15, 225);

        // Initialize options based on dropdown type
        if (_isNation) _currentOptions = _nationList;
        if (_isProvince) _currentOptions = _provinceList[3]; // Default to unidentified
        //if (_isName) {
        //    _nameList = new string[] { _letterReader.SenderName, _letterReader.ReceiverName, "[Unidentified]" };
        //    _currentOptions = _nameList; }
        if (_isName)
        {
               _nameList = new string[] { _mailProperties.Local_senderName, _mailProperties.Local_receiverName, "[Unidentified]" };
               _currentOptions = _nameList; }
        }

    private void Awake()
    {
        //_letterReader = GameObject.Find("Letter").GetComponent<LetterReader>();
        _mailProperties = GameObject.Find("Mail").GetComponent<MailProperties>();
        _audioSourcePool = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<AudioSourcePool>();
    }

    private void Update()
    {
        _handleMouseClick();
    }

    // --- Mouse Interaction ---

    private void OnMouseOver()
    {
        _textComponent.color = new Color32(124, 109, 15, 225);
        _mouseOver = true;
    }

    private void OnMouseExit()
    {
        _mouseOver = false;
        _textComponent.color = new Color32(84, 74, 15, 225);
    }

    // --- Core Functionality ---

    /// <summary>
    /// Handles left-click interaction to cycle through dropdown options
    /// </summary>
    private void _handleMouseClick()
    {
        if (!_mouseOver) return;

        if (Input.GetMouseButtonDown(0))
        {

            // Update displayed text
            _textComponent.text = _currentOptions[_currentIndex];
            _audioSourcePool.SFX_PaperFold.Play();

            // Special handling for nation dropdown
            //Province resets back to [Unidenfitied] when nation name changes
            if (_isNation)
            {
                _updateLinkedProvinceDropdown();
            }

            // Cycle to next option
            _currentIndex = (_currentIndex + 1 >= _currentOptions.Length) ? 0 : _currentIndex + 1;
        }
    }

    /// <summary>
    /// Updates the linked province dropdown when nation changes
    /// </summary>
    private void _updateLinkedProvinceDropdown()
    {
        Transform _provinceDropdown = transform.parent.Find("Province").transform;
        FillReviewSheet _provinceScript = _provinceDropdown.GetComponent<FillReviewSheet>();

        _provinceScript._currentOptions = _provinceList[_currentIndex];
        _provinceScript._currentIndex = 0;
        _provinceDropdown.GetComponent<TMP_Text>().text = "[Unidentified]";
    }
}