using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    protected AnimatorController animatorController;
    protected Agent agent;
    //현재 하고 있는 액션이 있는가?
    //아무 액션 없다면 새 액션 시작
    protected bool isAction=false;
    public bool isMoving=false;
    public bool isRunning=false;
    public int actionNum=1;
    //1:대기
    //2:걷기
    //3:뛰기


    protected int randomNum;

    protected AudioSource audioSource;
    protected bool isAudioPlaying;
    // Start is called before the first frame update
    

    // Update is called once per frame
    // void Update()
    // {
       
    //     //랜덤 자동 액션 설정
    //     if(!isAction){
    //         //isAction=true;
    //         //actionNum=Random.Range(1,4);//1~3
    //         //actionNum=1;
    //         //Debug.Log("isCalled");
    //         doAction(0);
    //     }
    // }

    protected IEnumerator PlayAudio(float time){
        audioSource.Play();
        yield return new WaitForSeconds(time);
        isAudioPlaying=false;
        
    }
    //1:대기
    //2:걷기
    //3:뛰기
    //펫에 맞게 이부분만 오버라이딩
    public virtual void doAction(string action,float time){
        
    }
    public virtual void doAction(int num){
        // if(num==0){//랜덤 액션
        //     actionNum=Random.Range(1,20);
        // }else{
        //     actionNum=num;
        // }
        // Debug.Log(actionNum);
        // isAction=true;
        // if(actionNum==-1){//무기한 대기//풀어주는 구간 필요
        //     isMoving=false;
        //     animatorController.SetInt("animation,1");
        // }
        // else if(actionNum < 5){//대기
        // isMoving=false;
        //     animatorController.SetInt("animation,1");
        //     float waitTime=Random.Range(3,5);
        //     StartCoroutine(waitMode(waitTime));
        // }else if(actionNum<10){//걷기 actionNum2
        //     isMoving=true;
        //     animatorController.SetInt("animation,21");
        // }else if(actionNum<15){//뛰기
        //     isMoving=true;
        //     animatorController.SetInt("animation,18");
        // }else if(actionNum<18){//다양한 감정 표현//해피
        //     isMoving=false;
        //     int actNum=Random.Range(0,7);
        //     if(actNum==0){//해피
        //         animatorController.SetInt("animation,2");
        //     }else if(actNum==1){//점프
        //         animatorController.SetInt("animation,10");
        //     }
        //     else if(actNum==2){//yes
        //         animatorController.SetInt("animation,11");
        //     }
        //     else if(actNum==3){//no
        //         animatorController.SetInt("animation,12");
        //     }
        //     else if(actNum==4){//sick
        //         animatorController.SetInt("animation,13");
        //     }
        //     else if(actNum==5){//jumpGround
        //         animatorController.SetInt("animation,14");
        //     }
        //     else if(actNum==6){//stun
        //         animatorController.SetInt("animation,4");
        //     }
        //     StartCoroutine(waitMode(3f));
        // }else if(actionNum<19){//죽은척
        //     isMoving=false;
        //     int actNum=Random.Range(0,2);
        //     if(actNum==0){
        //         animatorController.SetInt("animation,8");
        //     }else{
        //         animatorController.SetInt("animation,9");
        //     }
            
        //     StartCoroutine(waitMode(3f));
        // }else if(actionNum<20){//밥먹기
        //     isMoving=false;
        //     int actNum=Random.Range(0,2);
        //     if(actNum==0){
        //         animatorController.SetInt("animation,5");
        //     }else{
        //         animatorController.SetInt("animation,28");
        //     }
            
        //     StartCoroutine(waitMode(3f));
        // }

        // agent.move();
    }

    public void setAction(bool done){
        isAction=done;
    }

    public void setActionNum(int num){
        actionNum=num;
    }
    protected IEnumerator waitMode(float waitTime){
        int preActionNum=randomNum;
        //Debug.Log(preActionNum);
        yield return new WaitForSeconds(waitTime);
        //Debug.Log(actionNum);
        if(randomNum==preActionNum){//새로운 액션이 들어오지 않았다면 무기한 정지
            if(PetManager.Instance.force){//만약 움직이지 못하는 상황이라면?
                doAction(-1);
            }
            else{
                doAction(0);//자유 행동 시작
            }
            //Debug.Log(isAction);
        }
    }
}
