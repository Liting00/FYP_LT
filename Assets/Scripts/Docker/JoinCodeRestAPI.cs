using System.IO;
using System.Net;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;

public static class JoinCodeRestAPI
{
    public static List<T> Deserialize<T>(this string SerializedJSONString) => JsonConvert.DeserializeObject<List<T>>(SerializedJSONString);

    public static string GetJoinCodes()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{GameSettings.BASE}/joinCodes");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        return reader.ReadToEnd();
    }
    public static JoinCode GetJoinCode(string joinCode)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{GameSettings.BASE}/joinCode/{joinCode}");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        return JsonUtility.FromJson<JoinCode>(json);
    }
    //TODO: Delete and Put
    public static IEnumerator PutJoinCode(string joinCode, string allocationID)
    {
        string data = "{\"allocationID\":\"" + allocationID + "\"}";
        //Debug.Log(data);

        byte[] myData = System.Text.Encoding.UTF8.GetBytes(data);
        string uri = GameSettings.BASE + "/joinCode/" + joinCode;

        using (UnityWebRequest webRequest = UnityWebRequest.Put(uri, data))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log("Upload complete!");
            }
        }
    }
    public static IEnumerator GetRequest()
    {
        string uri = GameSettings.BASE + "/joinCodes";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }
    public static IEnumerator Delete(string joinCode)
    {
        string uri = GameSettings.BASE + "/joinCode/" + joinCode;

        using (UnityWebRequest webRequest = UnityWebRequest.Delete(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log("Delete complete!");
            }
        }
        
    }
}
[System.Serializable]
public class JoinCode
{
    public string joinCode;
    public string allocationID;
}