using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using DilmerGames.Core.Singletons;
using TMPro;

public class AdvisorManager : NetworkSingleton<AdvisorManager>
{
    string[] advise = { "Pass", "Destroy", "No Instruction" };

    [SerializeField] 
    private GameObject advisorUI;

    [SerializeField]
    private TextMeshProUGUI adviseTextBox;


    public void insertAdvise(string text)
    {
        //advisorBox.GetComponent<UnityEngine.UI.Text>().text = advise;
        adviseTextBox.text = text;
    }
    [ServerRpc(RequireOwnership = false)]
    public void updateAdviseTextServerRpc(string text)
    {
        Debug.Log("Update Server");
        adviseTextBox.text = text;
    }
    [ClientRpc]
    public void updateAdviseClientRpc(string text)
    {
        if (IsOwner) return;

        adviseTextBox.text = text;
    }
    public void setAdvisorUIState(bool state)
    {
        advisorUI.SetActive(state);
    }
    public void setAdvisorTextBoxState(bool state)
    {
        adviseTextBox.gameObject.SetActive(state);
    }
    private void advisorAgent(AdvisorAgent advisorAgent)
    {
        switch (advisorAgent)
        {
            case AdvisorAgent.Pacifist:
                pacifist();
                break;
            case AdvisorAgent.Trigger:
                break;
            case AdvisorAgent.RedBiasTrigger:
                break;
            case AdvisorAgent.BlueBiasTrigger:
                break;
            case AdvisorAgent.RedNpcCollection:
                break;
            case AdvisorAgent.BlueNpcCollection:
                break;
        }
    }
    private void pacifist()
    {

    }
    public enum AdvisorAgent
    {
        Pacifist = 0,
        Trigger = 1,
        RedBiasTrigger = 2,
        BlueBiasTrigger = 3,
        RedNpcCollection = 4,
        BlueNpcCollection = 5
    }
}
