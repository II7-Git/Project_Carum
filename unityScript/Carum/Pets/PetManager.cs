using System;
using System.Collections;
using System.Collections.Generic;
using Carum.Pets.Text;
using Carum.Util;
using UnityEngine;

public class PetManager : MonoBehaviour
{

    public static PetManager Instance { get; private set; }
    public bool force=false;//true이면 강제로 마우스 액션 동작하지 못하게 한다
    public MouseAction mouseAction;
    private GameObject pet;
    
    //대화 끝날 때 사용할 것 들
    private int preCamera=0;
    private bool preForce=false;
    [SerializeField]private Destination dest;
    private CameraController _cameraController;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            GameObject.Destroy(this.gameObject);
        }
        Instance = this;
        
        _cameraController = Camera.main.GetComponent<CameraController>();

    }
    

    private void Start() {
        //dest=GameObject.FindWithTag("Destination").GetComponent<Destination>();
    }
    public GameObject MakePet(string color, string emotion, string type) {// 펫 생성하기
        pet=transform.GetComponent<PetCreateTest>().MakePet(color,emotion, type);
        return pet;
        //pet.transform.position=new Vector3(1.0f,0f,1.0f);
        //Debug.Log(pet.transform.position);
    } 
    public void MakePet() {// 펫 생성하기
        pet=transform.GetComponent<PetCreateTest>().MakePet();
        //pet.transform.position=new Vector3(1.0f,0f,1.0f);
        //Debug.Log(pet.transform.position);
    } 

    public void DoConversation(string text, string emotion) {
        this.preForce=force; //대화를 종료한 뒤 동작 여부 설정 
        this.force=true; 
        transform.GetComponent<PetText>().DoConversation(text,emotion);
    }  //펫과의 대화

    public void ChangeMode()
    {
        preCamera = (preCamera+ 1) % 3;
        switch (preCamera)
        {
            case 1:
                CloseUp();
                break;
            case 0:
                CloseOut();
                break;
            case 2:
                Observe();
                break;
        }
    }

    public void ToggleCamera(){
        if (preCamera==1){
            return;
        }else if(preCamera==0){
            Observe();
        }else{
            CloseOut();
        }
    }
    public void CloseUp() { 
        force=true;
        preCamera=1;
        Camera.main.GetComponent<CameraController>().setCameraMode(1);
        //Debug.Log(pet);
        pet.GetComponent<ActionController>().doAction(-1);
    }  //아무 행동 안하고 화면 고정 상태

    public void CloseOut() {
        force=false;
        preCamera=0;
        Camera.main.GetComponent<CameraController>().setCameraMode(0);
        pet.GetComponent<ActionController>().doAction(0);
     }  // 카메라 멀리보고 자유 관찰

    public void Observe() { 
        force=false;
        preCamera=2;
        Camera.main.GetComponent<CameraController>().setCameraMode(2);
        pet.GetComponent<ActionController>().doAction(0);
    }  //자유 행동 , 카메라 깊게 관찰

    public void SetVisual(string emotion, string color) { 
        pet.GetComponent<VisualController>().setEmotion(emotion);
        pet.GetComponent<VisualController>().setColor(color);
        pet.GetComponent<VisualController>().setDefaultFace(emotion);
    }  // 펫의 생김새 변경

    public void SetVisual(string json)
    {
        RequestDto.PetVisualDto dto = JsonUtility.FromJson<RequestDto.PetVisualDto>(json);
        SetVisual(dto.face,dto.color);
    }

    public void DoAction(string text, float time) {
        //text에 따라 처리 필요
        // "HAPPY"
        // "JUMP"
        // "YES",11
        // "NO",12
        // "ATTACK",6
        // "DEATH",8
        // "EAT",5
        pet.GetComponent<ActionController>().doAction(text,time);
     } //애니메이션만 바꾸는

    public void MoveTo(Vector3 position) {
        pet.GetComponent<ActionController>().setAction(false);
        dest.changePosition(position);
        pet.GetComponent<ActionController>().doAction(13);//10~15 걷기 액션
    } //position 설정 후 이동 후 자유롭게`

    public void EndConversation(){
        //Debug.Log("end");
        //대화 종류에 따라서 원래 시점으로 돌아가야하지 않을까??
        force=preForce;
        if(!force)DoAction("HAPPY",0.5f);
        Camera.main.GetComponent<CameraController>().setCameraMode(preCamera);
        pet.GetComponent<VisualController>().doDefaultFace();
    }

    public void InitPet(string color,string emotion,string type)
    {
        // Debug.Log("INIT PET");
        // dest.SetRoomBase(roomBase);
        if (this.pet != null)
            Destroy(this.pet.gameObject);
        
        GameObject pet = MakePet(color, emotion, type);
        dest.SetPet(pet);
        mouseAction.SetPet(pet);
        _cameraController.target = pet.transform;
        // _cameraController.room = roomBase.transform;
    }
}
