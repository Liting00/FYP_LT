using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Network.Singletons;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System;
using System.Threading;

public class UIManager : NetworkSingleton<UIManager>
{
    [SerializeField]
    private Button quitButton;

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
    private GameObject joinCodeloadingIcon;

    [SerializeField]
    private TextMeshProUGUI numGameText;

    [SerializeField]
    private TextMeshProUGUI playerText;

    [SerializeField]
    private TextMeshProUGUI additionalInst;

    [SerializeField]
    private TextMeshProUGUI additionalInstTop;

    [SerializeField]
    private TextMeshProUGUI playerInfoText;

    private bool seal = false;


    private int GameNum = 0;
    int[] scorearray = new int[5];

    private int nextSceneIndex, prevSceneIndex;

    //Join Code List
    List<JoinCode> playerList = null;
    
    //Thread Lock
    public object _lock = new object();

    //Human Advisor additional Instruction
    string additionalGoalText = "";

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
            playerInfoLogger();

        if (PlayerManager.Instance.allowQuickJoin)
        {
            advisorStartGameClientRpc();
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
        additionalInst.gameObject.SetActive(false);
        additionalInstTop.gameObject.SetActive(false);

        StartCoroutine(loadAssets());

        //Quit Game Button
        quitButton?.onClick.AddListener(() =>
        {
            Debug.Log("Quit Game Button Pressed");
            SceneManager.LoadScene(nextSceneIndex);
        });
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

                numGameText.text = $"Game: {1+GameManager.Instance.NumberOfGames}";
                Debug.Log(numGameText.text);

                mainMenu.SetActive(false);
                joinCodeText.gameObject.SetActive(true);
                startGameButton.gameObject.SetActive(true);
                numGameText.gameObject.SetActive(true);

                //Game Number 1
                GameNum++;

                //Initalize Logger
                Logger.Instance.initLogging();
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
                additionalInst.gameObject.SetActive(false);
                loadingIcon.SetActive(true);
                try
                {
                    PlayerManager.Instance.playerState = PlayerState.Advisor;
                    await RelayManager.Instance.JoinRelay(inputCodeText.text);
                    loadingIcon.SetActive(false);
                    additionalInstTop.text = additionalGoalText;
                    additionalInstTop.gameObject.SetActive(true);

                    //Send Roleplay state to Host
                    updateRoleplayServerRpc(AdvisorManager.Instance.roleplay);
                }
                catch(Exception e)
                {
                    Debug.Log($"Invalid Input Code. {e}");
                    loadingIcon.SetActive(false);

                    playerText.text = "Could not Join Game. Invalid Input Code";
                    playerText.gameObject.SetActive(true);
                    StartCoroutine(JoinCodeError());

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
                StartCoroutine(JoinCodeError());

                Debug.Log("Empty Input Code.");
                return;
            }
            else
            {
                advisorMenu.SetActive(true);

                playerText.text = "Could not Join Game. Network Unable";
                playerText.gameObject.SetActive(true);
                StartCoroutine(JoinCodeError());

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
            Debug.Log("Player As Advisor");
            StartCoroutine(loadJoinCodes());
            mainMenu.SetActive(false);
            backButton.gameObject.SetActive(false);
            advisorMenu.SetActive(true);
            additionalInst.gameObject.SetActive(true);
            
            if (AdvisorManager.Instance.roleplay == Roleplay.BlueBias)
                additionalGoalText = GameSettings.ADDITIONALGOALTEXT + "Remove Blue NPCs and Red NPCs";
            else if (AdvisorManager.Instance.roleplay == Roleplay.GreenBias)
                additionalGoalText = GameSettings.ADDITIONALGOALTEXT + "Remove Green & Red NPCs";
            else
                additionalInst.gameObject.SetActive(false);

            additionalInst.text = additionalGoalText;
        });
        advisorBackButton?.onClick.AddListener(() =>
        {
            mainMenu.SetActive(true);
            backButton.gameObject.SetActive(true);
            advisorMenu.SetActive(false);
            playerText.gameObject.SetActive(false);
            JoinCodeLobbyManager.instance.DestroyLobby();
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
                ++GameManager.Instance.NumberOfGames;
                startGameButton.gameObject.SetActive(false);
                joinCodeText.gameObject.SetActive(false);
                numGameText.gameObject.SetActive(false);
                playerText.gameObject.SetActive(false);
                playerUI.SetActive(true);
                Debug.Log("Player Start Game");

                loadingIcon.SetActive(true);
                playerStartGame();
                Logger.Instance.startGameLog(GameManager.Instance.NumberOfGames);
                loadingIcon.SetActive(false);

                GameManager.Instance.ChangeState(GameState.GenerateGrid);
                Debug.Log("Start Advisor keys");
                advisorStartGameClientRpc();
                //Add Logger for 2 player roleplay
                if (PlayerManager.Instance.PlayerInGame == 2 && !seal)
                {
                    string roleplay = AdvisorManager.Instance.roleplay.ToString();
                    Logger.Instance.LogRolePlay(roleplay);
                    seal = true;
                }    
                else if(!seal)
                {
                    Logger.Instance.LogRolePlay("nill");
                }
                    


                return;
            }
        });
    }
    IEnumerator JoinCodeError()
    {
        RectTransform assign_text_1RT = playerText.GetComponent<RectTransform>();
        assign_text_1RT.anchoredPosition = new Vector3(29.60201f, 300f, 0f);

        yield return new WaitForSeconds(3f);
        assign_text_1RT.anchoredPosition = new Vector3(29.60201f, 156.5f, 0f);
        playerText.gameObject.SetActive(false);
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

        //blue,red,green elimination
        playerInfoText.gameObject.SetActive(false);
        Debug.Log(GameManager.Instance.GameState);
        if (GameManager.Instance.GameState == GameState.WinRound || GameManager.Instance.GameState == GameState.NoState
            || GameManager.Instance.GameState == GameState.LoseRound)
        {
            Debug.Log("Shooter Waiting to Start Game");
            AdvisorManager.Instance.setAdvisorUIState(false);
            AdvisorManager.Instance.setAdvisorTextBoxState(false);
            playerText.text = GameSettings.ADVISORWAITTEXT;
            playerText.gameObject.SetActive(true);
        }
        else 
        {
            Debug.Log("Shooter Start Game");
            Debug.Log("Enable Advisor UI");
            AdvisorManager.Instance.setAdvisorUIState(true);
            AdvisorManager.Instance.setAdvisorTextBoxState(true);
            //TODO: Add or not Add when Human Advisor enters the game
            AdvisorManager.Instance.insertAdvise(AdvisorAdvice.NoAdvice);
            AdvisorManager.Instance.updateAdviseTextServerRpc(AdvisorAdvice.NoAdvice);
            playerText.gameObject.SetActive(false);
        }
    }
    public void roundOver(GameState gameState)
    {
        //Send to Advisor
        AdvisorRoundOverClientRPC(gameState);

        playerText.text = GameResultMessage(gameState);
        playerText.gameObject.SetActive(true);

        playerUI.gameObject.SetActive(false);
        adviseTextBox.gameObject.SetActive(false);

        Logger.Instance.accumlateScore();
        Logger.Instance.resetScore();

        scorearray[GameNum - 1] = (GameManager.Instance.NumberOfGames);
        //Debug.Log("value "+ (GameNum - 1) +" : "+ scorearray[GameNum - 1]);
        
        if(gameState != GameState.GameOver)
        {
            numGameText.text = $"Game: {1+GameManager.Instance.NumberOfGames}";
            startGameButton.gameObject.SetActive(true);
            numGameText.gameObject.SetActive(true);
        }
        else
        {
            startGameButton.gameObject.SetActive(false);
            numGameText.gameObject.SetActive(false);
        }
            
    }
    [ClientRpc]
    private void AdvisorRoundOverClientRPC(GameState gameState)
    {
        if (IsOwner) return;

        Debug.Log("Disable Advisor UI");
        AdvisorManager.Instance.setAdvisorUIState(false);
        AdvisorManager.Instance.setAdvisorTextBoxState(false);

        Debug.Log(gameState);
        if (gameState == GameState.WinRound || gameState == GameState.LoseRound)
        {
            Debug.Log("Show Advisor Message");
            playerText.text = GameSettings.ADVISORWAITTEXT;
            playerText.gameObject.SetActive(true);
        }
    }
    //Update player Info Text
    public void playerInfoLogger()
    {
        int greenRemove = Logger.Instance.GreenRemove;
        int blueRemove = Logger.Instance.BlueRemove;
        int redRemove = Logger.Instance.RedRemove;
        //int infected = Logger.Instance.Infected;
        int infected = SpawnManager.Instance.Infected;
        float time = GameManager.Instance.timer;

        if (infected <= 4)
        {
            playerInfoText.text = $"Green Remove: {greenRemove}\n " +
            $"Blue Remove: {blueRemove}\n " +
            $"Red Remove: {redRemove}\n " +
            $"<color=#00FF00>Infected: {infected}</color>\n" +
            $"Timer: {(int)time}s";
        }
        else if (infected >4 && infected<=12)
        {
            playerInfoText.text = $"Green Remove: {greenRemove}\n " +
            $"Blue Remove: {blueRemove}\n " +
            $"Red Remove: {redRemove}\n " +
            $"<color=#FFFF00>Infected: {infected}</color>\n" +
            $"Timer: {(int)time}s";
        }
        else
        {
            playerInfoText.text = $"Green Remove: {greenRemove}\n " +
            $"Blue Remove: {blueRemove}\n " +
            $"Red Remove: {redRemove}\n " +
            $"<color=#FF0000>Infected: {infected}</color>\n" +
            $"Timer: {(int)time}s";
        }

    }
    IEnumerator loadJoinCodes()
    {
        Debug.Log("Loading Join Codes");
        var stopWatch = new System.Diagnostics.Stopwatch();
        stopWatch.Restart();

        var thread = new Thread(JoinCodeListThread);
        thread.Start();

        joinCodeloadingIcon.SetActive(true);

        //If http resquest has no response. Artifical delay 2.5 secs
        while (!JoinCodeListReceived() && stopWatch.IsRunning)
        {
            Debug.Log("Wait for Join Code " + stopWatch.ElapsedMilliseconds.ToString() + "s");
            yield return new WaitForSeconds(0.1f);
        }
        stopWatch.Stop();
        Debug.Log("Thread Time Taken: " + stopWatch.ElapsedMilliseconds.ToString());

        yield return new WaitForSeconds(1f);
        joinCodeloadingIcon.SetActive(false);

        if(playerList != null)
            JoinCodeLobbyManager.instance.DisplayLobbies(playerList);

        //JoinCode joinCode = JoinCodeRestAPI.GetJoinCode("JKAWEDI");
        //Debug.Log(joinCode.joinCode);
    }
    private bool JoinCodeListReceived()
    {
        lock(_lock)
            return playerList != null;
    }
    private void JoinCodeListThread()
    {
        try
        {
            //List<JoinCode> playerList = new List<JoinCode>();
            playerList = new List<JoinCode>();
            string json = JoinCodeRestAPI.GetJoinCodes();
            
            lock (_lock)
            {
                playerList = JoinCodeRestAPI.Deserialize<JoinCode>(json);
            }

            Debug.Log("BackgroundThread: Finished getting Shooter's list");
            
        }
        catch (Exception e)
        {
            playerList = null;
            Debug.Log("Error Cannot Get Join Codes. " + e);
        }
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
        else if (gameState == GameState.Interrupted)
            gameMessage = GameSettings.INTERRUPTEDTEXT;

        return gameMessage;
    }
    [ServerRpc(RequireOwnership = false)]
    public void updateRoleplayServerRpc(Roleplay roleplay)
    {
        Debug.Log("Send Roleplay to Host");
        AdvisorManager.Instance.roleplay = roleplay;

    }

}

