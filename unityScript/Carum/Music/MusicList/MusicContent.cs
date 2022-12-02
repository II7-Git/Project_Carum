using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
public class MusicContent : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip music = null;
    public int id;
    public HajiyevMusicManager musicManager;
    public PlayListManager playListManager;

    string url="https://k7a101.p.ssafy.io/api/music/file/";

    private void Start()
    {
        transform.localScale = Vector3.one;
    }

    public void musicPlay()
    {
        //뮤직 교체해야한다
        //Debug.Log(id);
        if(music==null)
            StartCoroutine(getMusicFromApi(id));
            //MusicData.Instance.getMusic(id, music);
        
        StartCoroutine(CheckMusicAndPlay());
        // if (audioSource.clip != music)
        // {
        //     if (musicManager.IsPlaying())
        //     {
        //         musicManager.Pause();
        //         playListManager.transform.parent.GetChild(2).GetComponent<MusicController>().playButtonSet();
        //     }
        //     audioSource.Stop();
        //     audioSource.clip = music;
        //     audioSource.Play();
        // }
        // else
        // {//같은 뮤직 //정시 시작
        //     if (audioSource.isPlaying)
        //     {
        //         audioSource.Pause();
        //     }
        //     else
        //     {
        //         audioSource.UnPause();
        //     }
        // }
    }

    IEnumerator CheckMusicAndPlay()
    {
        if (music != null)
        {
            if (audioSource.clip != music)
            {
                if (musicManager.IsPlaying())
                {
                    musicManager.Pause();
                    MusicController.Instance.playButtonSet();
                }
                audioSource.Stop();
                audioSource.clip = music;
                audioSource.Play();
            }
            else
            {//같은 뮤직 //정시 시작
                if (audioSource.isPlaying)
                {
                    audioSource.Pause();
                }
                else
                {
                    audioSource.UnPause();
                }
            }
            yield return null;
        }else{
            yield return new WaitForSeconds(0.3f);
            StartCoroutine(CheckMusicAndPlay());
        }
    }
    public void playListAdd()
    {
        //이전에 이미 있는 곡인지 체크intern
        foreach (int listId in playListManager.getPlayListId())
        {
            if (id == listId)
            {
                return;
            }
        }

        //musicManager.AddToPlayList(music);
        playListManager.AddToPlayList(id);
    }
    // IEnumerator getMusicFromApi(int id){
    //     Debug.Log(url+id);
    //     using (var www = new WWW(url+id))
    //     {
    //         yield return www;
    //         music= (AudioClip) www.GetAudioClip(false, true,AudioType.MPEG);
            
    //     }
    // }
    IEnumerator getMusicFromApi(int id){
        UnityWebRequest unityWebRequest = UnityWebRequestMultimedia.GetAudioClip(url + id, AudioType.MPEG);
        yield return unityWebRequest.SendWebRequest();
        music = DownloadHandlerAudioClip.GetContent(unityWebRequest);
    }
}
