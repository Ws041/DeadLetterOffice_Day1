using mailGenerator;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterFlip : MonoBehaviour
{
    [SerializeField] private bool _isRightFlip;
    [SerializeField] private GameObject _otherPageFlip;
    [SerializeField] private GameObject _leftFlip, _rightFlip;
    [SerializeField] private MainDataset _mainDataset;
    [SerializeField] private float _indentXChangeWhenLetterFlipped = 0.15f;

    private RepublikaData _republikaData;
    private GreschnovaData _greschnovaData;
    private KastavyeData _kastavyeData;


    private GameObject _receiverRegards, _sender;
    private TMP_Text _letterContents;
    private int _pageIndex = 0;
    private float _ogYPos, _ogXPos, _ogReceiverXPos, _ogSizeX, _ogSizeY;

    private AudioSourcePool _audioSourcePool;

    public int PageIndex {
        get => _pageIndex;
        set 
        {
            if(value >= (int)LetterPages.Page2)
            {
                _pageIndex = (int)LetterPages.Page2;
                return;
            }
            if (value <= 0)
            {
                _pageIndex = 0;
                return;
            }
        }
     }

    private enum LetterPages
    {
        Page1,
        Page2
    };

    private void Awake()
    {
        _letterContents = transform.parent.Find("Contents").GetComponent<TMP_Text>();
        _sender = transform.parent.Find("Sender").gameObject;
        _otherPageFlip.GetComponent<LetterFlip>().PageIndex = PageIndex;
        _receiverRegards = transform.parent.Find("Receiver").gameObject;
        _audioSourcePool = GameObject.FindGameObjectWithTag("AudioPool").GetComponent<AudioSourcePool>();

        _republikaData = _mainDataset.RepublikaData;
        _greschnovaData = _mainDataset.GreschnovaData;
        _kastavyeData = _mainDataset.KastavyeData;


        _ogYPos = _letterContents.transform.localPosition.y;
        _ogXPos = _letterContents.transform.localPosition.x;
        _ogReceiverXPos = _receiverRegards.transform.localPosition.x;

        _ogSizeX = _letterContents.rectTransform.sizeDelta.x;
        _ogSizeY = _letterContents.rectTransform.sizeDelta.y;
    }

    private void Start()
    {
        _letterContents.ForceMeshUpdate();
        //_tempSetLetterContents();

        if (_letterContents.textInfo.pageCount <= 1)
        {
            
            _adjustNoOverflowDesign(1);
            _receiverRegards.SetActive(true);
            _sender.SetActive(true);
            _leftFlip.SetActive(false);
            _rightFlip.SetActive(false);
            return;
        }

        
        _letterContents.rectTransform.sizeDelta = new Vector2(_ogSizeX, _ogSizeY + 0.09f);
        _receiverRegards.SetActive(false);
        UpdateFlipPages();

    }

    private void _adjustNoOverflowDesign(float pageCount)
    {
        _letterContents.ForceMeshUpdate();

        float padding = 0.12f;
        float letterHeight = _letterContents.preferredHeight;
        float _newReceiverXPos = _ogReceiverXPos;

        if (pageCount == 2)
        {

            // Get the bottom position of the last character
            TMP_CharacterInfo lastChar = _letterContents.textInfo.characterInfo[_letterContents.textInfo.pageInfo[1].lastCharacterIndex];
            float pageEndY = lastChar.bottomLeft.y; 
            //TMPro calculates text (bottomLeft) counts as below the baseline. Therefore it's a negative value
            //To adjust position of _receiverRegards, turn pageEndY into absolute values to get distance from baseline to the lastCharacter.

            letterHeight = Mathf.Abs(pageEndY);
            _newReceiverXPos = _ogReceiverXPos - _indentXChangeWhenLetterFlipped;
        }
        _receiverRegards.transform.localPosition = new Vector2(_newReceiverXPos, _letterContents.transform.localPosition.y - letterHeight - padding);
        
    }

    private void OnMouseDown()
    {
        _audioSourcePool.SFX_PaperFlip.Play();
        _checkRightClickHandler();
        UpdateFlipPages();
        _updateLetterContent();

       
    }

    private void _checkRightClickHandler()
    {
        if (_isRightFlip)
        {
            PageIndex++;
            return;
        }
        PageIndex--;
        
    }

    public void UpdateFlipPages()
    {
        bool isFirstPage = PageIndex == (int)LetterPages.Page1;
        bool isLastPage = PageIndex == (int) LetterPages.Page2;

        _otherPageFlip.GetComponent<LetterFlip>().PageIndex = PageIndex;
        _rightFlip.SetActive(isFirstPage);
        _leftFlip.SetActive(isLastPage);
        _receiverRegards.SetActive(isLastPage);
        _sender.SetActive(isFirstPage);

        transform.parent.GetComponent<SpriteRenderer>().flipX = isLastPage;

        if (_letterContents.textInfo.pageCount <= 1) return;
        if (isLastPage) {
            _letterContents.transform.localPosition = new(_ogXPos - _indentXChangeWhenLetterFlipped, _ogYPos + 0.09f);
            _letterContents.rectTransform.sizeDelta = new Vector2(_ogSizeX, _ogSizeY + 0.09f);
            _adjustNoOverflowDesign(2);
            return;
        }
        _letterContents.transform.localPosition = new(_ogXPos, _ogYPos);
        _letterContents.rectTransform.sizeDelta = new Vector2(_ogSizeX, _ogSizeY + 0.09f);
        _adjustNoOverflowDesign(1);

    }

    private void _updateLetterContent()
    {
        _letterContents.pageToDisplay = PageIndex + 1;
    }

}
