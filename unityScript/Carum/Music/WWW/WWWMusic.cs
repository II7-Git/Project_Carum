using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WWWMusic : MonoBehaviour
{
    public string url;
    public AudioSource source;

    IEnumerator Start()
    {
        source = GetComponent<AudioSource>();
        using (var www = new WWW(url))
        {
            yield return www;
            Debug.Log(url);
            Debug.Log(www);
            source.clip = (AudioClip) www.GetAudioClip(false, true,AudioType.MPEG);
        }
    }

    void Update()
    {
        if (!source.isPlaying && source.clip.isReadyToPlay)
            source.Play();
    }
}
