using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TouristbookFlip : MonoBehaviour
{
    private enum TouristBook {
        Frontpage,          //0
        TableOfContents,    //1
        //Republika
        PrienaPage,         //2
        RezivaPage,         //3
        VolknauPage,        //4
        DaravoyPage,        //5

        //Greschnova
        StSiestaPage,       //6
        BorcovaPage,        //7
        BokosnoyPage,       //8
        ParakivkaPage,      //9

        //Kastavye
        VayennePage,         //10
        BelAntiennePage      //11
        
    }

    [Header("Page Content References")]
    [SerializeField] private Sprite[] _touristBookBackground;
    [SerializeField] private Sprite[] _pictureList;

    public TouristbookFlip TouristbookFlip_script;
    [HideInInspector] public int CurrentPageIndex = 0;

    [SerializeField] private GameObject _leftPageFlip, _rightPageFlip;
    [SerializeField] private GameObject _frontPage;
    [SerializeField] private GameObject _tableOfContentsPage;
    [SerializeField] private bool _isRightClickHandler = false;

    [SerializeField] private GameObject _provincePage;

    [SerializeField] private GameObject _provinceTitle, _provinceTitleShadow, _description, _recommendedPlaces, _pictures;

    private int _lastPage = (int)TouristBook.BelAntiennePage;
    private ScoreTracker _scoreTracker;

    private SpriteRenderer _touristRenderer;
    private AudioSourcePool _audioSourcePool;
    private PauseScreen _pauseScreen;
    private void Start()
    {
        _touristRenderer = transform.parent.GetComponent<SpriteRenderer>();
        CurrentPageIndex = 0;
        UpdateTouristBook();
    }

    private void Awake()
    {
        _audioSourcePool = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<AudioSourcePool>();
        _scoreTracker = GameObject.FindGameObjectWithTag("ScoreTracker")?.GetComponent<ScoreTracker>();
        _pauseScreen = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<PauseScreen>();
    }

    private void OnMouseDown()
    {
        if (!_scoreTracker.IsStartDay || _pauseScreen.IsGamePaused) return;
        _audioSourcePool.SFX_PaperFlip.Play();
        _checkRightClickHandler();
        UpdateTouristBook(); //Public to be accessed in FlipToPageTouristBook
    }

    [HideInInspector] public void UpdateTouristBook()
    {
        TouristbookFlip_script.CurrentPageIndex = CurrentPageIndex;
        _updateBackgroundTemplate();
        _updatePageFlipButtons();
        _updateFrontPage();
        _updateTalbeOfContentsPage();
        _updateProvincePage();

    }

    private void _checkRightClickHandler()
    {
        if (_isRightClickHandler)
        {
            // Move to next page, clamping at last page
            CurrentPageIndex = CurrentPageIndex + 1 >= _lastPage
                ? _lastPage
                : CurrentPageIndex += 1;
        }
        else
        {
            // Move to previous page, clamping at first page
            CurrentPageIndex = CurrentPageIndex - 1 < 0
                ? 0
                : CurrentPageIndex -= 1;
        }

        transform.parent.transform.SetAsLastSibling();
    }

    private void _updatePageFlipButtons()
    {
        bool isFirstPage = CurrentPageIndex == (int)TouristBook.Frontpage;
        bool isLastPage = CurrentPageIndex == _lastPage;

        _leftPageFlip.SetActive(!isFirstPage);
        _rightPageFlip.SetActive(!isLastPage);
    }

    private void _updateFrontPage()
    {
        bool _isFrontPage = CurrentPageIndex == (int)TouristBook.Frontpage;
        _frontPage.SetActive(_isFrontPage);
    }

    private void _updateBackgroundTemplate() {
        _touristRenderer.sprite = CurrentPageIndex switch
        {
            0 => _touristBookBackground[0],  // Front Page
            _ => _touristBookBackground[1]   // Default background
        };
    }

    private void _updateTalbeOfContentsPage()
    {
        bool _isContentsPage = CurrentPageIndex == (int)TouristBook.TableOfContents;
        _tableOfContentsPage.SetActive(_isContentsPage);
    }

    private void _updateProvincePage()
    {
        bool _isProvincePage = CurrentPageIndex >= (int)TouristBook.PrienaPage && CurrentPageIndex <= _lastPage;
        _provincePage.SetActive(_isProvincePage);
        if (!_isProvincePage) return;
        _updateProvinceInfo();
    }

    private void _updateProvinceInfo()
    {
        //_provinceTitle, _description, _recommendedPlaces

        _provinceTitle.GetComponent<TMP_Text>().text = CurrentPageIndex switch
        {
            2 => "Priena",
            3 => "Reziva",
            4 => "Volknau",
            5 => "Daravoy",

            6 => "St. Siesta",
            7 => "Borcova",
            8 => "Bokosnoy",
            9 => "Parakivka",

            10 => "Vayenne",
            11 => "Bel Antienne",

            _ => ""
        };


        _provinceTitleShadow.GetComponent<TMP_Text>().text = CurrentPageIndex switch
        {
            2 => "Priena",
            3 => "Reziva",
            4 => "Volknau",
            5 => "Daravoy",

            6 => "St. Siesta",
            7 => "Borcova",
            8 => "Bokosnoy",
            9 => "Parakivka",

            10 => "Vayenne",
            11 => "Bel Antienne",

            _ => ""
        };

        _description.GetComponent<TMP_Text>().text = CurrentPageIndex switch
        {
            2 => "   \"The capital of Republika is known for its long tradition of law and justice, boasting astounding classical arts, architecture, and statues.\"",
            3 => "   \"A sun-kissed paradise with lush orchards and bustling markets of oranges, peaches, and more. Must-visit for food lovers!\"",
            4 => "   \"A sanctuary for bird enthusiasts, birdwatching, hunting expeditions, and  feather crafts. Nature’s beauty takes flight here!\"",
            5 => "   \"Nation's breadbasket where golden wheat fields stretch endlessly. Experience the charm of agrarian life and fresh farm delights.\"",

            6 => "   \"The heart of Greschnova, home to His Majesty and his Royal Guards. Grand palaces, royal pageants, and splendor paint this city.\"",
            7 => "   \"A sailor’s dream, where you can rent boats, explore coastal waters, or simply relax by the docks. The sea breeze and adventure await!\"",
            8 => "   \"A seafood lover’s haven, famous for its marine cuisine. Enjoy fresh catches by the harbour and piers, or dine in cozy seaside taverns.\"",
            9 => "   \"Rolling vineyards and olive groves mark the landscape. Sip on world-class wines and savor vintage  in this serene countryside.\"",

            10 => "   \"The stronghold of military tradition, where discipline and history blend. Witness tank parades, forts, and the legacy of national heroes.\"",
            11 => "   \"A world of towering mountains and deep forests, perfect for hiking, solitude, and reconnecting with the ancient untouched wilderness.\"",

            _ => ""
        };

        _recommendedPlaces.GetComponent<TMP_Text>().text = CurrentPageIndex switch
        {
            2 => "> Courthouse of St. Helga\r\n> The Bridge of Sighs\r\n> Petrified Gardens\r\n> The Stonemasons Guildhall",
            3 => "> Citrus Grove Promenade\r\n> The Honeyed Fig Inn\r\n> Reziva Spice Market",
            4 => "> The Old Eyre\r\n> Lake Mirren Bird Sanctuary\r\n> The Everglades\r\n> Duck Hunt Lake",
            5 => "> Harvest Moon Festival\r\n> The Millwright’s Waterwheel\r\n> The Rustic Plowman",

            6 => "> Scepter Banquet Hall\r\n> Cathedral of St. Siesta\r\n> Royal Procession Boulevard\r\n? The Winter Palace",
            7 => "> Old Kraken’s Reef\r\n> Blue Mast Ship Rentals\r\n? The Mariner’s Lighthouse",
            8 => "> The Shell Market\r\n> Salty Pier\r\n> The Old Seaman’s Dine",
            9 => "> Olive Press Square\r\n> Terrace of a Thousand Vines\r\n> The Drunken Poet Horn\r\n> Where the Mellow Sleep",

            10 => "> Angel Bastion\r\n> The Grande Battery\r\n> House of the Eternal Flame\r\n> The Great Armory",
            11 => "> Hermit’s Trail\r\n> Ranger’s County National Park\r\n> The Crone’s Long Overlook",

            _ => ""
        };

        _pictures.GetComponent<SpriteRenderer>().sprite = CurrentPageIndex switch
        {
            2 => _pictureList[0],
            3 => _pictureList[1],
            4 => _pictureList[2],
            5 => _pictureList[3],
            6 => _pictureList[4],
            7 => _pictureList[5],
            8 => _pictureList[6],
            9 => _pictureList[7],
            10 => _pictureList[8],
            11 => _pictureList[9],
            _ => null
        };
    }
    
}
