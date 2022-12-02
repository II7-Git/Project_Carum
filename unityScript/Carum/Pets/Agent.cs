using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    // 목표 지점
    public Transform target;
    NavMeshAgent agent;
    ActionController actionController;

    private bool isChange=false;
    void Awake()
    {
        // 해당 개체의 NavMeshAgent 를 참조합니다.
        agent = GetComponent<NavMeshAgent>();
        actionController = GetComponent<ActionController>();    
        target=GameObject.FindWithTag("Destination").transform;
    }
 
    // void Update()
    // {
    //     if(target==null||target.gameObject.activeSelf==false){
    //         isChange=true;
    //         // target=GameObject.FindWithTag("Destination").transform;
    //         // NavMeshBake.Bake();
    //     }

    //     if(isChange){
    //         if(GameObject.FindWithTag("Player")!=null){
    //             target=GameObject.FindWithTag("Destination").transform;
    //             NavMeshBake.Bake();
    //             isChange=false;
    //         }
    //     }

    //     // 매프레임마다 목표지점으로 이동합니다.
    //     // action이 걷기 일때만 실행
    //     // if(actionController.actionNum==2){
    //     //     agent.SetDestination(target.position);
    //     // } 
    // }


    public void move(){
        // 매프레임마다 목표지점으로 이동합니다.
        // action이 걷기 일때만 실행
        //Debug.Log(actionController.isMoving);
        if(!actionController.isMoving){//제자리
            agent.isStopped=true;
        } else{//이동
            if(actionController.actionNum<10){//걷기
                agent.speed=1f; 
            }else if(actionController.actionNum<15){//뛰기
                agent.speed=2f;
            }
            agent.SetDestination(target.position);
            
            agent.isStopped=false;
        }
    }
}