using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinCodeLobbyManager : MonoBehaviour
{
    public static JoinCodeLobbyManager instance;

    public GameObject joinCodeEntryPrefab;
    public GameObject JoinCodeListContent;
    public List<GameObject> listOfJoinCode = new List<GameObject>();

    private void Awake()
    {
        if(instance == null) { instance = this; }
    }
    public void DestroyLobbies()
    {
        foreach(GameObject item in listOfJoinCode)
        {
            Destroy(item);
        }
        listOfJoinCode.Clear();
    }
    public void DisplayLobbies(List<JoinCode> playerList)
    {
        Debug.Log("Displaying Lobbies");

        for(int i = 0; i < playerList.Count(); i++)
        {
            GameObject createdItem = Instantiate(joinCodeEntryPrefab);

            createdItem.GetComponent<JoinGameEntry>().joinCode = playerList[i].joinCode;
            createdItem.GetComponent<JoinGameEntry>().enterBtn.GetComponentInChildren<TextMeshProUGUI>().text = playerList[i].joinCode;

            createdItem.transform.SetParent(JoinCodeListContent.transform);
            createdItem.transform.localScale = Vector3.one;
            listOfJoinCode.Add(createdItem);
        }
    }
    public void DestroyLobby()
    {
        foreach (GameObject item in listOfJoinCode)
        {
            if (item == null)
                continue;

            Destroy(item.gameObject);
        }
    }
}
