using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class MusicController : MonoBehaviour
{
    public static MusicController Instance { get; private set; }
    public Sprite startIcon;
    public Sprite pauseIcon;
    public HajiyevMusicManager musicManager;

    private MusicData musicData;
    private TMP_Text tMP_Text;
    private int trackNum = 1;
    private int musicNum = 0;
    [SerializeField] AudioSource musicListAudio;
    private string title;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            GameObject.Destroy(this.gameObject);
        }
        Instance = this;
    }
    private void Start()
    {
        tMP_Text = transform.GetChild(3).GetComponent<TMP_Text>();
        musicData = MusicData.Instance;
        playButtonSet();
        //StartCoroutine(CheckTitle());
    }

    private void Update()
    {


        // if(musicManager.CurrentTrackNumber()!=trackNum){
        //     trackNum=musicManager.CurrentTrackNumber();
        //     Debug.Log("Track:"+trackNum);
        //     if(musicData!=null && musicData.playListId.Count!=0 && trackNum!= -1){
        //         MusicItem musicItem=musicData.getMusicInfo( musicData.playListId[trackNum]);
        //         string title=musicItem.title+"-"+musicItem.artist;
        //         tMP_Text.text=title;
        //     }
        // }
        CheckTitle();
    }

    public void CheckTitle()
    {
        trackNum = musicManager.CurrentTrackNumber();

        if (musicData != null && musicData.playListId.Count != 0)
        {
            if (trackNum < musicData.playListId.Count)
            {
                if (musicData.playListId[trackNum] != musicNum)
                {
                    musicNum = musicData.playListId[trackNum];
                    MusicItem musicItem = musicData.getMusicInfo(musicNum);
                    title = musicItem.title + "-" + musicItem.artist;
                    tMP_Text.text = title;
                    // if(musicManager.IsPlaying()){
                    //     musicManager.Pause();
                    //     musicManager.Play();
                    // }

                }
            }
        }else{
            tMP_Text.text = "";
        }
    }
    // IEnumerator CheckTitle()
    // {
    //     trackNum = musicManager.CurrentTrackNumber();

    //     if (musicData != null && musicData.playListId.Count != 0)
    //     {
    //         if (trackNum < musicData.playListId.Count)
    //         {
    //             if (musicData.playListId[trackNum] != musicNum)
    //             {
    //                 musicNum = musicData.playListId[trackNum];
    //                 MusicItem musicItem = musicData.getMusicInfo(musicNum);
    //                 title = musicItem.title + "-" + musicItem.artist;
    //                 tMP_Text.text = title;
    //                 // if(musicManager.IsPlaying()){
    //                 //     musicManager.Pause();
    //                 //     musicManager.Play();
    //                 // }

    //             }
    //         }
    //     }else{
    //         tMP_Text.text = "";
    //     }
    //     yield return new WaitForSeconds(0.5f);
    //     StartCoroutine(CheckTitle());
    // }
    public void playButtonSet()
    {

        if (musicManager.IsPlaying())
        {
            if (musicListAudio.isPlaying) musicListAudio.Pause();
            transform.GetChild(0).GetComponent<Image>().sprite = startIcon;
        }
        else
        {
            transform.GetChild(0).GetComponent<Image>().sprite = pauseIcon;
        }
    }

    public void PlayPauseToggle()
    {
        //음악이 아예 없다면 0으로 초기화
        AudioClip clip = musicManager.GetPlayListClip(musicManager.CurrentTrackNumber());
        if (!clip)
        {
            musicManager.Stop();
            transform.GetChild(0).GetComponent<Image>().sprite = pauseIcon;
            return;

        }else if(clip!=musicManager.gameObject.GetComponent<AudioSource>().clip){
            musicManager.RewindClip();
        }
        musicManager.PlayPauseToggle();

        if (musicManager.IsPlaying())
        {
            if (musicListAudio.isPlaying) musicListAudio.Pause();
            transform.GetChild(0).GetComponent<Image>().sprite = startIcon;
        }
        else
        {
            transform.GetChild(0).GetComponent<Image>().sprite = pauseIcon;
        }
    }
}
