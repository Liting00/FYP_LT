using UnityEngine;
using Unity.Netcode;
using DilmerGames.Core.Singletons;

public class PlayerManager : NetworkSingleton<PlayerManager>
{
    private NetworkVariable<int> playersInGame = new NetworkVariable<int>();

    [SerializeField]
    private GameObject playerUI;

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
    public void addPlayerInGame(int value)
    {
        playersInGame.Value = playersInGame.Value + value;
    }
    public void setPlayerUIState(bool state)
    {
        playerUI.gameObject.SetActive(state);
    }
    private void Start()
    {
        Debug.Log("Start Player Manager");
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (IsServer)
            {
                Debug.Log($"ID {id} just connected");
                addPlayerInGame(1);
            }
                
        };
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (IsServer)
            {
                Debug.Log($"ID {id} just disconnected");
                addPlayerInGame(-1);
            }
                
        };
    }
}

