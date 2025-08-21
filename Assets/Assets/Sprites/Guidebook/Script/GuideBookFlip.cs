using UnityEngine;
using TMPro;

/// <summary>
/// Handles page flipping functionality for a guidebook UI.
/// Manages content display for different pages including nations info, stamps, and maps.
/// </summary>
public class GuideBookFlip : MonoBehaviour
{
    /// <summary>
    /// Enum representing all possible guidebook pages
    /// </summary>
    private enum GuideBookPage
    {
        FrontPage,          // 0
        TableOfContents,    // 1
        Rules,              // 2
        WorldMap,           // 3
        RepublikaInfo,      // 4
        GreschnovaInfo,     // 5
        KastavyeInfo,       // 6
        LastPage            // 7
    }

    [Header("Page Content References")]
    [SerializeField] private Sprite[] _stampPictures;        // 0: Republika, 1: Greschnova, 2: Kastavye
    [SerializeField] private Sprite[] _guidebookPictures;   // 0: Table of contents, 1: Rules (Default), 2: Worldmap

    [Header("Navigation Controls")]
    [SerializeField] private bool _isRightClickHandler = false;
    private GameObject _leftPageFlip;
    private GameObject _rightPageFlip;

    [Header("Page Elements")]
    [SerializeField] private GameObject _stampTitle;
    [SerializeField] private GameObject _provinceTitle;
    [SerializeField] private GameObject _mapPageObjects;
    [SerializeField] private GameObject _contentsObjects;
    [SerializeField] private GameObject _lastPage;
    [SerializeField] private GameObject _frontPage;
    [SerializeField] private GameObject _rulesPage;

    [Header("Cross-script Reference")]
    public GuideBookFlip GuideBookFlip_Script;

    // Cached UI components
    private TMP_Text _nationText;
    private TMP_Text _provinceText;
    private SpriteRenderer _stampRenderer;
    private SpriteRenderer _guidebookRenderer;

    // Page tracking
    [HideInInspector] public int CurrentPageIndex = 0;
    private const int _firstNationPage = (int)GuideBookPage.RepublikaInfo; // Republika starts at index 4
    private const int _lastNationPage = (int)GuideBookPage.KastavyeInfo;   // Kastavye ends at index 6

    private AudioSourcePool _audioSourcePool;
    private ScoreTracker _scoreTracker;
    private PauseScreen _pauseScreen;

    private void Awake()
    {
        _cacheReferences();
        UpdateGuidebookPage(); //Public to be accessed in FlipToPageGuideBook
    }

    /// <summary>
    /// Caches all necessary component references
    /// </summary>
    private void _cacheReferences()
    {
        Transform parent = transform.parent;
        _nationText = parent.Find("Nation").GetComponent<TMP_Text>();
        _provinceText = parent.Find("Province").GetChild(0).GetComponent<TMP_Text>();
        _stampRenderer = parent.Find("StampPictures").GetComponent<SpriteRenderer>();
        _guidebookRenderer = parent.GetComponent<SpriteRenderer>();
        _mapPageObjects = parent.Find("MapNationLabels").gameObject;

        _leftPageFlip = parent.Find("BookFlipLeft")?.gameObject;
        _rightPageFlip = parent.Find("BookFlipRight")?.gameObject;

        _audioSourcePool = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<AudioSourcePool>();
        _scoreTracker = GameObject.FindGameObjectWithTag("ScoreTracker")?.GetComponent<ScoreTracker>();
        _pauseScreen = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<PauseScreen>();
    }

    /// <summary>
    /// Handles mouse click to flip pages forward or backward
    /// </summary>
    private void OnMouseDown()
    {
        if (!_scoreTracker.IsStartDay || _pauseScreen.IsGamePaused) return;
        if (_isRightClickHandler)
        {
            // Move to next page, clamping at last page
            CurrentPageIndex = CurrentPageIndex + 1 >= (int)GuideBookPage.LastPage
                ? (int)GuideBookPage.LastPage
                : CurrentPageIndex + 1;
        }
        else
        {
            // Move to previous page, clamping at first page
            CurrentPageIndex = CurrentPageIndex - 1 < 0
                ? 0
                : CurrentPageIndex - 1;
        }
        
        transform.parent.transform.SetAsLastSibling();
        _audioSourcePool.SFX_PaperFlip.Play();
        UpdateGuidebookPage();
    }

    /// <summary>
    /// Updates all guidebook UI elements based on current page
    /// </summary>
    [HideInInspector]
    public void UpdateGuidebookPage()
    {
        
        GuideBookFlip_Script.CurrentPageIndex = CurrentPageIndex;

        _updateGuideBookTemplate();
        _updateNationInfo();
        _updateProvinceList();
        _updateStampImage();
        _updateStampProvinceTitles();
        _updateMapPage();
        _updateContentsPage();
        _updateLastPage();
        _updateFrontPage();
        _updateRulesPage();
        _updatePageFlipButtons();
    }

    /// <summary>
    /// Updates the guidebook's base template sprite based on current page
    /// </summary>
    private void _updateGuideBookTemplate()
    {
        _guidebookRenderer.sprite = CurrentPageIndex switch
        {
            0 => _guidebookPictures[0],  // Front Page
            1 => _guidebookPictures[1],  // Table of contents
            3 => _guidebookPictures[3],  // World map
            _ => _guidebookPictures[2]   // Default rules page
        };
    }

    /// <summary>
    /// Updates the front page visibility and adjusts collider
    /// </summary>
    private void _updateFrontPage()
    {
        bool isFrontPage = CurrentPageIndex == (int)GuideBookPage.FrontPage;
        _frontPage.SetActive(isFrontPage);

        var boxCollider = transform.parent.GetComponent<BoxCollider2D>();
        if (isFrontPage)
        {
            boxCollider.size = new Vector2(1.354f, 1.8f);
            boxCollider.offset = new Vector2(0.678f, 0f);
        }
        else
        {
            boxCollider.size = new Vector2(2.714f, 1.8f);
            boxCollider.offset = Vector2.zero;
        }
    }

    /// <summary>
    /// Updates the table of contents page visibility
    /// </summary>
    private void _updateContentsPage()
    {
        bool isContentsPage = CurrentPageIndex == (int)GuideBookPage.TableOfContents;
        _contentsObjects.SetActive(isContentsPage);
    }

    /// <summary>
    /// Updates the last page visibility
    /// </summary>
    private void _updateLastPage()
    {
        bool isLastPage = CurrentPageIndex == (int)GuideBookPage.LastPage;
        _lastPage.SetActive(isLastPage);
    }

    private void _updateRulesPage()
    {
        bool isRulesPage = CurrentPageIndex == (int)GuideBookPage.Rules;
        _rulesPage.SetActive(isRulesPage);
    }

    /// <summary>
    /// Updates the nation name text display
    /// </summary>
    private void _updateNationInfo()
    {
        _nationText.text = CurrentPageIndex switch
        {
            (int)GuideBookPage.RepublikaInfo => "REPUBLIKA",
            (int)GuideBookPage.GreschnovaInfo => "GRESCHNOVA",
            (int)GuideBookPage.KastavyeInfo => "KASTAVYE",
            _ => ""
        };
    }

    /// <summary>
    /// Updates the province list text display
    /// </summary>
    private void _updateProvinceList()
    {
        _provinceText.text = CurrentPageIndex switch
        {
            (int)GuideBookPage.RepublikaInfo => "> Reziva\r\n> Priena\r\n> Volknau\r\n> Daravoy\r\n",
            (int)GuideBookPage.GreschnovaInfo => "> Borcova\r\n> St. Siesta\r\n> Bokosnoy\r\n> Parakivka\r\n",
            (int)GuideBookPage.KastavyeInfo => "> Bel Antienne\r\n> Vayenne\r\n",
            _ => ""
        };
    }

    /// <summary>
    /// Updates the stamp image display for nation pages
    /// </summary>
    private void _updateStampImage()
    {
        if (CurrentPageIndex >= _firstNationPage && CurrentPageIndex <= _lastNationPage)
        {
            _stampRenderer.sprite = _stampPictures[CurrentPageIndex - _firstNationPage];
        }
        else
        {
            _stampRenderer.sprite = null;
        }
    }

    /// <summary>
    /// Updates the visibility of page flip buttons based on current page
    /// </summary>
    private void _updatePageFlipButtons()
    {
        bool isFirstPage = CurrentPageIndex == (int)GuideBookPage.FrontPage;
        bool isLastPage = CurrentPageIndex == (int)GuideBookPage.LastPage;

        _leftPageFlip.SetActive(!isFirstPage);
        _rightPageFlip.SetActive(!isLastPage);
    }

    /// <summary>
    /// Updates the visibility of stamp and province titles
    /// </summary>
    private void _updateStampProvinceTitles()
    {
        bool isNationPages = CurrentPageIndex >= _firstNationPage && CurrentPageIndex <= _lastNationPage;
        _stampTitle.SetActive(isNationPages);
        _provinceTitle.SetActive(isNationPages);
    }

    /// <summary>
    /// Updates the visibility of world map elements
    /// </summary>
    private void _updateMapPage()
    {
        bool isMapPage = CurrentPageIndex == (int)GuideBookPage.WorldMap;
        _mapPageObjects.SetActive(isMapPage);
    }
}