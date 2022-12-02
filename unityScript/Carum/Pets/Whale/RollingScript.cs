using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RollingScript : MonoBehaviour
{

    public float rollingSpeed = -80f;
    private Transform _transform;
    private void Start()
    {
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        _transform.Rotate(new Vector3(0,0,rollingSpeed*Time.deltaTime));
    }
}
