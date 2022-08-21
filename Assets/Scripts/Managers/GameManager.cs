using Network.Singletons;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkSingleton<GameManager>
{
    //public static GameManager Instance;
    public GameState GameState = GameState.NoState;

    private float nextUpdatedTime = 0.1f;
    private float update = 0f;

    private bool gamestart = false;

    public int NumberOfGames { get; set; }

    private float counter_timer;
    private bool counter_start = false;
    public float timer { get => counter_timer; set => counter_timer = value; }

    private void Update()
    {
        if (!gamestart)
            return;

        if (counter_start)
                counter_timer += Time.deltaTime;
        
        update += Time.deltaTime;
        if (update > nextUpdatedTime)
        {
            update = 0f;
            updateGameStateClientRPC(GameState);
            checkGame();
        }
    }
    private void OnApplicationQuit()
    {
        if(PlayerManager.Instance.playerState == PlayerState.Shooter)
        {
            string joinCode = RelayManager.Instance.JoinCode;
            Debug.Log("Deleting Join Code " + joinCode);
            StartCoroutine(JoinCodeRestAPI.Delete(joinCode));
        }

        Debug.Log("Application is closed");
    }
    private void Awake()
    {
        //Instance = this;
        Debug.Log("Game Manager is called");
    }
    public bool IsGameStarted
    {
        get
        {
            return gamestart;
        }
    }

    public int[,] score = new int[10, 5];
    public void ChangeState(GameState newState)
    {
        GameState = newState;
        updateGameStateClientRPC(GameState);
        Debug.Log("State Change: " + GameState);
        switch (newState)
        {
            case GameState.GenerateGrid:
                Debug.Log("Generate Grid State");
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.SpawnNPC:
                Debug.Log("Generate Spawn State");
                SpawnManager.Instance.generateNPCs();
                break;
            case GameState.TargeterOn:
                TargetController.Instance.targetInit();
                counter_start = true;
                counter_timer = 0;
                gamestart = true;
                break;
            case GameState.WinRound:
                Debug.Log("Round Win");
                GameResult(GameState.WinRound);
                score[NumberOfGames, 0] = Logger.Instance.GreenRemove;
                score[NumberOfGames, 1] = Logger.Instance.BlueRemove;
                score[NumberOfGames, 2] = Logger.Instance.RedRemove;
                score[NumberOfGames, 3] = SpawnManager.Instance.Infected;
                score[NumberOfGames, 4] = (int)timer;
                break;
            case GameState.LoseRound:
                Debug.Log("Round Loss");
                GameResult(GameState.LoseRound);
                score[NumberOfGames, 0] = Logger.Instance.GreenRemove;
                score[NumberOfGames, 1] = Logger.Instance.BlueRemove;
                score[NumberOfGames, 2] = Logger.Instance.RedRemove;
                score[NumberOfGames, 3] = SpawnManager.Instance.Infected;
                score[NumberOfGames, 4] = (int)timer;
                break;
            case GameState.GameOver:
                Debug.Log("Game Over");
                GameResult(GameState.GameOver);
                break;
            default:
                throw new System.ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
    private void checkGame()
    {
        if (SpawnManager.Instance.Infected == 0)
        {
            counter_start = false;
            ChangeState(GameState.WinRound);
        }
        if(SpawnManager.Instance.NonInfected == 0)
        {
            counter_start = false;
            ChangeState(GameState.LoseRound);
        }
        if (NumberOfGames >= GameSettings.NUMBEROFGAMES && counter_start == false)
        {
            ChangeState(GameState.GameOver);
        }
    }
    private void GameResult(GameState gameState)
    {
        gamestart = false;

        //Artifical Delay the End Game Scene for N sec
        StartCoroutine(LoadFinishGameSessionAsynchronously(gameState));
    }
    IEnumerator LoadFinishGameSessionAsynchronously(GameState gameState)
    {
        yield return new WaitForSeconds(GameSettings.ENDGAME_SCENE_DELAY);

        SpawnManager.Instance.despawnNpcs();
        GridManager.Instance.despawnTiles();
        UIManager.Instance.roundOver(gameState);      
    }
    [ClientRpc]
    private void updateGameStateClientRPC(GameState gameState)
    {
        if (IsOwner) return;

        GameState = gameState;
    }

}

public enum GameState
{
    NoState = 0,
    GenerateGrid = 1,
    SpawnNPC = 2,
    TargeterOn = 3,
    WinRound = 4,
    LoseRound = 5,
    GameOver = 6
}
