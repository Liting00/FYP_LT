using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class UIManager : NetworkBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playersInGameText;

    private void Awake()
    {
        Cursor.visible = true;
    }

    private void OnGUI()
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
            
    }
    private void Update()
    {
    }
    private void Start()
    {
        
    }
}
