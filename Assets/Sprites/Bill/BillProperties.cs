using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace interactObjects
{
    //This script allows player to interact with the Bill object (player gets fees they have to pay at the end of each day)
    //Player can tick off things they want to pay (such as rent, food, heat, etc.) 
    //To conclude, player press [Tab] to sign the Bill. 

    //Currently, this feature is not yet available to play. It's remains in playtesting mode.
    //If you want to test this feature, find the "Bill" folder, then drag and drop the "Bill" prefab into the scene
    public class BillProperties : PaperInteractTabAnimation
    {
        private bool _hasSigned = false;
        [SerializeField] private Sprite[] _signedStates;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        //Set the default appearance of Bill to that of unsigned 
        private void Start()
        {
            _spriteRenderer.sprite = _signedStates[0];
        }

        //Only check when mouse is over to avoid calling Update() method
        private void OnMouseOver()
        {
            _onTab();
        }

        //When player press [Tab], the bill will do a little bounce animation (_letterInteracted()),
        //then switch SpriteRenderer to that of a signed bill
        private void _onTab()
        {
            if (!Input.GetKeyDown(KeyCode.Tab)) return; //If no [Tab], return to save on redundancy
            if (_hasSigned) return; //Same goes to the Bill being signed
            StartCoroutine(_letterInteracted());
            _spriteRenderer.sprite = _signedStates[1];
            _hasSigned = true;

        }
    }
}