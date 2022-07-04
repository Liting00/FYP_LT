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

    private void Update()
    {
        if (!gamestart)
            return;
        
        update += Time.deltaTime;
        if (update > nextUpdatedTime)
        {
            update = 0f;
            checkGame();
        }
    }
    private void Awake()
    {
        Instance = this;
        Debug.Log("Game Manager is called");
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
                gamestart = true;
                break;
            case GameState.RoundOver:
                roundOver();
                break;
            default:
                throw new System.ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
    private void checkGame()
    {
        if (SpawnManager.Instance.Infected == 0)
        {
            ChangeState(GameState.RoundOver);
        }
    }
    private void roundOver()
    {
        gamestart = false;

        //delay the game end in N sec
        StartCoroutine(LoadFinishGameAsynchronously());
    }
    IEnumerator LoadFinishGameAsynchronously()
    {
        yield return new WaitForSeconds(GameSettings.ENDGAME_DELAY);

        SpawnManager.Instance.despawnNpcs();
        GridManager.Instance.despawnTiles();
        UIManager.Instance.roundOver();
    }
}

public enum GameState
{
    GenerateGrid = 0,
    SpawnNPC = 1,
    Targeter = 2,
    RoundOver = 3,
    GameOver = 4
}