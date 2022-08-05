using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DilmerGames.Core.Singletons;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

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
    private Button backButton;

    private bool hasServerStarted;

    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private GameObject playerUI;

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


    private int i = 0;
    int[] scorearray = new int[5];

    //private bool quickJoined { get; set; }

    private int nextSceneIndex, prevSceneIndex;

    private void Awake()
    {
        Cursor.visible = true;
    }
    private void Update()
    {
        if(joinCodeText.gameObject.activeSelf)
            joinCodeText.text = "Lobby Code: " + RelayManager.Instance.JoinCode;

        if ((Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return)) && advisorMenu.activeSelf)
        {
            startClientButton.onClick.Invoke();
        }

        if (GameManager.Instance.IsGameStarted)
            playerInfo();

        if (PlayerManager.Instance.allowQuickJoin)
        {
            advisorStartGameClientRpc();
            //quickJoined = false;
        }
    }
    private void Start()
    {
        nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        prevSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;

        inputCodeText.text = inputCodeText.text.ToUpper();

        //Logger.Instance.resetScore();
        backButton.gameObject.SetActive(true);
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

        StartCoroutine(loadAssets());

        //Start HOST (Shooter)
        startHostButton?.onClick.AddListener(async () =>
        {
            if (RelayManager.Instance.isRelayEnabled)
            {
                mainMenu.SetActive(false);
                backButton.gameObject.SetActive(false);
                loadingIcon.SetActive(true);

                await RelayManager.Instance.SetupRelay();

                loadingIcon.SetActive(false);
            }

            if (NetworkManager.Singleton.StartHost())
            {
                Debug.Log("Host Started...");

                PlayerManager.Instance.playerState = PlayerState.Shooter;

                numGameText.text = $"Game: {++GameManager.Instance.NumberOfGames}";

                mainMenu.SetActive(false);
                joinCodeText.gameObject.SetActive(true);
                startGameButton.gameObject.SetActive(true);
                numGameText.gameObject.SetActive(true);

                i++;
            }
            else
            {
                Debug.Log("Host could not be Started...");
            }
        });
        //Start Client (Advisor)
        startClientButton?.onClick.AddListener(async () =>
        {
            if (RelayManager.Instance.isRelayEnabled && !string.IsNullOrEmpty(inputCodeText.text))
            {
                Debug.Log("Start Advisor");
                advisorMenu.SetActive(false);
                backButton.gameObject.SetActive(false);
                playerText.gameObject.SetActive(false);
                loadingIcon.SetActive(true);
                try
                {
                    PlayerManager.Instance.playerState = PlayerState.Advisor;
                    await RelayManager.Instance.JoinRelay(inputCodeText.text);
                    loadingIcon.SetActive(false);
                }
                catch(Exception e)
                {
                    Debug.Log($"Invalid Input Code. {e}");
                    loadingIcon.SetActive(false);

                    playerText.text = "Could not Join Game. Invalid Input Code";
                    playerText.gameObject.SetActive(true);
                    
                    advisorMenu.SetActive(true);
                    backButton.gameObject.SetActive(true);
                    return;
                }
            }
            else if(string.IsNullOrEmpty(inputCodeText.text))
            {
                advisorMenu.SetActive(true);

                playerText.text = "Could not Join Game. Empty Input Code";
                playerText.gameObject.SetActive(true);

                Debug.Log("Empty Input Code.");
                return;
            }
            else
            {
                advisorMenu.SetActive(true);

                playerText.text = "Could not Join Game. Network Unable";
                playerText.gameObject.SetActive(true);

                Debug.Log("Network Unable.");
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
            backButton.gameObject.SetActive(false);
            advisorMenu.SetActive(true);
            //TODO: Get Request
            try 
            {
                //StartCoroutine(JoinCodeRestAPI.GetRequest());
                string json = JoinCodeRestAPI.GetJoinCodes();
                List<JoinCode> playerList = new List<JoinCode>();
                playerList = JoinCodeRestAPI.Deserialize<JoinCode>(json);
                for(int i=0; i < playerList.Count(); i++)
                {
                    Debug.Log(playerList[i].joinCode);
                }
                //JoinCode joinCode = JoinCodeRestAPI.GetJoinCode("JKAWEDI");
                //Debug.Log(joinCode.joinCode);
            }
            catch(Exception e)
            {
                Debug.Log("Cannot Get Join Codes" + e);
            }
        });
        advisorBackButton?.onClick.AddListener(() =>
        {
            mainMenu.SetActive(true);
            backButton.gameObject.SetActive(true);
            advisorMenu.SetActive(false);
            playerText.gameObject.SetActive(false);
        });
        backButton?.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(prevSceneIndex);
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
                playerUI.SetActive(true);
                Debug.Log("Player Start Game");

                loadingIcon.SetActive(true);
                playerStartGame();
                advisorStartGameClientRpc();
                loadingIcon.SetActive(false);

                GameManager.Instance.ChangeState(GameState.GenerateGrid);
                return;
            }
        });
    }
    private void playerStartGame()
    {
        AdvisorManager.Instance.setAdvisorTextBoxState(true);
        //PlayerManager.Instance.setPlayerUIState(true);
        playerInfoText.gameObject.SetActive(true);
    }
    [ClientRpc]
    private void advisorStartGameClientRpc()
    {
        if (IsOwner) return;

        Debug.Log("Enable Advisor UI");
        AdvisorManager.Instance.setAdvisorUIState(true);
        AdvisorManager.Instance.setAdvisorTextBoxState(true);
        playerInfoText.gameObject.SetActive(true);
        AdvisorManager.Instance.insertAdvise(AdvisorAdvice.NoAdvice);
    }
    public void roundOver(GameState gameState)
    {
        playerText.text = GameResultMessage(gameState);
        playerText.gameObject.SetActive(true);

        playerUI.gameObject.SetActive(false);
        adviseTextBox.gameObject.SetActive(false);

        Logger.Instance.accumlateScore();
        Logger.Instance.resetScore();

        scorearray[i-1] = (GameManager.Instance.NumberOfGames);
        Debug.Log("value "+ (i-1) +" : "+ scorearray[i-1]);
        
        if(gameState != GameState.GameOver)
        {
            numGameText.text = $"Game: {++GameManager.Instance.NumberOfGames}";
            startGameButton.gameObject.SetActive(true);
            numGameText.gameObject.SetActive(true);
        }
        else
        {
            startGameButton.gameObject.SetActive(false);
            numGameText.gameObject.SetActive(false);
        }
            
    }
    //Update player Info Text
    public void playerInfo()
    {
        int greenRemove = Logger.Instance.GreenRemove;
        int blueRemove = Logger.Instance.BlueRemove;
        int redRemove = Logger.Instance.RedRemove;
        int infected = Logger.Instance.Infected;
        float time = GameManager.Instance.timer;

        playerInfoText.text = $"Green Remove: {greenRemove}\n " +
            $"Blue Remove: {blueRemove}\n " +
            $"Red Remove: {redRemove}\n " +
            $"Infected: {infected}\n" +
            $"Timer: {((int)time)}s";

    }
    IEnumerator loadAssets()
    {
        loadingIcon.SetActive(true);
        yield return new WaitForSeconds(1f);
        loadingIcon.SetActive(false);
        mainMenu.SetActive(true);
        Debug.Log("Loading Complete");
    }
    private string GameResultMessage(GameState gameState)
    {
        string gameMessage = "";

        if (gameState == GameState.WinRound)
            gameMessage = GameSettings.WINGAMETEXT;
        else if (gameState == GameState.LoseRound)
            gameMessage = GameSettings.LOSEGAMETEXT;
        else if (gameState == GameState.GameOver)
            gameMessage = GameSettings.GAMEOVERTEXT;

        return gameMessage;
    }
}

