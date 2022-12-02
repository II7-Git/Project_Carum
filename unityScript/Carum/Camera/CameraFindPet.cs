using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraFindPet : MonoBehaviour
{
    private CinemachineVirtualCamera cam;

    [SerializeField]
    private CameraController cameraController;
    private void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }
    // Update is called once per frame
    void Update()
    {
        if(!cam.LookAt){
            cam.LookAt = cameraController.target;
        }
    }
}
