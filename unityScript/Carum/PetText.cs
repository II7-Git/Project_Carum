// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using DialogueEditor;
//
// public class PetText : MonoBehaviour
// {
//     public static PetText Instance { get; private set; }
//     public GameObject talkPanel;
//     public string message;
//     public string emotion;
//     public NPCConversation conversation;
//     [SerializeField] private TextMeshProUGUI textTitle;
//     [SerializeField] private ConversationController conversationController;
//
//     private CameraController _cameraController;
//     // Start is called before the first frame update
//     void Awake()
//     {
//         if (Instance != null && Instance != this)
//         {
//             GameObject.Destroy(this.gameObject);
//         }
//         Instance = this;
//         _cameraController = Camera.main.GetComponent<CameraController>();
//     }
//     
//
//     // Update is called once per frame
//     // void Update()
//     // {
//         
//     //     textTitle.text=message;
//     //     if(textTitle.text==""){
//     //         talkPanel.SetActive(false);
//     //     }else{
//     //         talkPanel.SetActive(true);
//     //     }
//     // }
//
//     // public void setMessage(string newMessage){
//     //     message=newMessage;
//     // }
//
//     public void DoConversation(string text,string emotion){
//         conversationController.DoConversation();
//         _cameraController.setCameraMode(1);
//         GameObject player=GameObject.FindWithTag("Player");
//         player.GetComponent<ActionController>().doAction(-1);
//         player.GetComponent<VisualController>().newFace=emotion;
//         EditableConversation newCon= conversation.DeserializeForEditor();
//         newCon.SpeechNodes[0].Text=text;
//         //newCon.SpeechNodes[0].EditorInfo.xPos=50f;
//         //newCon.SpeechNodes[0].EditorInfo.yPos=50f;
//         newCon.SpeechNodes[0].Volume=0.03f;
//         conversation.Serialize(newCon);
//         
//         ConversationManager.Instance.StartConversation(conversation);
//     }
//
//     public void TestConversation(){
//         DoConversation(message,emotion);
//     }
// }
