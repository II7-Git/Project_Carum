using System;
using Carum.Interior;
using TMPro;
using UnityEngine;

// ReSharper disable Unity.NoNullPropagation

namespace Carum.UI
{
    public class UIController : MonoBehaviour
    {
        // singleton pattern
        private static UIController _instance = null;
        
        public static UIController GetInstance()
        {
            return _instance;
        }
        
        public GameObject functionBtnPanel;
        public GameObject putModePanel;
        public GameObject settingPanel;
        
        // window size
        [Header("유니티 뷰모드 전환 크기값")]
        [SerializeField]
        private int widthThreshold = 500;
        [SerializeField]
        private int heightThreshold = 400;
        private int _width;
        private int _height;
        [SerializeField] private GameObject exitFullscreenBtn;

        [Header("조작모드 / 뷰모드 오브젝트")]
        [SerializeField]
        private GameObject controlModeUI;
        [SerializeField] private GameObject viewModeUI;
        [SerializeField] private GameObject activeUI;
        private bool _isControlMode = false;
        private Camera _camera;

        private float _timer = 0;
        public float interval = 2f;

        void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(this);

            _width = Screen.width;
            _height = Screen.height;

        }
        private void Start()
        {
            functionBtnPanel?.SetActive(false);
            // UpdateViewMode();
            _camera = Camera.main;
            
        }

        private void LateUpdate()
        {
            // 창 크기 감지
            // if (_width != Screen.width || _height != Screen.height)
            // {
            //     Debug.Log("창 크기 변경!");
            //     _width = Screen.width;
            //     _height = Screen.height;
            //     Debug.Log("width:"+_width+","+"height:"+_height);
            //
            //     UpdateViewMode();
            //
            // }
            
            if (_isControlMode && Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Debug.Log("터치인식");
                SwitchToControlMode(false);
            } else if (!_isControlMode && Input.GetMouseButtonDown(0) && Input.touchCount == 0)
            {
                Debug.Log("마우스인식");
                SwitchToControlMode(true);
            }
            if (IsMouseIn() || Input.touchCount >= 1)
            {
                _timer = interval;
            }

            _timer -= Time.deltaTime;
            ActiveUI(_timer > 0);
        }

        private void ActiveUI(bool active)
        {
            activeUI.SetActive(active);
        }

        private void UpdateViewMode()
        {
            // Debug.Log("모드:"+!IsControllableSize());
            SwitchToControlMode(!IsControllableSize());
        }
        private void SwitchToControlMode(bool controlMode)
        {
            _isControlMode = controlMode;
            // Debug.Log("controlMode:"+_isControlMode);
            controlModeUI.SetActive(_isControlMode);
            viewModeUI.SetActive(!_isControlMode);
        }

        public bool IsControllableSize()
        {
            return _width >= widthThreshold && _height >= heightThreshold;
        }
        /// <summary>
        /// 기능 UI 켜고 끄기
        /// </summary>
        public void ToggleFunctionPanel()
        {
            functionBtnPanel?.SetActive(!functionBtnPanel.activeSelf);
        }
        /// <summary>
        /// 가구배치모드 진입
        /// 
        /// </summary>
        public void EnterPutMode()
        {
            InteriorManager.Instance.ChangeMode(PutMode.Pick);
            SetActivePutModePanel(true);
        }
        /// <summary>
        /// 가구배치모드 종료
        /// </summary>
        public void ExitPutMode()
        {
            InteriorManager.Instance.ChangeMode(PutMode.None);
            SetActivePutModePanel(false);
        }
        /// <summary>
        /// 배치모드 UI 켜고끄기
        /// </summary>
        /// <param name="active">표시 여부</param>
        public void SetActivePutModePanel(bool active)
        {
            functionBtnPanel?.SetActive(!active);
            putModePanel?.SetActive(active);
        }

        public void ToggleSettingPanel()
        {
            settingPanel?.SetActive(!settingPanel.activeSelf);
        }

        public void SetFullscreen(bool fullscreen)
        {
            Screen.fullScreen=fullscreen;
            exitFullscreenBtn.SetActive(fullscreen);
            // SwitchToControlMode(fullscreen);
            if (fullscreen)
            {
                controlModeUI.SetActive(true);
                viewModeUI.SetActive(false);
            }
            else
            {
                SwitchToControlMode(_isControlMode);
            }
        }

        public bool IsMouseIn()
        {
            var view = _camera.ScreenToViewportPoint(Input.mousePosition);
            var isOutside = view.x is < 0 or > 1 || view.y is < 0 or > 1;
            return !isOutside;
        }
    }
}