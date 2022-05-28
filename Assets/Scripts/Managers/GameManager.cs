using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState;


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
                break;
            default:
                throw new System.ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
}

public enum GameState
{
    GenerateGrid = 0,
    SpawnNPC = 1,
    Targeter = 2,
}