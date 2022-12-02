using System.Collections;
using System.Collections.Generic;
using Carum.Interior;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseAction : MonoBehaviour
{

    GameObject player;
    //ActionController actionController;
    Destination destination;
    // Start is called before the first frame update
    private bool isChange = false;

    public GameObject defaultSetting;


    
    List<Talk> talkList=new List<Talk>(){
        new Talk("무슨 일인가요?","NORMAL"),
        new Talk("무슨 일 있나요...?","WORRY"),
        new Talk("심심해요! 놀아줘요!","HEART"),
        new Talk("오늘 슬픈 영화를 봐서 울었어요 ㅠㅠ","SAD"),
        new Talk("저도 오늘 일기를 적었어요!","HAPPY"),
        new Talk("옆집 친구랑 하루종일 놀아서 즐거웠어요!","HAPPY"),
        new Talk("아무 일도 없어서 너무나 심심했어요...","WORRY"),
        new Talk("깜짝이야! 놀랐어요!","SURPRISE"),
        new Talk("빙빙 돌았더니 어지러워요~~","CONFUSE"),
        new Talk("햇살이 따듯해서 하루 종일 낮잠을 잤어요","PEACE"),
        new Talk("이러면 좀 무서워 보이나요?","ANGRY"),
        new Talk("책을 읽었는데 하나도 이해를 못하겠어요~","STUN"),
        new Talk("노래가 너무나도 제 취향이예요!","HAPPY"),
        new Talk("집이 제 취향이라 너무 마음에 들어요","HEART"),
        new Talk("굉장히 화가 나는 일이 있었어요","ANGRY"),
        new Talk("오늘 방 청소는 저도 도울게요!","NORMAL"),
        new Talk("이야기가 너무 슬퍼요 ㅠㅠ","SAD"),
    };

    class Talk {
        public string text;
        public string emotion;
        public Talk(string text,string emotion){
            this.text=text;
            this.emotion=emotion;
        }
    }
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        //actionController=GameObject.FindWithTag("Player").GetComponent<ActionController>();
        destination = GameObject.FindWithTag("Destination").GetComponent<Destination>();
    }

    // Update is called once per frame
    void Update()
    {

        // if(GameObject.FindWithTag("Player")!=null){
        //     actionController=GameObject.FindWithTag("Player").GetComponent<ActionController>();
        // }
        // if(GameObject.FindWithTag("Destination")!=null){
        //     destination= GameObject.FindWithTag("Destination").GetComponent<Destination>();
        // }
        if (InteriorManager.Instance._putMode != PutMode.None) return;
        if (player == null) return;
        // if (player == null || player.activeSelf == false)
        // {
        //     isChange = true;
        //     return;
        // }
        //
        // if (isChange)
        // {
        //     if (GameObject.FindWithTag("Player") != null)
        //     {
        //         player = GameObject.FindWithTag("Player");
        //         if(!player)return;
        //         isChange = false;
        //     }
        // }
        //사용하기 전 마우스 raycast에 걸릴만한 colider는 다 치워주셔야합니다.
        int touchCount = Input.touchCount;
        if (Input.GetMouseButtonDown(0) && touchCount == 0)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (PetManager.Instance.force) return;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Player")
                    {//player action
                     //펫을 클릭했을 때 동작
                     //Camera.main.GetComponent<CameraController>().setCameraMode(1);
                     //actionController.doAction(-1);

                        //defaultSetting.GetComponent<PetText>().setMessage("무슨 일인가???");
                        //defaultSetting.GetComponent<PetText>().DoConversation("무슨 일인가???","HAPPY");
                        int num= Random.Range(0,talkList.Count);
                        PetManager.Instance.DoConversation(talkList[num].text,talkList[num].emotion );
                    }
                    else
                    {//해당 위치로 이동
                     //펫을 클릭했을 때 동작
                     // if(Camera.main.GetComponent<CameraController>().cameraMode==1){
                     //     Camera.main.GetComponent<CameraController>().setCameraMode(0);
                     //     //player.GetComponent<VisualController>().doDefaultFace();
                     // }
                        player.GetComponent<ActionController>().setAction(false);

                        destination.changePosition(hit.point);
                        player.GetComponent<ActionController>().doAction(13);//10~15 걷기 액션
                    }
                }
            }
            else
            {
                //defaultSetting.GetComponent<PetText>().setMessage("");
            }
        }
        else if (touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended && !EventSystem.current.IsPointerOverGameObject())
            {
                if (PetManager.Instance.force) return;
                Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Player")
                    {//player action
                     //펫을 클릭했을 때 동작
                     //Camera.main.GetComponent<CameraController>().setCameraMode(1);
                     //actionController.doAction(-1);

                        //defaultSetting.GetComponent<PetText>().setMessage("무슨 일인가???");
                        //defaultSetting.GetComponent<PetText>().DoConversation("무슨 일인가???","HAPPY");
                        int num= Random.Range(0,talkList.Count);
                        PetManager.Instance.DoConversation(talkList[num].text,talkList[num].emotion );
                    }
                    else
                    {//해당 위치로 이동
                     //펫을 클릭했을 때 동작
                     // if(Camera.main.GetComponent<CameraController>().cameraMode==1){
                     //     Camera.main.GetComponent<CameraController>().setCameraMode(0);
                     //     //player.GetComponent<VisualController>().doDefaultFace();
                     // }
                        player.GetComponent<ActionController>().setAction(false);

                        destination.changePosition(hit.point);
                        player.GetComponent<ActionController>().doAction(13);//10~15 걷기 액션
                    }
                }
            }
            else
            {
                //defaultSetting.GetComponent<PetText>().setMessage("");
            }
        }

    }
    public void SetPet(GameObject pet)
    {
        this.player = pet;
    }
}
