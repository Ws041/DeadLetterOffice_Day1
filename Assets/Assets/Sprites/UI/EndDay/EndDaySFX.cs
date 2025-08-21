using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDaySFX : MonoBehaviour
{
    private AudioSourcePool _audioSourcePool;

    private void Awake()
    {
        _audioSourcePool = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<AudioSourcePool>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _audioSourcePool.SFX_PaperFold.Play();
    }

}
