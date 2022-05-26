using DilmerGames.Core.Singletons;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Collections;

public class LevelManager : NetworkSingleton<LevelManager>
{
    public static new LevelManager Instance;


    [SerializeField] private string menu = "Menu";
    [SerializeField] private string newGame = "Grid Environment";

    public PlayerState playerState;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        DontDestroyOnLoad(this);
    }
    public void NewGame(PlayerState playerState)
    {
        this.playerState = playerState;

        SceneManager.LoadScene(newGame);

        if(playerState == PlayerState.Shooter)
        {
            Debug.Log("Player Start Game");
        }
        else if(playerState == PlayerState.Advisor)
        {
            //do something
        }
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(menu);
    }
    public enum PlayerState
    {
        Shooter = 0,
        Advisor = 1
    }

}
