using System;
using System.Collections;
using mailGenerator;
using TMPro;
using UnityEngine;

//Checks whether the player has filled out review sheet correctly
//Assigned to Reviewsheet prefab
public class CheckReviewAccuracy : MonoBehaviour
{
    // Cached references
    private MailProperties _mailProperties;
    private ScoreTracker _scoreTracker;
    private TicketSpawner _spawnTicket;

    // Validation state
    private bool _correctInput = false;
    private string _senderViolation, _receiverViolation;

    // UI elements to check
    //Drag and drop Name, Nation, Province of sender and receiver into these arrays
    //Can be found in ReviewSheet prefab
    [SerializeField] private GameObject[] _senderInformation;
    [SerializeField] private GameObject[] _receiverInformation;

    // Initializes required components on awake
    private void Awake()
    {
        _scoreTracker = GameObject.FindWithTag("ScoreTracker").GetComponent<ScoreTracker>();
        _spawnTicket = GameObject.FindWithTag("TicketSpawner").GetComponent<TicketSpawner>();
    }

    // Main method to check all mail accuracy aspects
    [HideInInspector]
    public void checkAccuracy()
    {
        ResetValidationState();

        // Check all validation aspects
        CheckDiscardMail();
        if (_correctInput) // Only check other details if not discarded
        {
            CheckSenderInfoAccuracy();
            CheckReceiverInfoAccuracy();
        }

        UpdateScoreAndSpawnTicket();
    }

    // Resets validation state before new check
    private void ResetValidationState()
    {
        _senderViolation = string.Empty;
        _receiverViolation = string.Empty;
        _correctInput = true; // Assume correct until proven otherwise
    }

    // Updates score and spawns violation ticket if needed
    private void UpdateScoreAndSpawnTicket()
    {
        if (_correctInput)
        {
            _scoreTracker.MailCorrect++;
        }
        else
        {
            _scoreTracker.MailIncorrect++;
            StartCoroutine(_spawnTicket.SpawnTicket(_senderViolation, _receiverViolation)); //Accesses SpawnTicket script
        }
    }


    // Checks if mail should be discarded based on province names
    //This is so that program can skip checking sender and receiver name accuracy because it doesn't matter in the end,
    //The stamp is invalid, after all.
    [HideInInspector]
    public void CheckDiscardMail()
    {
        _mailProperties = GameObject.FindWithTag("Mail").GetComponent<MailProperties>();

        if (_mailProperties.Local_senderProvinceName == "Discard" ||
            _mailProperties.Local_receiverProvinceName == "Discard")
        {
            _correctInput = false; //To skip or not to skip checking receiver/sender names
            _senderViolation = "INVALID STAMP"; 
            _receiverViolation = string.Empty;
        }
    }

    // Validates sender information against mail properties
    public void CheckSenderInfoAccuracy()
    {
        _mailProperties = transform.parent.Find("Mail").GetComponent<MailProperties>();
        if (_senderInformation == null) return;

        // Check each sender field sequentially
        if (!_validateField(_senderInformation[0], _mailProperties.Local_senderName, "INVALID SENDER NAME", ref _senderViolation)) return;
        if (!_validateField(_senderInformation[1], _mailProperties.Local_senderNationName, "INVALID SENDER NATION", ref _senderViolation)) return;
        if (!_validateField(_senderInformation[2], _mailProperties.Local_senderProvinceName, "INVALID SENDER PROVINCE", ref _senderViolation)) return;
    }

    // Validates receiver information against mail properties
    public void CheckReceiverInfoAccuracy()
    {
        _mailProperties = transform.parent.Find("Mail").GetComponent<MailProperties>();
        if (_receiverInformation == null) return;

        // Check each receiver field sequentially
        if (!_validateField(_receiverInformation[0], _mailProperties.Local_receiverName, "INVALID RECEIVER NAME", ref _receiverViolation)) return;
        if (!_validateField(_receiverInformation[1], _mailProperties.Local_receiverNationName, "INVALID RECEIVER NATION", ref _receiverViolation)) return;
        if (!_validateField(_receiverInformation[2], _mailProperties.Local_receiverProvinceName, "INVALID RECEIVER PROVINCE", ref _receiverViolation)) return;
    }


    // Helper method to validate a single field
    private bool _validateField(GameObject fieldObject, string expectedValue, string violationMessage, ref string violationField)
    {
        var textComponent = fieldObject.GetComponent<TMP_Text>();
        if (textComponent.text == expectedValue) return true;

        violationField = violationMessage;
        _correctInput = false;
        return false;
    }
}