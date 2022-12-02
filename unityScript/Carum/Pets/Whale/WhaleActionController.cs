using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleActionController : ActionController
{
    public AudioClip walkSound;
    //public AudioClip runSound;

    private void Start() {
        animatorController=this.GetComponent<AnimatorController>();
        agent=this.GetComponent<Agent>();
        audioSource=GetComponent<AudioSource>();
        audioSource.clip=walkSound;
    }
    void FiexdUpdate(){
        //랜덤 자동 액션 설정
        if(!isAction){
            //isAction=true;
            //actionNum=Random.Range(1,4);//1~3
            //actionNum=1;
            //Debug.Log("isCalled");
            doAction(0);
        }

        if(isMoving&& !isAudioPlaying){
            isAudioPlaying=true;
            if(isRunning)
                StartCoroutine(PlayAudio(2.0f));
            else
                StartCoroutine(PlayAudio(1.0f));
            //AudioSource.PlayClipAtPoint(walkSound,transform.position);
        }
    }
    //whale 액션
    public override void doAction(int num){
        isRunning=false;
        randomNum=Random.Range(0,int.MaxValue);
        if(num==0){//랜덤 액션
            actionNum=Random.Range(1,20);
        }else{
            actionNum=num;
        }
        //Debug.Log(actionNum);
        isAction=true;
        if(actionNum==-1){//무기한 대기//풀어주는 구간 필요
            isMoving=false;
            animatorController.SetInt("animation,1");
        }
        else if(actionNum < 5){//대기
        isMoving=false;
            animatorController.SetInt("animation,1");
            float waitTime=Random.Range(3,5);
            StartCoroutine(waitMode(waitTime));
        }else if(actionNum<10){//걷기 actionNum2
            isMoving=true;
            animatorController.SetInt("animation,6");
        }else if(actionNum<15){//뛰기
            isMoving=true;
            isRunning=true;
            animatorController.SetInt("animation,7");
        }else if(actionNum<18){//다양한 감정 표현//해피
            isMoving=false;
            int actNum=Random.Range(0,5);
            if(actNum==0){//해피
                animatorController.SetInt("animation,3");
            }else if(actNum==1){//점프
                animatorController.SetInt("animation,8");
            }
            else if(actNum==2){//yes
                animatorController.SetInt("animation,5");
            }
            else if(actNum==3){//no
                animatorController.SetInt("animation,4");
            }
            else if(actNum==4){// atk
                animatorController.SetInt("animation,2");
            }
            StartCoroutine(waitMode(3f));
        }else if(actionNum<19){//죽은척
            isMoving=false;
             int actNum=Random.Range(0,2);
            if(actNum==0){
                animatorController.SetInt("animation,10");
            }else{
                animatorController.SetInt("animation,9");
            }
            
            StartCoroutine(waitMode(3f));
        }else if(actionNum<20){//밥먹기
            isMoving=false;
            animatorController.SetInt("animation,5");
            
            StartCoroutine(waitMode(3f));
        }

        agent.move();
    }

    private Dictionary<string,int> actDict=new Dictionary<string, int>(){
        {"HAPPY",3},
        {"JUMP",8},
        {"YES",5},
        {"NO",4},
        {"ATTACK",2},
        {"DEATH",9},
        {"EAT",5}
    };

    public override void doAction(string action,float time){
        isRunning=false;
        isAction=true;
        isMoving=false;

        animatorController.SetInt("animation,"+actDict[action]);
        StartCoroutine(waitMode(time));
        agent.move();
    }
}
