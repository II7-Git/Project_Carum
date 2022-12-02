using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSceneObjectMover : MonoBehaviour
{
    public float moveSpeed = 0.7f;
    public float maxDistance = 7f;
    private Vector3 _startPos;

    private Transform _transform;

    private Vector3 _move;
    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _startPos = _transform.position;
        _move = new Vector3(-moveSpeed, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        _transform.position += _move*Time.deltaTime;
        if (Mathf.Abs(_transform.position.x) > maxDistance)
        {
            _transform.position = new Vector3(maxDistance, _startPos.y, _startPos.z);
            
        }
        
    }
}
