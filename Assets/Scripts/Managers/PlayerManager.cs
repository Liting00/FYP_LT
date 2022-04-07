using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using DilmerGames.Core.Singletons;

public class PlayerManager : NetworkSingleton<PlayerManager>
{
    private NetworkVariable<int> playersInGame = new NetworkVariable<int>();

    [SerializeField] private GameObject playerUI;
    

    public int PlayerInGame
    {
        get
        {
            return playersInGame.Value;
        }
    }
    public void setPlayerInGame(int value)
    {
        playersInGame.Value = value;

    }
    private void Start()
    {
        //Debug.Log("Start");
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (IsHost)
            {
                Debug.Log($"ID {id} just connected");
                playersInGame.Value++;
                //playerUI.SetActive(true);
            }
        };
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (IsHost)
            {
                Debug.Log($"ID {id} just disconnected");
                playersInGame.Value--;
                //playerUI.SetActive(false);
            }
        };
    }
}

