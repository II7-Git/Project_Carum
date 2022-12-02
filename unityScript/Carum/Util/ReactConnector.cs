using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Carum.Interior;
using Carum.Pets.Text;
using Carum.UI;
using Carum.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReactConnector : MonoBehaviour
{
    // singleton
    public static ReactConnector Instance { get; private set; }
    public int loadTemplate = 0;

    private void Awake()
    {
        if (Instance != null || Instance == this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public string debugToken;

    [DllImport("__Internal")]
    private static extern void ReactCall(string reactFunctionName, string param);

    [DllImport("__Internal")]
    private static extern void ReactCall2(string reactFunctionName);

    // Start is called before the first frame update
    void Start()
    {
        RequestTokenToReact();
    }

    public void RequestTokenToReact()
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        UnityEngine.WebGLInput.captureAllKeyboardInput = false;
        Debug.Log("토큰주세용");
        ReactCall2("sendTokenToUnity");
#endif
#if UNITY_EDITOR == true
        if (loadTemplate == 0)
        {
            string json =
                "{\"mainRoomId\":5,\"token\":{\"accessToken\":\"" + debugToken +
                "\",\"refreshToken\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpYXQiOjE2Njc0NTUzNTQsImV4cCI6MTY2ODA2MDE1NH0.-fY_k4JjDkIGSYLQpJUPDeQ7685ZWqE6b8C2ZXrZa4w\"}," +
                "\"petType\":\"DINO\",\"dailyFace\":\"ANGRY\",\"dailyColor\":8,\"todayDiary\":false}";
            StartUnity(json);
        }
        else
        {   
            string json =
                "{\"mainRoomId\":"+loadTemplate+",\"token\":{\"accessToken\":\"" + debugToken +
                "\",\"refreshToken\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpYXQiOjE2Njc0NTUzNTQsImV4cCI6MTY2ODA2MDE1NH0.-fY_k4JjDkIGSYLQpJUPDeQ7685ZWqE6b8C2ZXrZa4w\"}," +
                "\"petType\":\"DINO\",\"dailyFace\":\"ANGRY\",\"dailyColor\":8,\"todayDiary\":false}";
            LoadTemplate(json);
        }

#endif
    }

    /// <summary>
    /// 유니티 씬 초기 로드함수
    /// 토큰 설정 및 메인 룸 로드
    /// </summary>
    /// <param name="json">로그인 정보가 담긴 토큰</param>
    public void StartUnity(string json)
    {
        // Debug.Log("StartUnity:" + json);
        RequestDto.StartDto dto = JsonUtility.FromJson<RequestDto.StartDto>(json);
        SetTokens(dto.token);
        InteriorManager.Instance?.LoadRoom(dto.mainRoomId);
        // 펫 생성
        if (!dto.petType.Equals("NONE"))
            PetManager.Instance?.InitPet(dto.dailyColor.ToString(), dto.dailyFace, dto.petType);
    }

    private void LoadTemplate(string json)
    {
        RequestDto.StartDto dto = JsonUtility.FromJson<RequestDto.StartDto>(json);
        SetTokens(dto.token);
        InteriorManager.Instance?.LoadTemplate(dto.mainRoomId);

    }
    private void SetTokens(RequestDto.Token token)
    {
        ServerConnector.Instance?.SetToken(token);
    }

    // 임시메서드
    public void PetCloseUp()
    {
        // pettext 찾음 아무튼
        // PetText petText = PetText.Instance;
        // petText.DoConversation("다이어리 쓰기","ANGRY");
        PetManager.Instance?.CloseUp();
    }

    // 임시 메서드
    public void PetEndCloseUp()
    {
        // ConversationManager.Instance.EndConversation();
        // CameraController.Instance.cameraMode = 0;
        PetText.Instance.EndConversation();
        PetManager.Instance?.CloseOut();
    }

    public void PetConversation(string json)
    {
        RequestDto.PetConversationDto dto = JsonUtility.FromJson<RequestDto.PetConversationDto>(json);

        PetManager.Instance.DoConversation(dto.text, dto.emotion);
    }

    public void PetCreate(string json)
    {
        RequestDto.StartDto dto = JsonUtility.FromJson<RequestDto.StartDto>(json);
        // 펫 생성
        if (!dto.petType.Equals("NONE"))
            PetManager.Instance?.InitPet(dto.dailyColor.ToString(), dto.dailyFace, dto.petType);
    }

    public void DiaryWrite()
    {
        // Debug.Log("다이어리 작성 들었음");
        // 오늘의 펫 정보 요청
        // 해당 펫 정보로 바꾸기
        ServerConnector.Instance.GetDailyPet(PetManager.Instance.SetVisual);
    }

    public void ChangeRoom(string json)
    {
        // Debug.Log("방 교체 신호");
        RequestDto.RoomDto dto = JsonUtility.FromJson<RequestDto.RoomDto>(json);
        InteriorManager.Instance?.LoadRoom(dto.roomId);
    }

    public void Logout()
    {
        MySceneManager.Instance.LoadScene("LoadingScene");
    }
}