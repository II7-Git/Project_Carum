using System.Collections;
using TMPro;
using UnityEngine;

namespace Carum.Pets.Text
{
    public class PetText : MonoBehaviour
    {
        public static PetText Instance { get; private set; }
        public GameObject talkPanel;
        public string message;
        public string emotion;
        //public NPCConversation conversation;
        [SerializeField] private TypeEffect typeEffect;
        [SerializeField] private TextMeshProUGUI textTitle;
        //[SerializeField] private ConversationController conversationController;
        private CameraController _cameraController;

        string text;
        // Start is called before the first frame update
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                GameObject.Destroy(this.gameObject);
            }
            Instance = this;
            _cameraController = Camera.main.GetComponent<CameraController>();

        }
    
        // Update is called once per frame
        // void Update()
        // {
        
        //     textTitle.text=message;
        //     if(textTitle.text==""){
        //         talkPanel.SetActive(false);
        //     }else{
        //         talkPanel.SetActive(true);
        //     }
        // }

        // public void setMessage(string newMessage){
        //     message=newMessage;
        // }

        public void DoConversation(string text,string emotion){
            //conversationController.DoConversation();
            _cameraController.setCameraMode(1);

            GameObject player=GameObject.FindWithTag("Player");
            player.GetComponent<ActionController>().doAction(-1);
            player.GetComponent<VisualController>().newFace=emotion;
            // EditableConversation newCon= conversation.DeserializeForEditor();
            // newCon.SpeechNodes[0].Text=text;
            // //newCon.SpeechNodes[0].EditorInfo.xPos=50f;
            // //newCon.SpeechNodes[0].EditorInfo.yPos=50f;
            // newCon.SpeechNodes[0].Volume=0.03f;
            // conversation.Serialize(newCon);
        
            // ConversationManager.Instance.StartConversation(conversation);
            //textTitle.text=text;
            talkPanel.SetActive(true);

            this.text=text;
            typeEffect.SetMsg(this.text);
        }

        public void EndConversation(){
            if(typeEffect.isAnim){//아직 채팅 생기는 중이면 완성 시키기
                typeEffect.SetMsg(text);
                return;
            }
            PetManager.Instance.EndConversation();
            StartCoroutine(ConversationExit());
            //talkPanel.SetActive(false);
        }

        IEnumerator ConversationExit(){
            yield return new WaitForSeconds(0.5f);
            //Debug.Log("fsafa");
            talkPanel.SetActive(false);

        }
        public void TestConversation(){
            DoConversation(message,emotion);
        }
    }
}
