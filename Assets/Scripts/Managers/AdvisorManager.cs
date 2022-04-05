using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using DilmerGames.Core.Singletons;

public class AdvisorManager : NetworkSingleton<AdvisorManager>
{
    string[] advise = { "Destroy", "Don't Destroy", "Destroy when alone" };
    [SerializeField] internal GameObject advisorBox;
    [SerializeField] private GameObject advisorUI;

    internal void insertAdvise(string advise)
    {
        advisorBox.GetComponent<UnityEngine.UI.Text>().text = advise;
    }

    void Start()
    {
        /*NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (IsClient)
            {
                Debug.Log($"ID {id} just connected");
                advisorUI.SetActive(true);
            }
        };
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (IsClient)
            {
                Debug.Log($"ID {id} just disconnected");
                advisorUI.SetActive(false);
            }
        };*/
    }


}
