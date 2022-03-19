using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    //[SerializeField] private string menu = "Menu";
    [SerializeField] private string newGame = "GridEnvironment";

    private void Awake()
    {
        Instance = this;
    }

    public void NewGame()
    {
        SceneManager.LoadScene(newGame);
    }


}
