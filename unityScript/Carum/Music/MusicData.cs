using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MusicData : MonoBehaviour
{
    public static MusicData Instance {get; private set;}
    //public List<AudioClip> musicDict = new List<AudioClip>();
    //public AudioClip clip;
    public bool isLoading;
    [SerializeField] HajiyevMusicManager musicManager;
    [SerializeField] PlayListManager playListManager;
    [SerializeField] MusicListManager musicListManager;
    public string url="https://k7a101.p.ssafy.io/api/music/file/";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            GameObject.Destroy(this.gameObject);
        }
        Instance = this;
    }
    // IEnumerator Start()
    // {
    //     for (int i = 1; i <= musicInfos.Count; i++)
    //     {
    //         //Debug.Log(url);
    //         using (var www = new WWW(url+i))
    //         {
    //             yield return www;
    //             musicDict.Add(www.GetAudioClip(false, true,AudioType.MPEG));
    //         }
            
    //     }
    //     //Debug.Log("musicDict : "+musicDict.Count);
    //     musicListManager.SetMusicList();

    // }
    public List<MusicItem> musicInfos = new List<MusicItem>(){
        new MusicItem("1","(여자)아이들","ANGRY","..","TOMBOY"),
        new MusicItem("2","BLACKPINK","ANGRY","..","뚜두뚜두"),
        new MusicItem("3","GAYLE","ANGRY","..","abcdefu"),
        new MusicItem("4","IMAGINEDRAGONS","ANGRY","..","Enemy"),
        new MusicItem("5","Taylor Swift","ANGRY","..","Bad Blood"),
        new MusicItem("6","Winona Oak","ANGRY","..","Floor 555"),
        new MusicItem("7","XXXTENTACION","ANGRY","..","Enemy"),
        new MusicItem("8","버즈 (Buzz)","ANGRY","..","1st"),
        new MusicItem("9","아이유 (IU)","ANGRY","..","Mordern Times"),
        new MusicItem("10","AJR","HAPPY","..","World's Smallest Violin"),
        new MusicItem("11","Charlie Puth","HAPPY","..","Left Right Left"),
        new MusicItem("12","DAY6(데이식스)","HAPPY","..","한 페이지가 될 수 있게"),
        new MusicItem("13","Guns N' Roses","HAPPY","..","Sweet Child O' Mine (영화 '토르) "),
        new MusicItem("14","Lizzo","HAPPY","..","Good as Hell (Feat. Ariana Grande)"),
        new MusicItem("15","Lizzo","HAPPY","..","Juice"),
        new MusicItem("16","Ljones","HAPPY","..","Cafe"),
        new MusicItem("17","NewJeans","HAPPY","..","Hype boy"),
        new MusicItem("18","Oneul","HAPPY","..","봄을 피워내는 요정 친구들"),
        new MusicItem("19","Oneul","HAPPY","..","집에서 마시는 커피도 맛있다"),
        new MusicItem("20","Oneul","HAPPY","..","한여름밤의 산책"),
        new MusicItem("21","마미손","HAPPY","..","사랑은 (Feat. 원슈타인)"),
        new MusicItem("22","박효신","HAPPY","..","Gift"),
        new MusicItem("23","백예린","HAPPY","..","Antifreeze"),
        new MusicItem("24","아이유","HAPPY","..","unlucky"),
        new MusicItem("25","윤하","HAPPY","..","살별"),
        new MusicItem("26","Anno Domini Beats","PEACE","..","Sunny Days "),
        new MusicItem("27","Coldplay","PEACE","..","Amazing Day"),
        new MusicItem("28","Em Beihold","PEACE","..","Numb Little Bug"),
        new MusicItem("29","Jack Dean","PEACE","..","I Wish"),
        new MusicItem("30","Laura Shigihara","PEACE","..","Everything's Alright"),
        new MusicItem("31","Laura Shigihara","PEACE","..","To the Moon - Main Theme"),
        new MusicItem("32","Lesfm","PEACE","..","just relax"),
        new MusicItem("33","Oasis","PEACE","..","Champagne Supernova"),
        new MusicItem("34","pATCHES","PEACE","..","Lulu Is the Cat I Like Best"),
        new MusicItem("35","slr","PEACE","..","Cook"),
        new MusicItem("36","Tai Verdas","PEACE","..","LAst dAy oN EaRTh"),
        new MusicItem("37","Tom Misch","PEACE","..","It Runs Through Me (Feat. De La Soul)"),
        new MusicItem("38","Various Artists","PEACE","..","생명의 이름 (いのちの名前 - Inochino Namae)"),
        new MusicItem("39","가을방학","PEACE","..","낮잠열차"),
        new MusicItem("40","가을방학","PEACE","..","루프탑"),
        new MusicItem("41","빌리어코스티","PEACE","..","소란했던 시절에"),
        new MusicItem("42","스텔라장","PEACE","..","Stairs"),
        new MusicItem("43","아이유","PEACE","..","너의 의미 (Feat. 김창완)"),
        new MusicItem("44","윤하","PEACE","..","사건의 지평선"),
        new MusicItem("45","이윤지","PEACE","..","우리 식구"),
        new MusicItem("46","5 Seconds Of Summer","SAD","..","Ghost Of You"),
        new MusicItem("47","Adele","SAD","..","Hello"),
        new MusicItem("48","Conan Gray","SAD","..","Memories"),
        new MusicItem("49","Ed Sheeran","SAD","..","Happier"),
        new MusicItem("50","Jeremy Zucker","SAD","..","Scared"),
        new MusicItem("51","potsu","SAD","..","Lovesick"),
        new MusicItem("52","The Script","SAD","..","Six Degrees of Separation"),
        new MusicItem("53","도민","SAD","..","동암역 2번출구 (Feat. 김무성 & 성지영)"),
        new MusicItem("54","아이유","SAD","..","이름에게"),
        new MusicItem("55","카더가든","SAD","..","가까운 듯 먼 그대여"),
        new MusicItem("56","카더가든","SAD","..","그대 나를 일으켜주면"),
        new MusicItem("57","G-DRAGON","SURPRISE","..","어쩌란 말이냐"),
        new MusicItem("58","Glass Animals","SURPRISE","..","Heat Waves"),
        new MusicItem("59","Kanye West","SURPRISE","..","Hurricane"),
        new MusicItem("60","Sam Ryder","SURPRISE","..","Space Man"),
        new MusicItem("61","Sia","SURPRISE","..","The Greatest (Feat. Kendrick Lamar)"),
        new MusicItem("62","Taylor Swift","SURPRISE","..","You Need To Calm Down"),
        new MusicItem("63","마크툽 (Maktub) & 이라온","SURPRISE","..","별을 담은 시 (Ode To The Stars)"),
        new MusicItem("64","심규선 (Lucia)","SURPRISE","..","달과 6펜스"),
        new MusicItem("65","윤하 (YOUNHA)","SURPRISE","..","하나의 달"),
        new MusicItem("66","이루마","SURPRISE","..","MAY Be"),
        new MusicItem("67","하진 (HAJIN)","SURPRISE","..","We All Lie"),
        new MusicItem("68","Isak Danielson","WORRY","..","Let Somebody Go"),
        new MusicItem("69","Lauv","WORRY","..","Modern Loneliness"),
        new MusicItem("70","St. Vincent","WORRY","..","Happy Birthday, Johnny"),
        new MusicItem("71","김연우","WORRY","..","이미 넌 고마운 사람"),
        new MusicItem("72","김윤아","WORRY","..","Going Home"),
        new MusicItem("73","심규선 & 에피톤 프로젝트","WORRY","..","부디"),
        new MusicItem("74","심규선 & 에피톤 프로젝트","WORRY","..","안녕, 안녕"),
        new MusicItem("75","쏜애플 (THORNAPPLE)","WORRY","..","로마네스크"),
        new MusicItem("76","어쿠루브","WORRY","..","내게 기대"),
        new MusicItem("77","이하이","WORRY","..","한숨"),
        new MusicItem("78","폴킴","WORRY","..","비"),

    };

    [SerializeField] public List<int> playListId = new List<int>();

    public void setPlayListId()
    {
        playListId = playListManager.getPlayListId();
    }
    public void getMusic(int id,AudioClip clip)
    {
        // if (musicDict.Capacity >= id && id >= 0)
        //     return musicDict[id - 1];
        // else
        //     return null;
        isLoading=true;
        StartCoroutine(getMusicFromApi(id,clip));
        
        
    }

    IEnumerator getMusicFromApi(int id,AudioClip clip){
        Debug.Log(url+id);
        using (var www = new WWW(url+id))
        {
            yield return www;
            clip= (AudioClip) www.GetAudioClip(false, true,AudioType.MPEG);
            isLoading=false;
            Debug.Log("여기가 끝");
        }
    }
    public MusicItem getMusicInfo(int id)
    {
        if (musicInfos.Capacity >= id && id >= 0)
            return musicInfos[id - 1];
        else
            return null;
    }
}
