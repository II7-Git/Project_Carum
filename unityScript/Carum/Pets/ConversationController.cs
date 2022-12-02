using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationController : MonoBehaviour
{
        // Start is called before the first frame update
        public void DoConversation(){
            gameObject.SetActive(true);
        }
        public void EndConversation(){
            PetManager.Instance.EndConversation();
            gameObject.SetActive(false);
        }
    }
