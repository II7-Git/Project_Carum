using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float speed = 3.5f;
    private float X;
    private float Y;
    public Camera camera;
    private bool activate = false;

    void Update()
    {
        if (!activate) return;
        // if (Input.GetMouseButton(0))
        // {
        //     transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * speed, -Input.GetAxis("Mouse X") * speed, 0));
        //     X = transform.rotation.eulerAngles.x;
        //     Y = transform.rotation.eulerAngles.y;
        //     transform.rotation = Quaternion.Euler(X, Y, 0);
        // }
        if (Input.GetMouseButton(1))
        {
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * speed, -Input.GetAxis("Mouse X") * speed, 0));
            X = transform.rotation.eulerAngles.x;
            Y = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(X, Y, 0);
        }

        if (Input.GetMouseButton(0))
        {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            // float angle = transform.rotation.eulerAngles.y*Mathf.Deg2Rad;
            //
            // transform.position +=
            //     new Vector3(x * Mathf.Cos(angle), 0, y * Mathf.Cos(angle));
            camera.transform.position += new Vector3(-x, 0, -y);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        camera.orthographicSize -= scroll;
    }

    public void ToggleMode()
    {
        activate = !activate;
    }
}