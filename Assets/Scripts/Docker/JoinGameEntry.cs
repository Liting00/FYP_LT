using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoinGameEntry : MonoBehaviour
{
    internal string joinCode;
    private TextMeshProUGUI joinCodeText;

    public void setJoinCodeData()
    {
        joinCodeText.text = joinCode;
    }

    public void joinGame()
    {   

    }
}
