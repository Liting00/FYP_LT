using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState;

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
            checkGame();
        }
    }
    private void OnApplicationQuit()
    {
        //TODO: Delete (Done)
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
        Instance = this;
        Debug.Log("Game Manager is called");
    }
    public bool IsGameStarted
    {
        get
        {
            return gamestart;
        }
    }

    public void ChangeState(GameState newState)
    {
        GameState = newState;
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
            case GameState.Targeter:
                TargetController.Instance.targetInit();
                counter_start = true;
                counter_timer = 0;
                gamestart = true;
                break;
            case GameState.WinRound:
                Debug.Log("Round Win");
                GameResult(GameState.WinRound);
                break;
            case GameState.LoseRound:
                Debug.Log("Round Loss");
                GameResult(GameState.LoseRound);
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

        //delay the End Game in N sec
        StartCoroutine(LoadFinishGameSessionAsynchronously(gameState));
    }
    IEnumerator LoadFinishGameSessionAsynchronously(GameState gameState)
    {
        yield return new WaitForSeconds(GameSettings.ENDGAME_DELAY);

        SpawnManager.Instance.despawnNpcs();
        GridManager.Instance.despawnTiles();
        UIManager.Instance.roundOver(gameState);      
    }
}

public enum GameState
{
    GenerateGrid = 0,
    SpawnNPC = 1,
    Targeter = 2,
    WinRound = 3,
    LoseRound = 4,
    GameOver = 5
}
