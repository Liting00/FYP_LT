using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class UIManager : NetworkBehaviour
{
    private TextMesh playerInGameText;

    private void Awake()
    {
        Cursor.visible = true;
    }

    private void OnGUI()
    {
        GUIStyle customButton = new GUIStyle("button");
        customButton.fontSize = 50;
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
