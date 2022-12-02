using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Carum.UI
{
    public class MySceneManager : MonoBehaviour
    {
        public static MySceneManager Instance { get; private set; }
        [SerializeField] private SceneCurtainScript sceneCurtainScript;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        public void LoadScene(string sceneName)
        {
            // ReSharper disable once Unity.NoNullPropagation
            sceneCurtainScript?.BlackSceneAnimation();
            // ReSharper disable once HeapView.ObjectAllocation
            StartCoroutine(LoadSceneAfter(0.3f, sceneName));
            
        }

        private IEnumerator LoadSceneAfter(float time,string sceneName)
        {
            yield return new WaitForSeconds(time);
            SceneManager.LoadScene(sceneName);
        }

        public void SetSceneCurtain(bool activate)
        {
            if (activate)
                sceneCurtainScript.BlackSceneAnimation();
            else
            {
                sceneCurtainScript.WhiteSceneAnimation();
            }
        }
    }
}