using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TypeEffect : MonoBehaviour
{
    public float CharPerSeconds;
    public string targetMsg;
    public AudioSource audioSource;
    public TMP_Text msgText;
    int index;
    //float interval;
    // Start is called before the first frame update
    public bool isAnim;
    public void SetMsg(string msg)
    {   
        if(isAnim){
            CancelInvoke();
            msgText.text=targetMsg;
            EffectEnd();
            return;
        }
        targetMsg=msg;
        EffectStart();
    }

    private void EffectStart()
    {
        msgText.text="";
        index=0;
        isAnim=true;
        //interval = 1.0f/CharPerSeconds;
        Invoke("Effecting",1.0f/CharPerSeconds);
    }
    private void Effecting()
    {

        if(msgText.text == targetMsg){
            EffectEnd();
            return;
        }
        
        msgText.text += targetMsg[index];
        if(targetMsg[index] != ' ' && targetMsg[index] != '.')
            audioSource.Play();
        index++;
        
        Invoke("Effecting",1/CharPerSeconds);
    }
    private void EffectEnd()
    {
        isAnim=false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
