using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class UIManager : NetworkBehaviour
{
    [SerializeField]
    private Button startServerButton;

    [SerializeField]
    private Button startHostButton;

    [SerializeField]
    private Button startClientButton;

    [SerializeField]
    private Button StartGameButton;

    [SerializeField]
    private TextMeshProUGUI playersInGameText;

    private bool hasServerStarted = false;

    private void Awake()
    {
        Cursor.visible = true;
    }

    /*private void OnGUI()
    {
        if (GUILayout.Button("Client", GUILayout.Width(100), GUILayout.Height(100)))
        {
            NetworkManager.Singleton.StartClient();
            Debug.Log("Client Started...");
        }

        if (GUILayout.Button("Host", GUILayout.Width(100), GUILayout.Height(100)))
        {
            NetworkManager.Singleton.StartHost();
            Debug.Log("Host Started...");
        }
            
    }*/
    private void Update()
    {
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
        StartGameButton.onClick.AddListener(() =>
        {
            if (!hasServerStarted)
            {
                Debug.Log("Server has not yet started");
                return;
            }
            GridManager.Instance.GenerateGrid();
            SpawnManager.Instance.spawnObject();
        });

    }
}
