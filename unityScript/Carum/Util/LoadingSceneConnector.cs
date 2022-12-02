using System.Runtime.InteropServices;
using Carum.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Carum.Util
{
    public class LoadingSceneConnector : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void ReactCall2(string reactFunctionName);

        private float _requestTime = 2f;

        public float requestInterval = 2f;

        // Start is called before the first frame update
        void Start()
        {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
            CheckLogin();
#endif
            _requestTime = requestInterval;
        }

        private void CheckLogin()
        {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
            ReactCall2("checkLogin");
            UnityEngine.WebGLInput.captureAllKeyboardInput = false;

#endif
#if UNITY_EDITOR == true
            MoveScene("SceneD");
#endif
        }

        // Update is called once per frame
        void Update()
        {
            if (_requestTime < 0)
            {
                _requestTime = requestInterval;
                CheckLogin();
            }

            _requestTime -= Time.deltaTime;
        }

        public void MoveScene(string sceneName)
        {
            MySceneManager.Instance.LoadScene(sceneName);
        }
    }
}