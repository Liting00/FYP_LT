using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DilmerGames.Core.Singletons;
using Unity.Netcode;

public class UIManager : NetworkSingleton<UIManager>
{
    [SerializeField]
    private Button startServerButton;

    [SerializeField]
    private Button startHostButton;

    [SerializeField]
    private Button startClientButton;

    [SerializeField]
    private Button startGameButton;
    
    [SerializeField]
    private TextMeshProUGUI playersInGameText;

    private bool hasServerStarted;

    [SerializeField]
    private GameObject playerUI;

    [SerializeField]
    private GameObject advisorUI;

    [SerializeField]
    private TextMeshProUGUI adviseTextBox;

    private void Awake()
    {
        Cursor.visible = true;
    }
    private void Update()
    {
        playersInGameText.text = $"Player in Game: {PlayerManager.Instance.PlayerInGame}";
    }
    private void Start()
    {
        startHostButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartHost())
            {
                Debug.Log("Host Started...");
            }
            else
            {
                Debug.Log("Host could not be Started...");
            }
        });
        startClientButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartClient())
            {
                Debug.Log("Client Started...");
            }
            else
            {
                Debug.Log("Client could not be Started...");
            }
        });
        startServerButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartServer())
            {
                Debug.Log("Server Started...");
            }
            else
            {
                Debug.Log("Server could not be Started...");
            }
        });
        NetworkManager.Singleton.OnServerStarted += () =>
        {
            hasServerStarted = true;
        };
        startGameButton.onClick.AddListener(() =>
        {
            if (!hasServerStarted)
            {
                Debug.Log("Server has not yet started.");
                return;
            }
            if (IsServer)
            {
                Debug.Log("Player Start Game");
                playerUI.SetActive(true);
                adviseTextBox.gameObject.SetActive(true);
                adviseTextBox.text = "No Instruction";
                updateClientRpc();
                GameManager.Instance.ChangeState(GameState.GenerateGrid);
                return;
            }
        });
    }
    [ClientRpc]
    private void updateClientRpc()
    {
        if (IsOwner) return;

        Debug.Log("Active Advisor UI");
        advisorUI.SetActive(true);
        adviseTextBox.gameObject.SetActive(true);
        adviseTextBox.text = "No Instruction";
    }
    [ServerRpc(RequireOwnership = false)]
    public void updateServerRpc(string advise)
    {
        Debug.Log("Update Server");
        adviseTextBox.text = advise;
    }
    public void updateAdviseText(string advise)
    {
        adviseTextBox.text = advise;
    }

}
