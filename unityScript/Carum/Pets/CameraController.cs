using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    
    public float zoomSize = 0.5f;
    public Transform target;        // 따라다닐 타겟 오브젝트의 Transform
    public Transform room;
    
    private Transform tr;                // 카메라 자신의 Transform
    private Transform parent;
    private Vector3 cameraDefault;
    public bool isFollow=false; //따라가냐 안따라가냐
    public int cameraMode=0;// 0: 기본 1: 클릭 2:따라다니기 3:대화
    private bool isChange=false;
    Vector3 speed = Vector3.zero; // (0,0,0) 은 .zero 로도 표현가능

    private Camera _camera;

    [SerializeField]
    private CinemachineVirtualCamera[] virtualCameras;
   
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            GameObject.Destroy(this.gameObject);
        }
        Instance = this;
    }

    void Start()
    {
        //target=GameObject.FindWithTag("Player").transform;
        // room=GameObject.FindWithTag("RoomTarget").transform;
        tr = transform;
        parent = tr.parent;
        tr.LookAt(room);
        cameraDefault=tr.position;//카메라 맨처음 위치
        // cameraDefault = parent.position;
        _camera = GetComponent<Camera>();

    }
 
    public void setFollow(bool isFollow){
        this.isFollow=isFollow;
    }
    public void setCameraMode(int newMode){
        this.cameraMode=newMode;
    }
    void LateUpdate()
    {
        if(target==null||target.gameObject.activeSelf==false){
            isChange=true;
            return;
        }

        if(isChange){
            if(GameObject.FindWithTag("Player")!=null){
                target=GameObject.FindWithTag("Player").transform;
                isChange=false;
            }
        }
        else{

        if(cameraMode==0){
            // tr.position = Vector3.Lerp(tr.position, cameraDefault, 0.03f);
            // _camera.orthographicSize=3.5f;
            // tr.LookAt(room);
                // parent.position = Vector3.Lerp(parent.position, cameraDefault, 0.01f);
                // _camera.orthographicSize = 3.5f;

                //cinemachine
            OnChangeCamera(0);
        }
        else if(cameraMode==1){//클릭 및 대화
            
            // Vector3 destination=new Vector3(target.position.x + 7f, target.position.y+5f, target.position.z - 3f);
            // Vector3 focus = target.transform.position;
            // focus.y += 0.5f;
            // tr.position = Vector3.Lerp(tr.position, destination, 0.03f);
            // _camera.orthographicSize=zoomSize;
             target.LookAt(tr);
            // tr.LookAt(focus);

            OnChangeCamera(1);
        }
        else if(cameraMode==2){//따라다니기
           
            // Vector3 destination=new Vector3(target.position.x + 7f, target.position.y+5f, target.position.z -3f);
            // tr.position = Vector3.Lerp(tr.position, destination, 0.03f);
            // _camera.orthographicSize=1.0f;
            // tr.LookAt(target);

            OnChangeCamera(2);
        }else if(cameraMode==3){//대화

            Vector3 destination=new Vector3(target.position.x- 8f, target.position.y+4f, target.position.z );
            tr.position = Vector3.Lerp(tr.position, destination, 0.03f);
            _camera.orthographicSize=1.0f;
            tr.LookAt(target);
            target.LookAt(tr);
        }
        }
     }


     void OnChangeCamera(int num){
        if(virtualCameras[num].Priority==10)return;

        int count = virtualCameras.Length;
        virtualCameras[num].Priority = 10;
        for (int i = 0; i < count; i++)
        {
            if(i!=num){
                virtualCameras[i].Priority = 0;
            }
        }
    }
     
}
