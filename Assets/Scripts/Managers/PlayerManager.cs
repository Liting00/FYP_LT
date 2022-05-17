using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using DilmerGames.Core.Singletons;

public class PlayerManager : NetworkSingleton<PlayerManager>
{
    private NetworkVariable<int> playersInGame = new NetworkVariable<int>();
    
    public int PlayerInGame
    {
        get
        {
            return playersInGame.Value;
        }
    }
    private void Start()
    {
        Debug.Log("Start");
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (IsServer)
            {
                Debug.Log($"ID {id} just connected");
                playersInGame.Value++;
            }
        };
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (IsServer)
            {
                Debug.Log($"ID {id} just disconnected");
                playersInGame.Value--;
            }
        };
    }
}

