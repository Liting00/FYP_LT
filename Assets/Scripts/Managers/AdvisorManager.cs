using UnityEngine;
using Unity.Netcode;
using DilmerGames.Core.Singletons;
using TMPro;
using System;
using System.Collections.Generic;

public class AdvisorManager : NetworkSingleton<AdvisorManager>
{
    [SerializeField] 
    private GameObject advisorUI;

    [SerializeField]
    private TextMeshProUGUI adviseTextBox;

    private AdvisorAgent selectedAgent;

    private float update = 0f;
    private float nextUpdatedTime = GameSettings.ADVISOR_UPDATE_TIME;

    internal List<GameObject> highlightedNPCs = new List<GameObject>();

    private void Start()
    {
        //Run Advisor Agent if Single Player
        Array values = Enum.GetValues(typeof(AdvisorAgent));
        System.Random random = new System.Random();

        selectedAgent = (AdvisorAgent)values.GetValue(random.Next(values.Length));

        //For testing purpose
        selectedAgent = AdvisorAgent.GreenBiasTrigger;

        Debug.Log(selectedAgent);
    }
    private void Update()
    {
        update += Time.deltaTime;
        if (update > nextUpdatedTime)
        {
            update = 0f;
            advisorAgent(selectedAgent);
        }
    }
    public void insertAdvise(AdvisorAdvice advisorAdvice)
    {
        //advisorBox.GetComponent<UnityEngine.UI.Text>().text = advise;
        switch (advisorAdvice)
        {
            case AdvisorAdvice.Shoot:
                adviseTextBox.text = Advise(AdvisorAdvice.Shoot);
                adviseTextBox.color = Color.red;
                break;
            case AdvisorAdvice.Pass:
                adviseTextBox.text = Advise(AdvisorAdvice.Pass);
                adviseTextBox.color = Color.green;
                break;
            case AdvisorAdvice.NoAdvice:
                adviseTextBox.text = Advise(AdvisorAdvice.NoAdvice);
                adviseTextBox.color = Color.white;
                break;
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void updateAdviseTextServerRpc(AdvisorAdvice advisorAdvice)
    {
        Debug.Log("Update Server");

        switch (advisorAdvice)
        {
            case AdvisorAdvice.Shoot:
                adviseTextBox.text = Advise(AdvisorAdvice.Shoot);
                adviseTextBox.color = Color.red;
                break;
            case AdvisorAdvice.Pass:
                adviseTextBox.text = Advise(AdvisorAdvice.Pass);
                adviseTextBox.color = Color.green;
                break;
            case AdvisorAdvice.NoAdvice:
                adviseTextBox.text = Advise(AdvisorAdvice.NoAdvice);
                adviseTextBox.color = Color.white;
                break;
        }
    }
    [ClientRpc]
    public void updateAdviseClientRpc(AdvisorAdvice advisorAdvice)
    {
        if (IsOwner) return;

        switch (advisorAdvice)
        {
            case AdvisorAdvice.Shoot:
                adviseTextBox.text = Advise(AdvisorAdvice.Shoot);
                adviseTextBox.color = Color.red;
                break;
            case AdvisorAdvice.Pass:
                adviseTextBox.text = Advise(AdvisorAdvice.Pass);
                adviseTextBox.color = Color.green;
                break;
            case AdvisorAdvice.NoAdvice:
                adviseTextBox.text = Advise(AdvisorAdvice.NoAdvice);
                adviseTextBox.color = Color.white;
                break;
        }
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
                Pacifist();
                break;
            case AdvisorAgent.Trigger:
                Trigger();
                break;
            case AdvisorAgent.GreenBiasTrigger:
                GreenBiasTrigger();
                break;
            case AdvisorAgent.BlueBiasTrigger:
                BlueBiasTrigger();
                break;
            case AdvisorAgent.GreenNpcCollection:
                GreenBiasCollection();
                break;
            case AdvisorAgent.BlueNpcCollection:
                BlueBiasCollection();
                break;
        }
    }
    private void Pacifist()
    {
        if (highlightedNPCs.Count >= 1)
        {
            insertAdvise(AdvisorAdvice.Pass);
            updateAdviseClientRpc(AdvisorAdvice.Pass);
        }
        else
        {
            insertAdvise(AdvisorAdvice.NoAdvice);
            updateAdviseClientRpc(AdvisorAdvice.NoAdvice);
        }  
    }
    private void Trigger()
    {
        if (highlightedNPCs.Count >= 1)
        {
            insertAdvise(AdvisorAdvice.Shoot);
            updateAdviseClientRpc(AdvisorAdvice.Shoot);
        }
        else
        {
            insertAdvise(AdvisorAdvice.Pass);
            updateAdviseClientRpc(AdvisorAdvice.Pass);
        }
    }
    //Bias to Green, Kill Green upon entering zone of influence
    private void GreenBiasTrigger()
    {
        int biasCounter = 0;

        foreach (GameObject npc in TargetController.Instance.highlightedNPCs)
        {
            if (npc == null)
                continue;

            if (npc.name.Contains("Green") && !npc.name.Contains("Infected"))
            {
                //Debug.Log(npc.name);
                biasCounter++;
            }
        }
        if(highlightedNPCs.Count == 1 && highlightedNPCs[0].gameObject != null && highlightedNPCs[0].name.Contains("Hostile"))
        {
            insertAdvise(AdvisorAdvice.Shoot);
            updateAdviseClientRpc(AdvisorAdvice.Shoot);
        }
        else if (biasCounter >= 1)
        {
            insertAdvise(AdvisorAdvice.Shoot);
            updateAdviseClientRpc(AdvisorAdvice.Shoot);
        }
        else if(SpawnManager.Instance.Infected <= 1)
        {
            insertAdvise(AdvisorAdvice.NoAdvice);
            updateAdviseClientRpc(AdvisorAdvice.NoAdvice);
        }
        else
        {
            insertAdvise(AdvisorAdvice.Pass);
            updateAdviseClientRpc(AdvisorAdvice.Pass);
        }
    }
    private void BlueBiasTrigger()
    {
        int biasCounter = 0;

        foreach (GameObject npc in TargetController.Instance.highlightedNPCs)
        {
            if (npc == null)
                continue;

            if (npc.name.Contains("Blue") && !npc.name.Contains("Infected"))
            {
                //Debug.Log(npc.name);
                biasCounter++;
            }
        }
        if (highlightedNPCs.Count == 1 && highlightedNPCs[0].name.Contains("Hostile") && highlightedNPCs[0] != null)
        {
            insertAdvise(AdvisorAdvice.Shoot);
            updateAdviseClientRpc(AdvisorAdvice.Shoot);
        }
        else if (biasCounter >= 1)
        {
            insertAdvise(AdvisorAdvice.Shoot);
            updateAdviseClientRpc(AdvisorAdvice.Shoot);
        }
        else if (SpawnManager.Instance.Infected <= 1)
        {
            insertAdvise(AdvisorAdvice.NoAdvice);
            updateAdviseClientRpc(AdvisorAdvice.NoAdvice);
        }
        else
        {
            insertAdvise(AdvisorAdvice.Pass);
            updateAdviseClientRpc(AdvisorAdvice.Pass);
        }
    }
    private void GreenBiasCollection()
    {
        int biasCounter = 0;
        int collection = 2;

        foreach (GameObject npc in highlightedNPCs)
        {
            if (npc == null)
                continue;

            if (npc.name.Contains("Green"))
                biasCounter++;
        }
        if (biasCounter >= collection)
        {
            insertAdvise(AdvisorAdvice.Shoot);
            updateAdviseClientRpc(AdvisorAdvice.Shoot);
        }
        else
        {
            insertAdvise(AdvisorAdvice.Pass);
            updateAdviseClientRpc(AdvisorAdvice.Pass);
        }
    }
    private void BlueBiasCollection()
    {
        int biasCounter = 0;
        int collection = 2;

        foreach (GameObject npc in highlightedNPCs)
        {
            if (npc == null)
                continue;

            if (npc.name.Contains("Blue"))
                biasCounter++;
        }
        if (biasCounter >= collection)
        {
            insertAdvise(AdvisorAdvice.Shoot);
            updateAdviseClientRpc(AdvisorAdvice.Shoot);
        }
        else
        {
            insertAdvise(AdvisorAdvice.Pass);
            updateAdviseClientRpc(AdvisorAdvice.Pass);
        }
    }
    public string Advise(AdvisorAdvice advisorAdvice)
    {
        string advise;

        if (advisorAdvice == AdvisorAdvice.NoAdvice)
            advise = GameSettings.NO_ADVISE_TEXT;
        else if (advisorAdvice == AdvisorAdvice.Pass)
            advise = GameSettings.GREEN_BUTTON_TEXT;
        else if (advisorAdvice == AdvisorAdvice.Shoot)
            advise = GameSettings.RED_BUTTON_TEXT;
        else
            advise = GameSettings.NO_ADVISE_TEXT;

        return advise;
    }
    public enum AdvisorAgent
    {
        Pacifist = 0,
        Trigger = 1,
        GreenBiasTrigger = 2,
        BlueBiasTrigger = 3,
        GreenNpcCollection = 4,
        BlueNpcCollection = 5
    }
    public enum AdvisorAdvice
    {
        Pass,
        Shoot,
        NoAdvice
    }
    
}
