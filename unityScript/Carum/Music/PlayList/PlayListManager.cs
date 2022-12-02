using System;
using System.Collections;
using System.Collections.Generic;
using Carum.Interior;
using UnityEngine;
using UnityEngine.Networking;

using TMPro;
using Carum.Util;

public class PlayListManager : MonoBehaviour
{
    public static PlayListManager Instance { get; private set; }
    public GameObject musicManagerObject;
    public GameObject playListItemPrefab;

    public MusicController musicController;
    [SerializeField] HajiyevMusicManager musicManager;
    [SerializeField] MusicData musicData;
    [SerializeField] Transform content;
    // Start is called before the first frame update
    // private const float inchToCm = 2.54f;
    // private EventSystem eventSystem = null;

    // private readonly float dragThresholdCM = 0.5f;
    private PlayList _playList;
    [SerializeField]private AudioSource audioSource;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            GameObject.Destroy(this.gameObject);
        }
        Instance = this;
    }

    void Start()
    {
        //musicManager = musicManagerObject.GetComponent<HajiyevMusicManager>();
        musicManager = HajiyevMusicManager.instance;
        musicData = MusicData.Instance;
        // content = gameObject.transform.GetChild(0).GetChild(0).GetChild(0);

        //musicManager.AutoAdvance();
        //HajiyevMusicManager.instance.AutoAdvance();
        //Debug.Log(HajiyevMusicManager.instance.autoAdvance);
        //setPlayList();

        // if (eventSystem == null)
        // {
        //     eventSystem = GetComponent<EventSystem>();
        // }

        // SetDragThreshold();
        audioSource = musicManager.gameObject.GetComponent<AudioSource>();
    }

    float preTime = -0.0f;
    int preMusicId = -1;
    int trackNumber = 0;
    int preTrackNum = -1;
    private void Update()
    {
        trackNumber = musicManager.CurrentTrackNumber();
        //노래 넘어가기

        //노래가 아예 없다면
        if(!musicManager.GetPlayListClip(trackNumber)){
            Stop();
            return;
        }

        
        //노래가 있는데 그 노래가 바뀌었다면?
        if(preMusicId !=musicData.playListId[trackNumber]){
            //preTrackNum=trackNumber;
            preMusicId = musicData.playListId[trackNumber];

            // if(preTrackNum!=trackNumber){//노래 위치가 바뀌었다//trackNum 위치로 이동
            //     preTrackNum = trackNumber;

            // }else{

            // }
            
            //musicManager.RewindClip(); 
            
            if(musicManager.IsPlaying())
                musicManager.Stop();
            musicManager.SetTrack(trackNumber);
            musicManager.Play();
            musicController.playButtonSet();
            
            preTime=0.0f;
        }
        else if(musicManager.IsPlaying()){
            if(preTime>audioSource.time){//다음곡 재생
                //Debug.Log("다음곡 왔냐?"+preTime+" "+audioSource.time);
                musicManager.Next();
            }else{//시간 갱신
                preTime=audioSource.time-0.5f;
            }
        }
    }
    
    private void Stop(){
        preTrackNum = -1;
        preMusicId = -1;
        preTime = 0.0f;
        musicManager.Stop();
        musicController.playButtonSet();
    }
    // private void SetDragThreshold()
    // {
    //     if (eventSystem != null)
    //     {
    //         eventSystem.pixelDragThreshold = (int)(dragThresholdCM * Screen.dpi / inchToCm);
    //     }
    // }
    // Update is called once per frame


    // //플레이 리스트를 다시 musicManager에 List에 따라 다시 그려주는 함수
    // public void setPlayList()
    // {
    //     //기존 리스트 초기화
    //     Transform[] oldList = content.GetComponentsInChildren<Transform>();

    //     if (oldList != null)
    //     {
    //         for (int i = 1; i < oldList.Length; i++)
    //         {
    //             if (oldList[i] != transform)
    //                 Destroy(oldList[i].gameObject);
    //         }
    //     }

    //     //새로운 리스트 만들기
    //     foreach (AudioClip musicClip in musicManager.playList)
    //     {
    //         GameObject newItem = Instantiate(playListItemPrefab);

    //         //제목&가수 설정
    //         //string titleAndSinger=item.title+"-"+item.artist;
    //         //newItem.transform.GetChild(0).GetComponent<TMP_Text>().text=titleAndSinger;
    //         //부모 설정
    //         newItem.transform.parent = content;
    //     }

    // }

    public void AddToPlayList(int id)
    {
        //Debug.Log("코루틴 시작 전");
        StartCoroutine(AddMusic(id));
        //Debug.Log("코루틴 끝");
        // musicData.playListId.Add(id);
        // AudioClip clip = null;
        // musicData.getMusic(id, clip);
        // musicManager.AddToPlayList(clip);
        // GameObject newItem = Instantiate(playListItemPrefab);
        // PlayListItem playListItem = newItem.GetComponent<PlayListItem>();
        // playListItem.musicManager = musicManager;
        // playListItem.id = id;
        // MusicItem musicItem = musicData.getMusicInfo(id);
        // string titleAndSinger = musicItem.title + "-" + musicItem.artist;
        // newItem.transform.GetChild(0).GetComponent<TMP_Text>().text = titleAndSinger;

        // newItem.transform.SetParent(content);
    }

    IEnumerator AddMusic(int id)
    {
        UnityWebRequest unityWebRequest = UnityWebRequestMultimedia.GetAudioClip(musicData.url + id, AudioType.MPEG);
        yield return unityWebRequest.SendWebRequest();
        AudioClip clip = DownloadHandlerAudioClip.GetContent(unityWebRequest);
        //Debug.Log("뮤직 더하기 시작");
        musicData.playListId.Add(id);
        musicManager.AddToPlayList(clip);
        //Debug.Log("프리팹 생성 직전");
        GameObject newItem = Instantiate(playListItemPrefab);
        //Debug.Log("변수들 할당 직전");
        PlayListItem playListItem = newItem.GetComponent<PlayListItem>();
        playListItem.musicManager = musicManager;
        playListItem.id = id;
        MusicItem musicItem = musicData.getMusicInfo(id);
        string titleAndSinger = musicItem.title + "-" + musicItem.artist;
        newItem.transform.GetChild(0).GetComponent<TMP_Text>().text = titleAndSinger;
        //Debug.Log("자식 설정 직전");
        newItem.transform.SetParent(content);

        //Debug.Log("컨텐츠의 자식 설정 완료,노래 시작하기 직전");
        if (id == musicData.playListId[0])
        {
            //Debug.Log("첫 곡 로드 다됨");
            musicManager.Play();
            //Debug.Log(musicController);
            musicController.playButtonSet();
            //Debug.Log("버튼셋 끝");
        }
    }

    // IEnumerator AddMusic(int id)
    // {
    //     //Debug.Log("뮤직 더하기 시작");
    //     using (var www = new WWW(musicData.url + id))
    //     {
    //         //Debug.Log("www 호출 전");
    //         yield return www;

    //         //Debug.Log("불러오기 완료");
    //         AudioClip clip = (AudioClip)www.GetAudioClip(false, true, AudioType.MPEG);
    //         musicData.playListId.Add(id);
    //         musicManager.AddToPlayList(clip);
    //         //Debug.Log("프리팹 생성 직전");
    //         GameObject newItem = Instantiate(playListItemPrefab);
    //         //Debug.Log("변수들 할당 직전");
    //         PlayListItem playListItem = newItem.GetComponent<PlayListItem>();
    //         playListItem.musicManager = musicManager;
    //         playListItem.id = id;
    //         MusicItem musicItem = musicData.getMusicInfo(id);
    //         string titleAndSinger = musicItem.title + "-" + musicItem.artist;
    //         newItem.transform.GetChild(0).GetComponent<TMP_Text>().text = titleAndSinger;
    //         //Debug.Log("자식 설정 직전");
    //         newItem.transform.SetParent(content);

    //         Debug.Log("컨텐츠의 자식 설정 완료,노래 시작하기 직전");
    //         if (id == musicData.playListId[0])
    //         {
    //             //Debug.Log("첫 곡 로드 다됨");
    //             musicManager.Play();
    //             //Debug.Log(musicController);
    //             musicController.playButtonSet();
    //             Debug.Log("버튼셋 끝");
    //         }
    //     }
    // }

    public void DeleteToPlayList(int index)
    {
        int nowTrack = musicManager.CurrentTrackNumber();
        musicData.playListId.RemoveAt(index);
        musicManager.RemoveFromPlayList(index);

        // if(nowTrack>=musicManager.GetPlayListLength()){
        //     Debug.Log("Call Stop?");
        //     Stop();
        //     musicController.playButtonSet();
        // }
        // else if (nowTrack>=index){//영향을 받았을 클립이므로 rewind
        //     musicManager.RewindClip();
        // }
        DestroyImmediate(content.GetChild(index).gameObject);
    }
    public List<int> getPlayListId()
    {
        return musicData.playListId;
    }

    public void LoadPlayList()
    {
        ServerConnector.Instance.GetPlayList(InteriorManager.Instance.GetRoomId(), SetPlayList);
    }

    public void SavePlayList()
    {
        List<int> playlist = new List<int>();
        foreach (int id in musicData.playListId)
        {
            playlist.Add(id);
        }
        //Debug.Log(playlist);
        PlayListSend playListSend = new PlayListSend(playlist);
        //Debug.Log(JsonUtility.ToJson(playListSend));
        ServerConnector.Instance.SavePlayList(InteriorManager.Instance.GetRoomId(), JsonUtility.ToJson(playListSend));
    }

    public void SetPlayList(string json)
    {
        //Debug.Log(content.childCount);
        Stop();
        if (content.childCount > 0)
        {
            int count = content.childCount;
            for (int i = 0; i < count; i++)
            {
                DeleteToPlayList(0);
            }
            
        }

        _playList = JsonUtility.FromJson<PlayList>(json);
        //Debug.Log(_playList.playlist);
        if (_playList.GetCount() == 0) return;
        StartCoroutine(SetPlayListPriority(_playList));
        // for (int i = 0; i < _playList.GetCount(); i++)
        // {
        //    AddToPlayList(int.Parse(_playList.playlist[i].id));
           
        // }
    }

    IEnumerator SetPlayListPriority(PlayList playList){
        for (int i = 0; i < playList.GetCount(); i++)
        {
           yield return StartCoroutine(AddMusic(int.Parse(playList.playlist[i].id))); //AddToPlayList(int.Parse(playList.playlist[i].id));
        }
    }

    [Serializable]
    public class PlayList
    {
        public List<MusicItem> playlist;

        public PlayList(List<MusicItem> playlist)
        {
            this.playlist = playlist;
        }

        public int GetCount()
        {
            return playlist.Count;
        }
    }

    [Serializable]
    public class PlayListSend
    {
        public List<int> playlist;

        public PlayListSend(List<int> playlist)
        {
            this.playlist = playlist;
        }
    }
}