using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHover : MonoBehaviour
{
    private bool mouseOver = false;
    [SerializeField] private Transform unhoverPos, hoverPos;
    [SerializeField] private float moveSpeed = 0.25f;
    [SerializeField] private float ogAngle, hoverAngle;
    private AudioSourcePool _audioSourcePool;
    private ScoreTracker _scoreTracker;
    private void Start()
    {
        transform.eulerAngles = new Vector3(0, 0, ogAngle);
        transform.position = unhoverPos.position;
    }

    private void Awake()
    {
        _audioSourcePool = GameObject.FindGameObjectWithTag("AudioPool").GetComponent<AudioSourcePool>();
        _scoreTracker = GameObject.FindGameObjectWithTag("ScoreTracker").GetComponent<ScoreTracker>();
    }
    private void OnMouseEnter()
    {
        if (!_scoreTracker.IsStartDay) return;
        mouseOver = true;
        _coffeePlaySound(true);
    }

    private void OnMouseExit()
    {
        if (!_scoreTracker.IsStartDay) return;
        mouseOver = false;
        _coffeePlaySound(false);
    }


    private void Update()
    {
        if (mouseOver)
        {
            Quaternion targetAngle = Quaternion.Euler(0,0,hoverAngle);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetAngle, Time.deltaTime * moveSpeed);
            transform.position = Vector2.Lerp(transform.position, hoverPos.position,Time.deltaTime * moveSpeed);
        }
        else
        {
            Quaternion targetAngle = Quaternion.Euler(0, 0, ogAngle);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetAngle, Time.deltaTime * moveSpeed);
            transform.position = Vector2.Lerp(transform.position, unhoverPos.position, Time.deltaTime * moveSpeed);
        }
    }

    private void _coffeePlaySound(bool isMouseEnter)
    {
        if (gameObject.name != "Coffee") return;
        if (isMouseEnter)
        {
            _audioSourcePool.SFX_CupSlide1.Play();
            return;
        }
        _audioSourcePool.SFX_CupSlide2.Play();
    }
}
