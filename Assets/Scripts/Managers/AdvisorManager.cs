using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using DilmerGames.Core.Singletons;

public class AdvisorManager : NetworkSingleton<PlayerManager>
{
    string[] advise = { "Destroy", "Don't Destroy", "Destroy when alone" };
    [SerializeField] internal GameObject advisorBox;
    [SerializeField] private GameObject advisorUI;

    PlayerManager playerManager;

    internal void insertAdvise(string advise)
    {
        advisorBox.GetComponent<UnityEngine.UI.Text>().text = advise;
    }

    void Start()
    {
        GameObject PManager = GameObject.Find("Player Manager");
        playerManager = PManager.GetComponent<PlayerManager>();

        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (IsClient)
            {
                Debug.Log($"ID {id} just connected");
                playerManager.setPlayerInGame(playerManager.PlayerInGame + 1);
                advisorUI.SetActive(true);
            }
        };
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (IsClient)
            {
                Debug.Log($"ID {id} just disconnected");
                playerManager.setPlayerInGame(playerManager.PlayerInGame - 1);
                advisorUI.SetActive(false);
            }
        };
    }


}
