using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;

//This spawns a ticket. Literally
//Occours when player submits a mail wrong.
public class TicketSpawner : MonoBehaviour
{
    private ScoreTracker _scoreTracker; //To access how many mails player got wrong
    [SerializeField] private GameObject _ticket;

    private void Awake()
    {
        _scoreTracker = GameObject.FindGameObjectWithTag("ScoreTracker").GetComponent<ScoreTracker>();
    }
    public IEnumerator SpawnTicket(string _senderViolation, string _receiverViolation) //Called by CheckReviewAccuracy script
    {
        yield return StartCoroutine(_workaroundSpawnTicket(_senderViolation, _receiverViolation));
        
    }

    //PLEASE KEEP THIS METHOD
    //Not sure why but yield return new WaitForSeconds(2f) just DOESN'T WORK on SpawnTicket() for some reason!
    //It used to work. Sometimes, if I cut paste it back up to SpawnTicket(), it works and it doesn't. 
    //So keep this method, to be safe. 
    //Hence why this is called _workaroundSpawnTicket. Lovely.
    private IEnumerator _workaroundSpawnTicket(string _senderViolation, string _receiverViolation)
    {
        yield return new WaitForSeconds(2);
        GameObject _ticketClone = Instantiate(_ticket, transform.position, Quaternion.identity, transform.parent.transform);
        TMP_Text _ticketClonePenaltyText = _ticketClone.transform.Find("Penalty").GetComponent<TMP_Text>();
        TMP_Text _ticketCloneWarningText = _ticketClone.transform.Find("Warning").GetComponent<TMP_Text>();
        TMP_Text _ticketCloneViolation = _ticketClone.transform.Find("Violation List").GetComponent<TMP_Text>();

        if (_scoreTracker.MailIncorrect == 1) //Player only gets one chance
        {
            _ticketClonePenaltyText.text = "NO PENALTY";
            _ticketCloneWarningText.text = "LAST WARNING";
        }
        else
        {
            _ticketClonePenaltyText.text = "PENALTY";
            _ticketCloneWarningText.text = "-5 MARKS";
        }

        _ticketCloneViolation.text = $"{_senderViolation}\r\n{_receiverViolation}";
    }
}
