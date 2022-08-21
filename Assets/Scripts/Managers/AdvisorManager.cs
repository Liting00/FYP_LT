using UnityEngine;
using Unity.Netcode;
using Network.Singletons;
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

    public bool changeTargetDelay {get; set;}

    internal List<GameObject> highlightedNPCs = new List<GameObject>();

    public int count { get; set; }

    private void Start()
    {
        //Inital Advise
        insertAdvise(AdvisorAdvice.NoAdvice);

        //Run Advisor Agent if Single Player
        Array values = Enum.GetValues(typeof(AdvisorAgent));
        System.Random random = new System.Random();

        selectedAgent = (AdvisorAgent)values.GetValue(random.Next(values.Length));

        //For testing purpose
        //selectedAgent = AdvisorAgent.GreenBiasTrigger;

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
    public void changeAdvisor()
    {
        Array values = Enum.GetValues(typeof(AdvisorAgent));
        System.Random random = new System.Random();

        selectedAgent = (AdvisorAgent)values.GetValue(random.Next(values.Length));

        Debug.Log(selectedAgent);
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
        //disable advisor if host is interacting with real player
        if (PlayerManager.Instance.PlayerInGame == 2) return;

        // Change Target Delay
        if (changeTargetDelay)
        {
            insertAdvise(AdvisorAdvice.NoAdvice);
            return;
        }

        switch (advisorAgent)
        {
            /*
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
            */

            case AdvisorAgent.GreenBias:
                GreenBiasAdvice();
                break;
            case AdvisorAgent.BlueBias:
                BlueBiasAdvice();
                break;
            case AdvisorAgent.GreenTrigger:
                GreenTriggerAdvice();
                break;
            case AdvisorAgent.BlueTrigger:
                BlueTriggerAdvice();
                break;
            case AdvisorAgent.RedTrigger:
                RedTriggerAdvice();
                break;
            case AdvisorAgent.Auto:
                AutoAdvice();
                break;
        }
    }
    /*
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
        bool shootGreenNPC = false;
        bool shootOnlyHostileNpc = false;

        if(SpawnManager.Instance.Infected == 0)
        {
            insertAdvise(AdvisorAdvice.NoAdvice);
            return;
        }

        foreach (GameObject npc in TargetController.Instance.highlightedNPCs)
        {
            if (npc == null)
                continue;

            // Detect Single infected in the killzone
            if ((npc.name.Contains("Hostile") || npc.name.Contains("Infected"))&& !npc.name.Contains("Non"))
            {
                //Debug.Log("Detect Only Infected");
                shootOnlyHostileNpc = true;
            }
            // Detect GreenNonHostileNPC in the killzone
            if (npc.name.Contains("GreenNonHostileNPC") && !npc.name.Contains("Infected"))
            {
                //Debug.Log("Detect Green Hostile");
                shootGreenNPC = true;
                shootOnlyHostileNpc = false;
            }
            // Detect BlueNonHostileNPC in the killzone
            else if (npc.name.Contains("BlueNonHostileNPC") && !npc.name.Contains("Infected"))
            {
                //Debug.Log("Detect Blue Hostile");
                shootGreenNPC = false;
                shootOnlyHostileNpc = false;
            }
        }
        //true if the tile only has a Hostile Npc or green npc
        if(shootOnlyHostileNpc || shootGreenNPC)
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

    private void BlueBiasTrigger()
    {
        bool shootGreenNPC = false;
        bool shootOnlyHostileNpc = false;

        if (SpawnManager.Instance.Infected == 0)
        {
            insertAdvise(AdvisorAdvice.NoAdvice);
            return;
        }

        foreach (GameObject npc in TargetController.Instance.highlightedNPCs)
        {
            if (npc == null)
                continue;

            // Detect Single infected in the killzone
            if ((npc.name.Contains("Hostile") || npc.name.Contains("Infected")) && !npc.name.Contains("Non"))
            {
                Debug.Log("Detect Only Infected");
                shootOnlyHostileNpc = true;
            }
            // Detect BlueNonHostileNPC in the killzone
            if (npc.name.Contains("BlueNonHostileNPC") && !npc.name.Contains("Infected"))
            {
                Debug.Log("Detect Blue Hostile");
                shootGreenNPC = true;
                shootOnlyHostileNpc = false;
            }
            // Detect GreenNonHostileNPC in the killzone
            else if (npc.name.Contains("GreenNonHostileNPC") && !npc.name.Contains("Infected"))
            {
                Debug.Log("Detect Green Hostile");
                shootGreenNPC = false;
                shootOnlyHostileNpc = false;
            }
        }
        //true if the tile only has a Hostile Npc or green npc
        if (shootOnlyHostileNpc || shootGreenNPC)
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
    }*/

    private void GreenBiasAdvice()
    {
        bool shootGreenNPC = false;
        bool shootOnlyHostileNpc = false;

        if (SpawnManager.Instance.Infected == 0)
        {
            insertAdvise(AdvisorAdvice.NoAdvice);
            return;
        }

        foreach (GameObject npc in TargetController.Instance.highlightedNPCs)
        {
            if (npc == null)
                continue;

            // Detect Single infected in the killzone
            if ((npc.name.Contains("Hostile") || npc.name.Contains("Infected")) && !npc.name.Contains("Non"))
            {
                //Debug.Log("Detect Only Infected");
                shootOnlyHostileNpc = true;
            }
            // Detect GreenNonHostileNPC in the killzone
            if (npc.name.Contains("GreenNonHostileNPC") && !npc.name.Contains("Infected"))
            {
                //Debug.Log("Detect Green Hostile");
                shootGreenNPC = true;
                shootOnlyHostileNpc = false;
            }
            // Detect BlueNonHostileNPC in the killzone
            else if (npc.name.Contains("BlueNonHostileNPC") && !npc.name.Contains("Infected"))
            {
                //Debug.Log("Detect Blue Hostile");
                shootGreenNPC = false;
                shootOnlyHostileNpc = false;
            }
        }
        //true if the tile only has a Hostile Npc or green npc
        if (shootOnlyHostileNpc || shootGreenNPC)
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

    private void BlueBiasAdvice()
    {
        
    }

    private void GreenTriggerAdvice()
    {
        
    }

    private void BlueTriggerAdvice()
    {
        
    }

    private void RedTriggerAdvice()
    {
        
    }

    private void AutoAdvice()
    {
        
    }
    private string Advise(AdvisorAdvice advisorAdvice)
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

}
public enum AdvisorAgent
{
    /*
    Pacifist = 0,
    Trigger = 1,
    GreenBiasTrigger = 2,
    BlueBiasTrigger = 3,
    GreenNpcCollection = 4,
    BlueNpcCollection = 5,
    auto = 6
    */
    GreenBias = 0,
    BlueBias = 1,
    GreenTrigger = 2,
    BlueTrigger = 3,
    RedTrigger = 4,
    Auto = 5
}
public enum AdvisorAdvice
{
    Pass,
    Shoot,
    NoAdvice
}