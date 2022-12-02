
using System;
using Carum.Interior;
using UnityEngine;
using UnityEngine.Rendering;

namespace Carum.Background
{
    public class BackgroundController : MonoBehaviour
    {
        public static BackgroundController Instance { get; private set; }
        
        public BackgroundSettings[] backgroundData;

        private void Awake()
        {
            if (Instance != null || Instance == this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        public void ChangeBackground(int idx)
        {
            BackgroundSettings data;
            try
            {
                data = backgroundData[idx];
            }
            catch (Exception e)
            {
                Debug.Log("배경 데이터 인덱스 벗어남");
                return;
            }
            Camera mainCam = Camera.main;
            if (mainCam)
                mainCam.backgroundColor = data.backgroundColor;
            RenderSettings.ambientSkyColor = data.skyColor;
            RenderSettings.ambientEquatorColor = data.equatorColor;
            RenderSettings.ambientGroundColor = data.groundColor;
        }
    }
}