
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
public class PlayListItem : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler, IBeginDragHandler ,IPointerClickHandler//,IBeginDragHandler, IEndDragHandler, IDropHandler
{
    public HajiyevMusicManager musicManager;
    public int id;
    public RectTransform currentTransform;
    private GameObject mainContent;
    private Vector3 currentPossition;
    private int preIndex;
    private int totalChild;
    private MusicData musicData;

    private void Start() {
        musicData = MusicData.Instance;
        transform.localScale = Vector3.one;
        //Debug.Log("내크기:"+transform.localScale);
    }
    public void playListDelete()
    {
        StartCoroutine(PlayListDestroy());
    }

    private IEnumerator PlayListDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        int index = transform.GetSiblingIndex();
        musicManager.RemoveFromPlayList(index);
        Destroy(transform.gameObject);
        musicData.playListId.RemoveAt(index);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        currentPossition = currentTransform.position;
        mainContent = currentTransform.parent.gameObject;
        totalChild = mainContent.transform.childCount;
        preIndex=transform.GetSiblingIndex();
    }

    public void OnPointerClick(PointerEventData eventData){
        //Debug.Log("click");
        if(preIndex!=transform.GetSiblingIndex()) return;
        if(preIndex==musicManager.CurrentTrackNumber()){//같은 노래면 일시 정지하던지 start 하던지
            if(musicManager.IsPlaying())musicManager.Pause();
            else musicManager.Play();
            MusicController.Instance.playButtonSet();
            //transform.parent.parent.parent.parent.parent.GetChild(2).GetComponent<MusicController>().playButtonSet();
            return;
        }
        if(musicManager.IsPlaying())
                musicManager.Stop();
            musicManager.SetTrack(preIndex);
            musicManager.Play();

        MusicController.Instance.playButtonSet();
        //transform.parent.parent.parent.parent.parent.GetChild(2).GetComponent<MusicController>().playButtonSet();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        // currentPossition = currentTransform.position;
        // mainContent = currentTransform.parent.gameObject;
        // totalChild = mainContent.transform.childCount;
        //preIndex=transform.GetSiblingIndex();
    }
    public void OnDrag(PointerEventData eventData)
    {
        currentTransform.position =
            new Vector3(currentTransform.position.x, eventData.position.y, currentTransform.position.z);

        for (int i = 0; i < totalChild; i++)
        {
            if (i != currentTransform.GetSiblingIndex())
            {
                Transform otherTransform = mainContent.transform.GetChild(i);
                int distance = (int)Vector3.Distance(currentTransform.position,
                    otherTransform.position);
                if (distance <= 10)
                {
                    Vector3 otherTransformOldPosition = otherTransform.position;
                    otherTransform.position = new Vector3(otherTransform.position.x, currentPossition.y,
                        otherTransform.position.z);
                    currentTransform.position = new Vector3(currentTransform.position.x, otherTransformOldPosition.y,
                        currentTransform.position.z);
                    currentTransform.SetSiblingIndex(otherTransform.GetSiblingIndex());
                    currentPossition = currentTransform.position;
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("clickDo");
        //  if(musicManager.IsPlaying())
        //         musicManager.Stop();
        //     musicManager.SetTrack(preIndex);
        //     musicManager.Play();
        //currentTransform.position = currentPossition;
        //Debug.Log("is it work?");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        currentTransform.position = currentPossition;

        
        //preIndex
        AudioClip temp= musicManager.GetPlayListClip(preIndex);
        int newIndex=transform.GetSiblingIndex();
        //Debug.Log(preIndex+"  fdf  "+newIndex);
        if(preIndex<newIndex){//밑으로 이동
            musicManager.playList.Insert(newIndex+1,temp);
            musicData.playListId.Insert(newIndex+1,musicData.playListId[preIndex]);
            musicManager.RemoveFromPlayList(preIndex);
            musicData.playListId.RemoveAt(preIndex);
        }else if(preIndex>newIndex){//위로 이동//삭제시 +1 필요
            
            musicManager.playList.Insert(newIndex,temp);
            musicData.playListId.Insert(newIndex,musicData.playListId[preIndex]);
            musicManager.RemoveFromPlayList(preIndex+1);
            musicData.playListId.RemoveAt(preIndex+1);
        }
    }

}
