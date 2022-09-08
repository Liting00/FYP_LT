using UnityEngine;
using Unity.Netcode;
using Network.Singletons;
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

    public PlayerState playerState { get; set; }

    void Awake()
    {
        Instance = this;
        playerInGameText.gameObject.SetActive(true);
        playerState = PlayerState.NoState;
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

        if (playersInGame.Value == 2)
            allowQuickJoin = false;
        else
            allowQuickJoin = true;
    }
}
public enum PlayerState
{
    Shooter = 0,
    Advisor = 1,
    NoState = 2
}

