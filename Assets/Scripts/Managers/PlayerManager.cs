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
    public string JoinCode
    {
        get
        {
            return RelayManager.Instance.joinCode;
        }
    }
    private void Start()
    {
        Debug.Log("Start Player Manager");
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

