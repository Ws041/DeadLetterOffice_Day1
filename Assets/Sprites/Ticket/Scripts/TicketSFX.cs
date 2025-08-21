using interactObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketSFX : MonoBehaviour
{
    private AudioSourcePool _audioSourcePool;

    private void Awake()
    {
        _audioSourcePool = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<AudioSourcePool>();
    }
    void Start()
    {
        _audioSourcePool.SFX_PrintTicket.Play();
    }

    private void _deleteAnim()
    {
        Destroy(GetComponent<Animator>());
    }

}
