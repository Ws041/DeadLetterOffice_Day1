using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//When assigned to a game object, said object will immediately slide downwards
//to mimic of a mail sliding towards the desk
public class SpawnSlideMovement : MonoBehaviour
{
    [SerializeField] private float spawnSpeed = 0.5f, minDistanceToTarget = 0.05f;
    private Vector2 targetLocation;
    private void Start()
    {
        targetLocation = new(transform.position.x, 0f);
        transform.SetAsLastSibling();
        transform.position = new Vector3(transform.position.x, transform.position.y, -23);
    }

    private void Update()
    {
        _spawnLerpMovement();
        _updateSortingLayerChildren();
    }

    private void _spawnLerpMovement() //Using lerp to smooth the movement
    {
        transform.position = Vector2.Lerp(transform.position, targetLocation, Time.deltaTime * spawnSpeed);
        _checkSpawnDistance();
    }

    private void _checkSpawnDistance() //To prevent from infinitely inching towards the target (because lerp does that)
    {
        if(Mathf.Abs(targetLocation.y - transform.position.y) <= minDistanceToTarget)
        {
            transform.position = targetLocation;
            Destroy(GetComponent<SpawnSlideMovement>());
        }
    }

    private void _updateSortingLayerChildren() //Have slided game object be on top of the pile for aesthetics
    {
        int order = 0;
        foreach (Transform child in transform.parent.transform)
        {
            child.gameObject.transform.position = new Vector3(child.gameObject.transform.position.x,
                child.gameObject.transform.position.y, order);
            order--;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, order - 1);

    }

}

