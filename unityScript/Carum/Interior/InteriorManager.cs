using System;
using System.Collections;
using System.Collections.Generic;
using Carum.Background;
using Carum.Room;
using Carum.UI;
using Carum.Util;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ReSharper disable Unity.NoNullPropagation

// ReSharper disable HeapView.ObjectAllocation

namespace Carum.Interior
{
    public class InteriorManager : MonoBehaviour
    {
        // singleton pattern
        public static InteriorManager Instance { get; private set; }

        // 방 기본 설정
        private int _roomId;
        private int _frame;
        private int _background;

        // 건드린 가구들
        private HashSet<Interior> _interiorTrackings = new();

        // NavMesh
        private NavMeshSurface _navMeshSurface;

        // 현재 배치중인 인테리어
        private Interior _pickedInterior;
        public float sensitive = 5f;
        public PutMode _putMode { get; private set; }
        private Transform baseTransform;
        private Transform interiorTransform;

        // 가구 배치 취소 시 원래 위치
        private Vector3 _originPosition;
        private Quaternion _originQuaternion;

        private Camera _camera;

        // UI
        [Header("평상시 UI")] public GameObject normalModeUI;
        public GameObject inventoryPanel;
        [Header("배치모드 UI")] public GameObject putModeUI;
        public GameObject pickedActionPanel;
        public GameObject pickedControlPanel;
        public float raycastDistance = 100f;
        [Header("가구 배치 UI")] public Image changeModeBtnImage;
        public Sprite raycastImage;
        public Sprite moveAxisImage;
        public GameObject rotationController;

        // raycast 레이어 마스크
        private LayerMask _layerMask;

        // inventory
        [Header("인벤토리")] public Inventory.InventoryController inventoryController;


        void Awake()
        {
            if (Instance != null || Instance == this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            _layerMask = LayerMask.GetMask("Interior");
            _camera = Camera.main;

            inventoryPanel.SetActive(false);
            putModeUI.SetActive(false);

            baseTransform = GameObject.FindWithTag("Base").transform;
            interiorTransform = GameObject.FindWithTag("Interior").transform;
        }

        void FixedUpdate()
        {
            if (_putMode == PutMode.Raycast &&
                ((Input.touchCount == 0 && !EventSystem.current.IsPointerOverGameObject()) ||
                 (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                ))
                DoInteriorAction();
        }

        void Update()
        {
            if (_putMode is PutMode.MoveAxis or PutMode.Rotate or PutMode.Pick &&
                ((Input.touchCount == 0 && !EventSystem.current.IsPointerOverGameObject()) ||
                 (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                ))
                DoInteriorAction();
        }


        /// <summary>
        /// 가구배치모드 진입.
        /// </summary>
        /// <param name="mode">진입하려는 모드.</param>
        private void EnterPutMode(PutMode mode)
        {
            _putMode = mode;
            // UI 활성화
            normalModeUI.SetActive(_putMode is PutMode.None or PutMode.Pick);
            putModeUI.SetActive(true);
            pickedActionPanel.SetActive(false);
            pickedControlPanel.SetActive(_putMode != PutMode.Pick);
            rotationController.SetActive(_putMode is PutMode.MoveAxis);
            // 선택한 가구 초기화
            if (_putMode is PutMode.Pick)
                SetPickedInterior(null);
            // 기본 UI 전환
            UIController.GetInstance()?.SetActivePutModePanel(true);

            // 배치 모드 스위칭 버튼 아이콘 변경
            if (_putMode == PutMode.Raycast)
                changeModeBtnImage.sprite = raycastImage;
            else if (_putMode == PutMode.MoveAxis)
                changeModeBtnImage.sprite = moveAxisImage;
        }

        /// <summary>
        /// 가구배치모드 종료
        /// </summary>
        public void ExitPutMode()
        {
            // UI 비활성화
            _putMode = PutMode.None;
            normalModeUI.SetActive(true);
            putModeUI.SetActive(false);
            SetPickedInterior(null);
            UIController.GetInstance()?.SetActivePutModePanel(false);
            SaveModification();
        }

        /// <summary>
        /// 배치모드에서 선택한 가구 설정.
        /// </summary>
        /// <param name="interior">선택할 가구.</param>
        public void SetPickedInterior(Interior interior)
        {
            // 이전에 선택했던 가구 비활성화
            if (_pickedInterior)
            {
                _pickedInterior.gameObject.layer = LayerMask.NameToLayer("Interior");
                _pickedInterior.SetOutline(false);
            }

            // 가구 선택
            if (!interior)
            {
                _pickedInterior = null;
            }
            else
            {
                _pickedInterior = interior;
                _pickedInterior.SetOutline(true);
                if (_pickedInterior.action != InteriorManageState.ADD)
                    _pickedInterior.action = InteriorManageState.MOD;
                var pickedInteriorTransform = _pickedInterior.transform;
                _originPosition = pickedInteriorTransform.position;
                _originQuaternion = pickedInteriorTransform.rotation;
                _pickedInterior.gameObject.layer = LayerMask.NameToLayer("PickedInterior");
            }

            // 가구선택 UI 활성화
            pickedActionPanel.SetActive(_pickedInterior);
        }

        /// <summary>
        /// 선택 가구한 가구의 배치 끝내기.
        /// </summary>
        /// <param name="save">현재 위치로 저장하거나 이동 전 위치로 되돌릴지 여부.</param>
        public void SaveInterior(bool save)
        {
            if (!_pickedInterior) return;
            if (!save)
            {
                var pickedInteriorTransform = _pickedInterior.transform;
                pickedInteriorTransform.position = _originPosition;
                pickedInteriorTransform.rotation = _originQuaternion;
            }

            SetPickedInterior(null);
            ChangeMode(PutMode.Pick);
            Bake();
        }

        /// <summary>
        /// 가구배치모드 터치 액션.
        /// 배치모드에 따라 조작이 바뀜.
        /// </summary>
        private void DoInteriorAction()
        {
            if (_putMode is PutMode.MoveAxis or PutMode.Rotate)
            {
                if (_putMode is PutMode.MoveAxis)
                {
                    int touchCount = Input.touchCount;
                    float xAxis = 0;
                    float yAxis = 0;
                    float zAxis = 0;
                    if (Input.GetMouseButton(0) && touchCount == 0)
                    {
                        xAxis = Input.GetAxis("Mouse X");
                        zAxis = Input.GetAxis("Mouse Y");
                    }

                    else if (Input.GetMouseButton(1) && touchCount == 0)
                    {
                        yAxis = Input.GetAxis("Mouse Y");
                    }
                    else if (touchCount == 1)
                    {
                        Touch touch = Input.GetTouch(0);
                        if (touch.phase == TouchPhase.Moved)
                        {
                            xAxis = -Input.touches[0].deltaPosition.x / 100;
                            zAxis = -Input.touches[0].deltaPosition.y / 100;
                        }
                    }
                    else if (touchCount >= 2)
                    {
                        Touch touch = Input.GetTouch(0);
                        if (touch.phase == TouchPhase.Moved)
                        {
                            yAxis = -Input.touches[0].deltaPosition.y / 100;
                        }
                    }

                    var moveVector = new Vector3(xAxis, yAxis, zAxis) * sensitive;
                    if (moveVector.magnitude != 0)
                        _pickedInterior.Move(moveVector, _camera.transform);
                }
                else if (_putMode is PutMode.Rotate)
                {
                    var xAxis = Input.GetAxis("Mouse X") * sensitive;
                    var yAxis = Input.GetAxis("Mouse Y") * sensitive;

                    if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
                    {
                        _pickedInterior.Rotate(new Vector3(0, 0, -xAxis));
                    }
                    else if (Input.GetMouseButton(0))
                    {
                        _pickedInterior.Rotate(new Vector3(0, xAxis, 0));
                    }
                    else if (Input.GetMouseButton(1))
                    {
                        _pickedInterior.Rotate(new Vector3(yAxis, 0, -xAxis));
                    }
                }
            }
            else if (_putMode is PutMode.Pick)
            {
                if (!Input.GetMouseButtonUp(0)) return;

                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, raycastDistance, _layerMask))
                {
                    Interior interior = hit.transform.gameObject.GetComponent<Interior>();
                    SetPickedInterior(interior);
                }
                else
                {
                    SetPickedInterior(null);
                }
            }
            else if (_putMode is PutMode.Raycast)
            {
                if (!Input.GetMouseButton(0)) return;

                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, raycastDistance, _layerMask))
                    // _pickedInterior?.Put(hit.point);
                    _pickedInterior?.PutFromRaycast(hit);
            }

            // // 터치 조작
            // else
            // {
            //     if (_putMode is PutMode.MoveAxis)
            //     {
            //         if (Input.touchCount == 1)
            //         {
            //             var xAxis = Input.touches[0].deltaPosition.x;
            //             var yAxis = Input.touches[0].deltaPosition.y;
            //             _pickedInterior.Move(new Vector3(xAxis * sensitive, 0, yAxis * sensitive));
            //         }
            //         else if (Input.touchCount == 2)
            //         {
            //             var yAxis = Input.GetAxis("Mouse Y");
            //             _pickedInterior.Move(new Vector3(0, yAxis * sensitive, 0));
            //         }
            //     }
            // }
        }

        /// <summary>
        /// UI를 통한 가구배치모드 변경
        /// </summary>
        /// <param name="mode">변경할 모드. Enum값와 일치하게 작성해야함.</param>
        public void ChangeMode(string mode)
        {
            PutMode putMode;
            try
            {
                putMode = (PutMode)Enum.Parse(typeof(PutMode), mode);
            }
            catch (Exception)
            {
                // Debug.Log("변경하려는 putMode 이름을 찾을 수 없음 : " + mode);
                return;
            }

            ChangeMode(putMode);
        }

        /// <summary>
        /// 스크립트를 통한 가구배치모드 변경
        /// </summary>
        /// <param name="mode">변경할 모드. Enum값와 일치하게 작성해야함.</param>
        public void ChangeMode(PutMode mode)
        {
            switch (mode)
            {
                case PutMode.None:
                    ExitPutMode();
                    break;
                default:
                    EnterPutMode(mode);
                    break;
            }
        }

        public void DeletePickedInterior()
        {
            if (!_pickedInterior) return;
            if (_pickedInterior.action is InteriorManageState.ADD)
                DeleteFromInteriorTracking(_pickedInterior);
            else
                _pickedInterior.action = InteriorManageState.DEL;

            _pickedInterior.gameObject.SetActive(false);
            SetPickedInterior(null);
            Bake();
        }

        /// <summary>
        /// 배치모드를 Raycast혹은 MoveAxis로 전환
        /// </summary>
        public void TogglePutMode()
        {
            if (_putMode is PutMode.Raycast)
                EnterPutMode(PutMode.MoveAxis);
            else if (_putMode is PutMode.MoveAxis)
                EnterPutMode(PutMode.Raycast);
        }


        /// <summary>
        /// 선택한 가구 수평으로회전
        /// </summary>
        /// <param name="angle">회전 각도</param>
        public void RotatePickedInteriorHorizon(float angle)
        {
            if (!_pickedInterior) return;
            _pickedInterior.Rotate(new Vector3(0, angle, 0));
        }

        /// <summary>
        ///  선택한 가구 수직으로 회전
        /// </summary>
        /// <param name="angle">회전 각도</param>
        public void RotatePickedInteriorVertical(float angle)
        {
            if (!_pickedInterior) return;
            _pickedInterior.Rotate(new Vector3(angle, 0, 0));
        }

        /// <summary>
        ///  선택한 가구 각도 초기화
        /// </summary>
        public void ResetPickedInteriorRotation()
        {
            if (!_pickedInterior) return;
            _pickedInterior.transform.rotation = Quaternion.Euler(0, 0, 0);
        }


        /// <summary>
        /// room id에 해당하는 방 불러오기
        /// </summary>
        /// <param name="roomId">불러오려는 방 id</param>
        public void LoadRoom(int roomId)
        {
            // Debug.Log(roomId + "번방 불러오기");
            _roomId = roomId;
            // 커튼치기
            MySceneManager.Instance.SetSceneCurtain(true);
            
            ServerConnector.Instance.GetRoom(_roomId, LoadRoomCallback);
            PlayListManager.Instance.LoadPlayList();
        }

        /// <summary>
        /// 템플릿 불러오기
        /// </summary>
        /// <param name="templateId">템플릿 id</param>
        public void LoadTemplate(int templateId)
        {
            _roomId = templateId;
            ServerConnector.Instance.GetTemplate(templateId, LoadRoomCallback);
        }

        public void Bake()
        {
            // Nav mesh 빌드
            _navMeshSurface.BuildNavMesh();
        }

        private void LoadRoomCallback(string json)
        {

            // 방 정보 파싱
            ResGetRoom roomDto = JsonUtility.FromJson<ResGetRoom>(json);
            // 프레임 로드
            LoadFrame(roomDto.frame);
            // 인테리어 로드
            LoadInteriors(roomDto);
            // 배경 로드
            LoadBackground(roomDto.background);

            // Debug.Log("방 불러오기 성공");
            MySceneManager.Instance.SetSceneCurtain(false);
        }

        /// <summary>
        /// 방 프레임 불러오기
        /// </summary>
        /// <param name="frame">프레임 인덱스</param>
        public GameObject LoadFrame(int frame)
        {
            // 방 정보 설정
            _frame = frame;
            // 기존 프레임 삭제
            Transform[] children = baseTransform.GetComponentsInChildren<Transform>();
            for (int i = 1; i < children.Length; i++)
            {
                Destroy(children[i].gameObject);
            }

            // 새로운 프레임 로드
            GameObject frameObject = Resources.Load<GameObject>("base/" + frame);
            GameObject instantiate = Instantiate(frameObject, baseTransform, true);
            _navMeshSurface = instantiate.GetComponent<NavMeshSurface>();
            Bake();
            return instantiate;
            // instantiate.transform.localScale = new Vector3(1.3f, 1, 1.3f);
        }

        public void LoadBackground(int background)
        {
            _background = background;
            BackgroundController.Instance.ChangeBackground(background);
        }

        private void LoadInteriors(ResGetRoom roomDto)
        {

            // 방 초기화
            InitRoom();
            // dto로 인테리어 인스턴스화
            foreach (InteriorDto dto in roomDto.interiorList)
            {
                // 에셋 로드
                LoadFurniture(dto);
            }

            Bake();
        }

        private void LoadFurniture(InteriorDto interiorDto)
        {
            GameObject obj = Resources.Load<GameObject>(interiorDto.GetResourcePath());
            if (!obj)
            {
                // Debug.Log("파일없는데?:" + interiorDto.GetResourcePath());
                return;
            }

            // GameObject furniture = Instantiate(opHandle.Result);
            // f
            // Interior interior = furniture.GetComponent<Interior>();
            Interior interior = Interior.Instantiate(obj);
            interior.transform.SetParent(interiorTransform);
            interior.Init(interiorDto);
            interior.SetOutline(false);
            AddToInteriorTracking(interior);
        }

        public void AddToInteriorTracking(Interior interior)
        {
            _interiorTrackings.Add(interior);
        }

        private void DeleteFromInteriorTracking(Interior interior)
        {
            _interiorTrackings.Remove(interior);
        }

        public void SaveModification()
        {
            ReqPutRoom putRoom = new();
            putRoom.frame = _frame;
            putRoom.background = _background;
            foreach (Interior interior in _interiorTrackings)
            {
                if (interior.action != InteriorManageState.NONE)
                    putRoom.interiorList.Add(interior.ToDto());
            }

            // Debug.Log(JsonUtility.ToJson(putRoom));
            ServerConnector.Instance.SaveRoom(_roomId, JsonUtility.ToJson(putRoom), InitTracking);
        }

        public void SaveTemplateModification()
        {
            ReqPutRoom putRoom = new();
            putRoom.frame = _frame;
            putRoom.background = _background;
            foreach (Interior interior in _interiorTrackings)
            {
                if (interior.action != InteriorManageState.NONE)
                    putRoom.interiorList.Add(interior.ToDto());
            }

            // Debug.Log(JsonUtility.ToJson(putRoom));
            ServerConnector.Instance.SaveTemplate(_roomId, JsonUtility.ToJson(putRoom), InitTracking);
        }

        public void InitTracking(string result)
        {
            // Debug.Log("INIT TRACKING");
            _interiorTrackings = new();
        }

        public int GetRoomId()
        {
            return _roomId;
        }

        public void InitRoom()
        {
            //기존 가구 삭제
            Transform[] children = interiorTransform.GetComponentsInChildren<Transform>();
            for (int i = 1; i < children.Length; i++)
            {
                children[i].gameObject.SetActive(false);
                Destroy(children[i].gameObject);
            }

            _interiorTrackings = new();
            Bake();
        }
    }

    public enum PutMode
    {
        None,
        Pick,
        Raycast,
        MoveAxis,
        Rotate
    }
}