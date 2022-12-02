
using UnityEngine;
using TMPro;
public class MusicListManager : MonoBehaviour
{
    public GameObject musicManagerObject;
    public GameObject musicListPrefab;

    public PlayListManager playListManager;
    HajiyevMusicManager musicManager;
    
    Transform content;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        musicManager=musicManagerObject.GetComponent<HajiyevMusicManager>();
        content=gameObject.transform.GetChild(0).GetChild(0).GetChild(0);
        audioSource =transform.GetComponent<AudioSource>();
        SetMusicList();
    }

    public void SetMusicList(){
        foreach(MusicItem item in MusicData.Instance.musicInfos){
            GameObject newItem=Instantiate(musicListPrefab);
            //스크립트 필요 변수 넣기
            MusicContent musicContent=newItem.GetComponent<MusicContent>();
            musicContent.audioSource=audioSource;
            //musicContent.music= musicData.getMusic(int.Parse(item.id));
            musicContent.musicManager=musicManager;
            musicContent.id=int.Parse(item.id);
            musicContent.playListManager=playListManager;

            //제목&가수 설정
            string titleAndSinger=(item.title+"-"+item.artist);
            newItem.transform.GetChild(0).GetComponent<TMP_Text>().text = titleAndSinger;

            //부모 설정
            newItem.transform.SetParent(content);
        }
        
    }
}
