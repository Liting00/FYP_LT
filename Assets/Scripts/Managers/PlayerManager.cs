using UnityEngine;
using Unity.Netcode;
using DilmerGames.Core.Singletons;
using TMPro;

public class PlayerManager : NetworkSingleton<PlayerManager>
{
    public static new PlayerManager Instance;

    private NetworkVariable<int> playersInGame = new NetworkVariable<int>();

    [SerializeField]
    private TextMeshProUGUI playerInGameText;

    public bool allowQuickJoin = true;

    public int PlayerInGame
    {
        get
        {
            return playersInGame.Value;
        }
        set
        {
            playersInGame.Value = value;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
        playerInGameText.gameObject.SetActive(true);
    }

    private void Start()
    {
        Debug.Log("Start Player Manager");

        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (NetworkManager.IsServer)
            {
                Debug.Log($"ID {id} just connected");
                PlayerInGame++;
            }   
        };
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (NetworkManager.IsServer)
            {
                Debug.Log($"ID {id} just disconnected");
                PlayerInGame--;
            }    
        };
    }
    void Update()
    {
        playerInGameText.text = $"Player in Game: {PlayerInGame}";

        if (playersInGame.Value < 2)
            allowQuickJoin = true;
        else
            allowQuickJoin = false;
    }
    //TODO: Not Used
    private void playerStartGame()
    {
        AdvisorManager.Instance.setAdvisorTextBoxState(true);
        //setPlayerUIState(true);
    }
    /*[ClientRpc]
    private void advisorStartGameClientRpc()
    {
        if (IsOwner) return;

        AdvisorManager.Instance.setAdvisorUIState(true);
        AdvisorManager.Instance.setAdvisorTextBoxState(true);
    }*/
}

