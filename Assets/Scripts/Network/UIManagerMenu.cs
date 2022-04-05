using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class UIManagerMenu : MonoBehaviour
{
    [SerializeField]
    private Button startServerButton;

    [SerializeField]
    private Button startHostButton;

    [SerializeField]
    private Button startClientButton;

    [SerializeField]
    private Button startGameButton;
    
    //[SerializeField]
    //private TextMeshProUGUI playersInGameText;

    private bool hasServerStarted;

    private void Awake()
    {
        Cursor.visible = true;
    }
    private void Update()
    {
        //playersInGameText.text = $"Player in Game: {PlayerManager.Instance.PlayerInGame}";
    }
    private void Start()
    {
        startHostButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartHost())
            {
                Debug.Log("Host Started...");
            }
            else
            {
                Debug.Log("Host could not be Started...");
            }
        });
        startClientButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartClient())
            {
                Debug.Log("Client Started...");
            }
            else
            {
                Debug.Log("Client could not be Started...");
            }
        });
        startServerButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartServer())
            {
                Debug.Log("Server Started...");
            }
            else
            {
                Debug.Log("Server could not be Started...");
            }
        });
        NetworkManager.Singleton.OnServerStarted += () =>
        {
            hasServerStarted = true;
        };
        startGameButton.onClick.AddListener(() =>
        {
            /*if (!hasServerStarted)
            {
                Debug.Log("Server has not yet started");
                return;
            }*/
            SceneManager.LoadScene("Grid Environment");
        });

    }

}
