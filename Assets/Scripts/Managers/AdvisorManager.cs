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
    private float nextUpdatedTime = 0.1f;

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
            string advise = Advise(AdvisorAdvice.Pass);
            insertAdvise(advise);
            updateAdviseClientRpc(advise);
        }
        else
        {
            string advise = Advise(AdvisorAdvice.NoAdvice);
            insertAdvise(advise);
            updateAdviseClientRpc(advise);
        }  
    }
    private void Trigger()
    {
        if (highlightedNPCs.Count >= 1)
        {
            string advise = Advise(AdvisorAdvice.Shoot);
            insertAdvise(advise);
            updateAdviseClientRpc(advise);
        }
        else
        {
            string advise = Advise(AdvisorAdvice.Pass);
            insertAdvise(advise);
            updateAdviseClientRpc(advise);
        }
    }
    private void GreenBiasTrigger()
    {
        int biasCount = 0;

        foreach (GameObject npc in highlightedNPCs)
        {
            if (npc == null)
                break;

            if (npc.name.Contains("Green"))
                biasCount++;
        }
        if (biasCount >= 1)
        {
            string advise = Advise(AdvisorAdvice.Shoot);
            insertAdvise(advise);
            updateAdviseClientRpc(advise);
        }
        else
        {
            string advise = Advise(AdvisorAdvice.Pass);
            insertAdvise(advise);
            updateAdviseClientRpc(advise);
        }
    }
    private void BlueBiasTrigger()
    {
        int biasCounter = 0;

        foreach (GameObject npc in highlightedNPCs)
        {
            if (npc == null)
                break;

            if (npc.name.Contains("Blue"))
                biasCounter++;
        }
        if (biasCounter >= 1)
        {
            string advise = Advise(AdvisorAdvice.Shoot);
            insertAdvise(advise);
            updateAdviseClientRpc(advise);
        }
        else
        {
            string advise = Advise(AdvisorAdvice.Pass);
            insertAdvise(advise);
            updateAdviseClientRpc(advise);
        }
    }
    private void GreenBiasCollection()
    {
        int biasCount = 0;
        int collection = 2;

        foreach (GameObject npc in highlightedNPCs)
        {
            if (npc == null)
                break;

            if (npc.name.Contains("Green"))
                biasCount++;
        }
        if (biasCount >= collection)
        {
            string advise = Advise(AdvisorAdvice.Shoot);
            insertAdvise(advise);
            updateAdviseClientRpc(advise);
        }
        else
        {
            string advise = Advise(AdvisorAdvice.Pass);
            insertAdvise(advise);
            updateAdviseClientRpc(advise);
        }
    }
    private void BlueBiasCollection()
    {
        int biasCounter = 0;
        int collection = 2;

        foreach (GameObject npc in highlightedNPCs)
        {
            if (npc == null)
                break;

            if (npc.name.Contains("Blue"))
                biasCounter++;
        }
        if (biasCounter >= collection)
        {
            string advise = Advise(AdvisorAdvice.Shoot);
            insertAdvise(advise);
            updateAdviseClientRpc(advise);
        }
        else
        {
            string advise = Advise(AdvisorAdvice.Pass);
            insertAdvise(advise);
            updateAdviseClientRpc(advise);
        }
    }
    public string Advise(AdvisorAdvice advisorAdvice)
    {
        string advise;

        if (advisorAdvice == AdvisorAdvice.NoAdvice)
            advise = "No Advise";
        else if (advisorAdvice == AdvisorAdvice.Pass)
            advise = "Pass";
        else if (advisorAdvice == AdvisorAdvice.Shoot)
            advise = "Shoot";
        else
            advise = "No Advise";

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
