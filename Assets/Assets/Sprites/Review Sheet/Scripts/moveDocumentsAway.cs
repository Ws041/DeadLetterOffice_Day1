using interactObjects;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SlideAwayMovement : MonoBehaviour
{
    private Vector3 _sentAwayPos;
    private AudioSourcePool _audioSourcePool;

    private void Awake()
    {
        _audioSourcePool = GameObject.FindGameObjectWithTag("AudioPool")?.GetComponent<AudioSourcePool>();
    }
    private void Start()
    {
        _audioSourcePool.SFX_PaperPickUp.Play();
        _sentAwayPos = new(transform.position.x - 20f, transform.position.y, 0f);
        Destroy(GetComponent<DragDropMovement>());
        Destroy(GetComponent<SpawnSlideMovement>());
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _sentAwayPos, Time.deltaTime * 1.8f);
    }
}
