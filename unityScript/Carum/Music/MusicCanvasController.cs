using UnityEngine;

public class MusicCanvasController : MonoBehaviour
{
    //[SerializeField] Canvas musicCanvas;

    public void CanvasExit(){
        gameObject.SetActive(false);
    }

    public void CanvasEnter(){
        gameObject.SetActive(true);
    }
}
