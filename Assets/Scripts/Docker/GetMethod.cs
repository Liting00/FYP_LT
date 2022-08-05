using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

public class GetMethod : MonoBehaviour
{
    public void getJoinCodes() => StartCoroutine(GetJoinCode_Coroutine());

    IEnumerator GetJoinCode_Coroutine()
    {
        string uri = GameSettings.BASE + "/joincodes";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();
            if(request.isNetworkError || request.isHttpError)
            {
                Debug.Log("Network Error. Cannot get Join Codes.");
            }
        }
    }
}