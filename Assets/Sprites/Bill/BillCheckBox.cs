using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using TMPro;
using System;

//This script enables player to check and uncheck things on the Bill gameobject
//Currently, this feature is unvailable for the DEMO, but is in playtesting
//To test it, find the "Bill" folder, then drag and drop the "Bill" prefab into the scene. 
public class BillCheckBox : MonoBehaviour
{
    // Appearances of a checkbox
    [SerializeField] private Sprite[] _availableCheckStates; //Available check states (checked or unchecked)
    [SerializeField] private Sprite[] _noCheckHoverStates; //Hovered or unhovered, unchecked
    [SerializeField] private Sprite[] _yesCheckHoverStates; //Hovered or unhovered, checked

    //Checkboxes influence 
    [SerializeField] private BillCheckBox _otherCheckBox;
    private TMP_Text _parentObject, _moneyObject;

    //Player can choose whether to pay for food and heat. Then that affects the total sum
    [SerializeField] private TMP_Text _billFood, _billHeat, _billTotal;
    private int _maxBill; //A temporary variable at the moment for debugging purposes
    //Sets a fixed Day 1 salary without the playtester going through the entire Day 1

    private List<GameObject> _objectsChangeColor;

    //Changing color for text next to the bill
    // A ticked checkbox turns text green. An unticked checkbox turns text yellowish
    private Color32 _tickedColor = new (40, 53, 28, 255);
    private Color32 _untickedColor = new(95, 73, 5, 255);
    private int _checkNum = 0; //Index
    private SpriteRenderer _checkState; //Default state (set to checked)

    [NonSerialized] public int TempTotalBill; //Will be changed by other BillCheckBox scripts on other gameobjects

    //Assign values
    private void Awake()
    {
        _checkState = GetComponent<SpriteRenderer>();
        _parentObject = transform.parent.gameObject.GetComponent<TMP_Text>();
        _moneyObject = transform.parent.Find("Money").gameObject.GetComponent<TMP_Text>();
    }

    private void Start()
    {
        _updateCheckState(0, _availableCheckStates); //Set default checkbox to checked (_availableCheckStates[0] is image of a checked box)
        _maxBill = 60 - 10 * 2 - 15 * 2;
        TempTotalBill = _maxBill; 
        //Add a negative sign in front of the abs of sum total or not, since -$5 is less awkward than $-5
        if (_maxBill >= 0) _billTotal.text = $"${_maxBill}";
        else _billTotal.text = $"-${Mathf.Abs(_maxBill)}";
    }

    private void OnMouseDown()
    {
        _checkNum = 1 + _checkNum * -1; //Changing index so that it can switch between 0 and 1 depending on the previous state
        _updateCheckState(_checkNum, _availableCheckStates);
        _updateTotalMoney(); //

        if (_checkNum == 0)
        {
            _parentObject.color = _tickedColor;
            _moneyObject.color = _tickedColor;
            return;
        }
        _parentObject.color = _untickedColor;
        _moneyObject.color = _untickedColor;
        
    }

    private void _updateTotalMoney()
    {
        int _ticked = _checkNum == 0 ? -1 : 1; //_ticked is either 1 or -1, so the sum either loses money or restores back
        if (_parentObject.name == "Food")
        {
            TempTotalBill += 15 * _ticked;
        }
        if (_parentObject.name == "Heat")
        {
            TempTotalBill += 10 * _ticked;
        }
        _otherCheckBox.TempTotalBill = TempTotalBill; //There's only two checkboxes, otherwise I'd call for a foreach
        //Basically updates the TempTotalBill of both BillCheckBox scripts. 
        _billTotal.text = $"${TempTotalBill}";
    }


    
    //Hover mouse and checkbox becomes lighter colored
    private void OnMouseOver()
    {
        if(_checkNum == 1)
        {
            _updateCheckState(1, _noCheckHoverStates);
            return;
        }
        _updateCheckState(1, _yesCheckHoverStates);
    }

    //Exit mouse and checkbox becomes dark
    //Interactivity! Love visual cues
    private void OnMouseExit()
    {
        if (_checkNum == 1)
        {
            _updateCheckState(0, _noCheckHoverStates);
            return;
        }
        _updateCheckState(0, _yesCheckHoverStates);
    }

    private void _updateCheckState(int num, Sprite[] spriteList)
    {
        _checkState.sprite = spriteList[num];
    }
}
