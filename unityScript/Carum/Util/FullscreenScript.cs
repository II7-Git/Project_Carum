using UnityEngine;
using ScreenOrientation = MarksAssets.ScreenOrientationWebGL.ScreenOrientationWebGL.ScreenOrientation;
namespace Carum.Util
{
    public class FullscreenScript : MonoBehaviour
    {

        public void ToggleFullScreen()
        {
            if(!Screen.fullScreen){
                Debug.Log("풀스크린으로");
                Screen.fullScreen=true;
                Screen.orientation = (UnityEngine.ScreenOrientation)ScreenOrientation.LandscapeLeft;
            }
            else{
                Debug.Log("풀스크린 탈출");
                Screen.fullScreen=false;
                Screen.orientation = (UnityEngine.ScreenOrientation)ScreenOrientation.LandscapeRight;
            }
        }
    }
}
