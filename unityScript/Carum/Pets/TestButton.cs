using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("펫 생성")]
    [SerializeField] string color;
    [SerializeField] string emotion;
    [SerializeField] string type;

    [Header("대화")]
    [SerializeField] string text;
    [SerializeField] string textEmotion;

    [Header("펫 비주얼 변경")]
    [SerializeField] string visualColor;
    [SerializeField] string visualEmotion;

    [Header("액션")]
    [SerializeField] string action;
    [SerializeField] float time;

    private void Start() {
        PetManager.Instance.MakePet(color,emotion,type);   
    }
    public void MakePet() {// 펫 생성하기
        PetManager.Instance.MakePet(color,emotion,type);
    } 

    public void DoConversation() { 
        PetManager.Instance.DoConversation(text,textEmotion);
    }  //펫과의 대화

    public void CloseUp() { 
        PetManager.Instance.CloseUp();
    }  //아무 행동 안하고 화면 고정 상태

    public void CloseOut() {
        PetManager.Instance.CloseOut();
     }  // 카메라 멀리보고 자유 관찰

    public void Observe() { 
        PetManager.Instance.Observe();
    }  //자유 행동 , 카메라 깊게 관찰

    public void SetVisual() { 
        PetManager.Instance.SetVisual(visualEmotion,visualColor);
    }  // 펫의 생김새 변경

    public void DoAction() {
        PetManager.Instance.DoAction(action,time);
     } //애니메이션만 바꾸는

    public void MoveTo() {
        PetManager.Instance.MoveTo(new Vector3(5,0,2));
    } //position 설정 후 이동 후 자유롭게
}
