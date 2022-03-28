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
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(GameState.GenerateGrid);
    }

    public void ChangeState(GameState newState)
    {
        GameState = newState;
        switch (newState)
        {
            case GameState.GenerateGrid:
                //Debug.Log("Generate Grid State");
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.SpawnNPC:
                //Debug.Log("Generate Spawn State");
                SpawnManager.Instance.spawnObject();
                break;
            default:
                throw new System.ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

public enum GameState
{
    GenerateGrid = 0,
    SpawnNPC = 1,
    SpawnRedNPC = 2
}