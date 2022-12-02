using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static Carum.Util.RequestDto;

// ReSharper disable HeapView.ObjectAllocation

namespace Carum.Util
{
    public class ServerConnector : MonoBehaviour
    {
        public static ServerConnector Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null || Instance == this)
                Destroy(gameObject);
            else
                Instance = this;
            
            #if UNITY_EDITOR == true
            if (connectToLocalDB)
                ServerURL = "http://localhosinvent:8080/api";
            #endif
        }

        public bool connectToLocalDB = false;
        // private string ServerURL = "http://localhost:8080/api";
        private string ServerURL = "https://k7a101.p.ssafy.io/api";

        [SerializeField] private Token _token;

        private IEnumerator GetRequest(string url, Action<string> callback)
        {
            UnityWebRequest uwr = UnityWebRequest.Get(url);
            if (_token != null)
            {
                uwr.SetRequestHeader("access-token", _token.accessToken);
                uwr.SetRequestHeader("refresh-token", _token.refreshToken);
            }

            yield return uwr.SendWebRequest();
            if (uwr.result == UnityWebRequest.Result.Success)
            {
                // Debug.Log("Received: " + uwr.downloadHandler.text);
                callback(uwr.downloadHandler.text);
            }
            else
            {
                // Debug.Log("Error While Sending: " + uwr.error);
            }
            uwr.Dispose();
        }

        private IEnumerator PostRequest(string url, string json, Action<string> callback)
        {
            var uwr = new UnityWebRequest(url, "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");
            if (_token != null)
            {
                uwr.SetRequestHeader("access-token", _token.accessToken);
                uwr.SetRequestHeader("refresh-token", _token.refreshToken);
            }

            //Send the request then wait here until it returns
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.Success)
            {
                // Debug.Log("Received: " + uwr.downloadHandler.text);
                callback(uwr.downloadHandler.text);
            }
            else
            {
                // Debug.Log("Error While Sending: " + uwr.error);
            }
            uwr.Dispose();

        }

        private IEnumerator PutRequest(string url, string json, Action<string> callback)
        {
            var uwr = new UnityWebRequest(url, "PUT");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");
            if (_token != null)
            {
                uwr.SetRequestHeader("access-token", _token.accessToken);
                uwr.SetRequestHeader("refresh-token", _token.refreshToken);
            }

            //Send the request then wait here until it returns
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.Success)
            {
                // Debug.Log("Received: " + uwr.downloadHandler.text);
                callback(uwr.downloadHandler.text);
            }
            else
            {
                // Debug.Log("Error While Sending: " + uwr.error);
            }
            uwr.Dispose();

        }

        public void Login(string id)
        {
            User user = new(id, "1234");
            string json = JsonUtility.ToJson(user);
            StartCoroutine(PostRequest(ServerURL + "/user/login", json, SetToken));
        }

        public void SetToken(string json)
        {
            _token = JsonUtility.FromJson<Token>(json);
        }
        public void SetToken(Token token)
        {
            _token = token;
            Debug.Log("토큰 설정 완료:"+_token.accessToken+","+_token.refreshToken);
        }
        public void GetInventory(string category, Action<string> callback)
        {
            StartCoroutine(GetRequest(ServerURL + "/inventory?type="+category, callback));
        }

        public void GetRoom(long roomId, Action<string> callback)
        {
            StartCoroutine(GetRequest(ServerURL + "/room/" + roomId, callback));
        }
        
        public void SaveRoom(long roomId, string json, Action<string> callback)
        {
            StartCoroutine(PutRequest(ServerURL + "/room/" + roomId, json, callback));
        }

        public void GetTemplate(long templateId, Action<string> callback)
        {
            StartCoroutine(GetRequest(ServerURL + "/template/" + templateId, callback));

        }

        public void SaveTemplate(long templateId, string json, Action<string> callback)
        {
            StartCoroutine(PutRequest(ServerURL + "/template/" + templateId, json, callback));
        }
        public void GetPlayList(long roomId, Action<string> callback)
        {
            StartCoroutine(GetRequest(ServerURL + "/room/"+roomId+"/playlist",callback));
        }
        public void SavePlayList(long roomId,string json)
        {
            StartCoroutine(PutRequest(ServerURL + "/room/"+roomId+"/playlist",json,s => { }));
        }

        public void GetDailyPet(Action<string> callback)
        {
            StartCoroutine(GetRequest(ServerURL + "/pet/face", callback));
        }
    }
}