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
    private GameObject mainMenu;

    [SerializeField]
    private GameObject playerUI;

    [SerializeField]
    private GameObject playerMenu;

    [SerializeField]
    private GameObject advisorUI;
    [SerializeField]
    private GameObject advisorMenu;

    [SerializeField]
    private Button playAsAdvisorButton;

    [SerializeField]
    private Button advisorBackButton;

    [SerializeField]
    private TextMeshProUGUI adviseTextBox;

    [SerializeField]
    private TMP_InputField inputCodeText;

    [SerializeField]
    private TextMeshProUGUI joinCodeText;

    [SerializeField]
    private GameObject loadingIcon;

    [SerializeField]
    private TextMeshProUGUI numGameText;

    [SerializeField]
    private TextMeshProUGUI playerText;

    [SerializeField]
    private TextMeshProUGUI playerInfoText;

    private void Awake()
    {
        Cursor.visible = true;
    }
    private void Update()
    {
        playersInGameText.text = $"Player in Game: {PlayerManager.Instance.PlayerInGame}";
        joinCodeText.text = RelayManager.Instance.JoinCode;

        if(GameManager.Instance.IsGameStarted)
            playerInfo();
    }
    private void Start()
    {
        //Logger.Instance.resetScore();
        mainMenu.SetActive(false);
        advisorMenu.SetActive(false);
        advisorUI.SetActive(false);
        startGameButton.gameObject.SetActive(false);
        loadingIcon.SetActive(false);
        playerUI.SetActive(false);
        joinCodeText.gameObject.SetActive(false);
        adviseTextBox.gameObject.SetActive(false);
        numGameText.gameObject.SetActive(false);
        playerText.gameObject.SetActive(false);
        playerInfoText.gameObject.SetActive(false);

        mainMenu.SetActive(true);

        //Start HOST
        startHostButton?.onClick.AddListener(async () =>
        {
            if (RelayManager.Instance.isRelayEnabled)
            {
                mainMenu.SetActive(false);
                loadingIcon.SetActive(true);

                await RelayManager.Instance.SetupRelay();

                loadingIcon.SetActive(false);
            }

            if (NetworkManager.Singleton.StartHost())
            {
                Debug.Log("Host Started...");
                mainMenu.SetActive(false);
                joinCodeText.gameObject.SetActive(true);
                startGameButton.gameObject.SetActive(true);
                numGameText.text = $"Game: {++GameManager.Instance.NumberOfGames}";
                numGameText.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("Host could not be Started...");
            }
        });
        //Start Client
        startClientButton?.onClick.AddListener(async () =>
        {
            if (RelayManager.Instance.isRelayEnabled && !string.IsNullOrEmpty(inputCodeText.text))
            {
                advisorMenu.SetActive(false);
                loadingIcon.SetActive(true);
                await RelayManager.Instance.JoinRelay(inputCodeText.text);
                loadingIcon.SetActive(false);
            }
            else
            {
                advisorMenu.SetActive(true);
                Debug.Log("Empty Input Code. Client could not be Started");
                return;
            }

            if (NetworkManager.Singleton.StartClient())
            {
                Debug.Log("Client Started...");
            }
            else
            {
                Debug.Log("Client could not be Started...");
            }
        });
        playAsAdvisorButton?.onClick.AddListener(() =>
        {
            mainMenu.SetActive(false);
            advisorMenu.SetActive(true);
        });
        advisorBackButton?.onClick.AddListener(() =>
        {
            mainMenu.SetActive(true);
            advisorMenu.SetActive(false);
        });
        // STATUS TYPE CALLBACKS
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            Debug.Log($"{id} just connected...");
        };
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
                startGameButton.gameObject.SetActive(false);
                joinCodeText.gameObject.SetActive(false);
                numGameText.gameObject.SetActive(false);
                playerText.gameObject.SetActive(false);
                Debug.Log("Player Start Game");

                playerStartGame();
                advisorStartGameClientRpc();

                GameManager.Instance.ChangeState(GameState.GenerateGrid);
                return;
            }
        });
    }
    private void playerStartGame()
    {
        AdvisorManager.Instance.setAdvisorTextBoxState(true);
        PlayerManager.Instance.setPlayerUIState(true);
        playerInfoText.gameObject.SetActive(true);
    }
    [ClientRpc]
    private void advisorStartGameClientRpc()
    {
        if (IsOwner) return;

        AdvisorManager.Instance.setAdvisorUIState(true);
        AdvisorManager.Instance.setAdvisorTextBoxState(true);
        playerInfoText.gameObject.SetActive(true);
    }

    public void roundOver()
    {
        //TODO: Might change text
        playerText.text = "All Hostile are killed";
        playerText.gameObject.SetActive(true);

        playerUI.gameObject.SetActive(false);
        adviseTextBox.gameObject.SetActive(false);

        numGameText.text = $"Game: {++GameManager.Instance.NumberOfGames}";
        numGameText.gameObject.SetActive(true);
        
        Logger.Instance.accumlateScore();
        Logger.Instance.resetScore();

        if (GameManager.Instance.NumberOfGames < GameSettings.NUMBEROFGAMES)
            startGameButton.gameObject.SetActive(true);
        else
            Debug.Log("TODO");
    }
    //Update player Info Text
    public void playerInfo()
    {
        int greenRemove = Logger.Instance.GreenRemove;
        int blueRemove = Logger.Instance.BlueRemove;
        int redRemove = Logger.Instance.RedRemove;
        int infected = Logger.Instance.Infected;

        playerInfoText.text = $"Green Remove: {greenRemove}\n " +
            $"Blue Remove: {blueRemove}\n " +
            $"Red Remove: {redRemove}\n " +
            $"Infected: {infected}";
    }
}
