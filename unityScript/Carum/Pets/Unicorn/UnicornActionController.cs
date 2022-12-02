using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicornActionController : ActionController
{
    //unicorn 액션
    public override void doAction(int num){
        if(num==0){//랜덤 액션
            actionNum=Random.Range(1,20);
        }else{
            actionNum=num;
        }
        Debug.Log(actionNum);
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
            animatorController.SetInt("animation,5");
        }else if(actionNum<18){//다양한 감정 표현//해피
            isMoving=false;
            int actNum=Random.Range(0,7);
            if(actNum==0){//해피
                animatorController.SetInt("animation,3");
            }else if(actNum==1){//점프
                animatorController.SetInt("animation,11");
            }
            else if(actNum==2){//fly
                animatorController.SetInt("animation,9");
            }
            else if(actNum==3){//no
                animatorController.SetInt("animation,4");
            }
            else if(actNum==4){//horn atk
                animatorController.SetInt("animation,8");
            }
            else if(actNum==5){//fly
                animatorController.SetInt("animation,9");
            }
            else if(actNum==6){//fly
                animatorController.SetInt("animation,9");
            }
            StartCoroutine(waitMode(3f));
        }else if(actionNum<19){//죽은척
            isMoving=false;
            animatorController.SetInt("animation,10");
            
            StartCoroutine(waitMode(3f));
        }else if(actionNum<20){//밥먹기
            isMoving=false;
            animatorController.SetInt("animation,7");
            
            StartCoroutine(waitMode(3f));
        }

        agent.move();
    }
}
