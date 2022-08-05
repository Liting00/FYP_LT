using System.Collections;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections.Generic;

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
}
[System.Serializable]
public class JoinCode
{
    public string joinCode;
    public string allocationID;
}