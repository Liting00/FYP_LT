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
    public TextMeshProUGUI adviseTextBox;

    public AdvisorAgent selectedAgent;

    public Roleplay roleplay;

    private float update = 0f;
    private float nextUpdatedTime = GameSettings.ADVISOR_UPDATE_TIME;

    public bool changeTargetDelay { get; set; }
    public bool AutoButtonPressed { get; set; }

    internal List<GameObject> highlightedNPCs = new List<GameObject>();

    private void Start()
    {
        //Inital Advise
        insertAdvise(AdvisorAdvice.NoAdvice);

        System.Random random = new System.Random();

        //Roleplay or Freeplay
        Array type = Enum.GetValues(typeof(Roleplay));
        roleplay = (Roleplay)type.GetValue(random.Next(type.Length));
        Debug.Log("Human Advisor is Bias Against - " + roleplay);

        //Run Advisor Agent if Single Player
        Array values = Enum.GetValues(typeof(AdvisorAgent));

        selectedAgent = (AdvisorAgent)values.GetValue(random.Next(values.Length));

        //For testing
        //selectedAgent = AdvisorAgent.BlueBias;

        Debug.Log("Ai Advisor is Bias Against - " + selectedAgent);
    }
    private void Update()
    {
        update += Time.deltaTime;
        if (update > nextUpdatedTime)
        {
            update = 0f;
            // Debug.Log("Round " + GameManager.Instance.NumberOfGames + " advisor used: " + selectedAgent);
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
                adviseTextBox.text = AdviceText(AdvisorAdvice.Shoot);
                adviseTextBox.color = Color.red;
                break;
            case AdvisorAdvice.Pass:
                adviseTextBox.text = AdviceText(AdvisorAdvice.Pass);
                adviseTextBox.color = Color.green;
                break;
            case AdvisorAdvice.NoAdvice:
                adviseTextBox.text = AdviceText(AdvisorAdvice.NoAdvice);
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
                adviseTextBox.text = AdviceText(AdvisorAdvice.Shoot);
                adviseTextBox.color = Color.red;
                break;
            case AdvisorAdvice.Pass:
                adviseTextBox.text = AdviceText(AdvisorAdvice.Pass);
                adviseTextBox.color = Color.green;
                break;
            case AdvisorAdvice.NoAdvice:
                adviseTextBox.text = AdviceText(AdvisorAdvice.NoAdvice);
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
                adviseTextBox.text = AdviceText(AdvisorAdvice.Shoot);
                adviseTextBox.color = Color.red;
                break;
            case AdvisorAdvice.Pass:
                adviseTextBox.text = AdviceText(AdvisorAdvice.Pass);
                adviseTextBox.color = Color.green;
                break;
            case AdvisorAdvice.NoAdvice:
                adviseTextBox.text = AdviceText(AdvisorAdvice.NoAdvice);
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
        if (PlayerManager.Instance.PlayerInGame == 2 ) return;

        // Change Target Delay
        if (changeTargetDelay)
        {
            insertAdvise(AdvisorAdvice.NoAdvice);
            return;
        }

        switch (advisorAgent)
        {
            case AdvisorAgent.GreenBias:
                GreenBiasAdvice();
                break;
            case AdvisorAgent.BlueBias:
                BlueBiasAdvice();
                break;
            case AdvisorAgent.GreenOnly:
                GreenOnlyAdvice();
                break;
            case AdvisorAgent.BlueOnly:
                BlueOnlyAdvice();
                break;
            case AdvisorAgent.RedTrigger:
                RedTriggerAdvice();
                break;
        }
    }

    //Shoot when there is green npc in it, no advice when red npc alone
    private void GreenBiasAdvice()
    {
        bool shootGreenNPC = false;
        bool shootRedNPC = true;

        if (SpawnManager.Instance.Infected == 0 || SpawnManager.Instance.Infected == 1)
        {
            insertAdvise(AdvisorAdvice.NoAdvice);
            return;
        }

        foreach (GameObject npc in TargetController.Instance.highlightedNPCs)
        {
            if (npc == null)
                continue;

            // Detect GreenNonHostileNPC in the killzone
            if (npc.name.Contains("GreenNonHostileNPC") && !npc.name.Contains("Infected"))
            {
                //Debug.Log("Detect Green Hostile");
                shootGreenNPC = true;
                shootRedNPC = false;
            }
            // Detect BlueNonHostileNPC in the killzone
            else if (npc.name.Contains("BlueNonHostileNPC") && !npc.name.Contains("Infected"))
            {
                //Debug.Log("Detect Blue Hostile");
                shootGreenNPC = false;
                shootRedNPC = false;
            }
        }
        //true if the tile has green npc or only red npc
        if (shootGreenNPC || shootRedNPC)
        {
            insertAdvise(AdvisorAdvice.Shoot);
            updateAdviseClientRpc(AdvisorAdvice.Shoot);
        }
        //tile does not has any green npc 
        else
        {
            insertAdvise(AdvisorAdvice.Pass);
            updateAdviseClientRpc(AdvisorAdvice.Pass);
        }
    }

    //shoot when there is blue npc in it, no advice when red npc is alone
    private void BlueBiasAdvice()
    {
        bool shootBlueNPC = false;
        bool shootRedNPC = true;

        if (SpawnManager.Instance.Infected == 0 || SpawnManager.Instance.Infected == 1)
        {
            insertAdvise(AdvisorAdvice.NoAdvice);
            return;
        }

        foreach (GameObject npc in TargetController.Instance.highlightedNPCs)
        {
            if (npc == null)
                continue;

            // Detect BlueNonHostileNPC in the killzone
            if (npc.name.Contains("BlueNonHostileNPC") && !npc.name.Contains("Infected"))
            {
                //Debug.Log("Detect Blue Hostile");
                shootBlueNPC = true;
                shootRedNPC = false;
            }
            // Detect GreenNonHostileNPC in the killzone
            else if (npc.name.Contains("GreenNonHostileNPC") && !npc.name.Contains("Infected"))
            {
                //Debug.Log("Detect Green Hostile");
                shootBlueNPC = false;
                shootRedNPC = false;
            }
        }
        //true if the tile has blue npc or only red npc
        if (shootBlueNPC || shootRedNPC)
        {
            insertAdvise(AdvisorAdvice.Shoot);
            updateAdviseClientRpc(AdvisorAdvice.Shoot);
            
        }
        //tile does not contain any blue npc
        else
        {
            insertAdvise(AdvisorAdvice.Pass);
            updateAdviseClientRpc(AdvisorAdvice.Pass);
        }
    }
    private void GreenOnlyAdvice()
    {
        bool shootGreenNPC = false;
        bool shootRedNPC = true;

        if (SpawnManager.Instance.Infected == 0 || SpawnManager.Instance.Infected == 1)
        {
            insertAdvise(AdvisorAdvice.NoAdvice);
            return;
        }

        foreach (GameObject npc in TargetController.Instance.highlightedNPCs)
        {
            if (npc == null)
                continue;

            // Detect GreenNonHostileNPC in the killzone
            if (npc.name.Contains("BlueNonHostileNPC") && !npc.name.Contains("Infected"))
            {
                if (npc.name.Contains("GreenNonHostileNPC") && !npc.name.Contains("Infected"))
                {
                    //Debug.Log("Detect Green Hostile");
                    shootGreenNPC = true;
                    shootRedNPC = false;
                }
                else
                {
                    //Debug.Log("Detect Green Hostile");
                    shootGreenNPC = false;
                    shootRedNPC = false;
                }
            }
            // Detect BlueNonHostileNPC in the killzone
            else if (npc.name.Contains("BlueNonHostileNPC") && !npc.name.Contains("Infected"))
            {
                //Debug.Log("Detect Blue Hostile");
                shootGreenNPC = false;
                shootRedNPC = false;
            }
        }
        //true if the tile only has green npc 
        if (shootGreenNPC || shootRedNPC)
        {
            insertAdvise(AdvisorAdvice.Shoot);
            updateAdviseClientRpc(AdvisorAdvice.Shoot);
        }
        //tile contain blue npc or only red npc 
        else
        {
            insertAdvise(AdvisorAdvice.Pass);
            updateAdviseClientRpc(AdvisorAdvice.Pass);
        }
    }

    //Shoot when there is only blue in it
    private void BlueOnlyAdvice()
    {
        bool shootBlueNPC = false;
        bool shootRedNPC = true;

        if (SpawnManager.Instance.Infected == 0 || SpawnManager.Instance.Infected == 1)
        {
            insertAdvise(AdvisorAdvice.NoAdvice);
            return;
        }

        foreach (GameObject npc in TargetController.Instance.highlightedNPCs)
        {
            if (npc == null)
                continue;

            // Detect GreenNonHostileNPC in the killzone
            if (npc.name.Contains("GreenNonHostileNPC") && !npc.name.Contains("Infected"))
            {
                if (npc.name.Contains("BlueNonHostileNPC") && !npc.name.Contains("Infected"))
                {
                    //Debug.Log("Detect Blue Hostile");
                    shootBlueNPC = true;
                    shootRedNPC = false;
                }
                else
                {
                    //Debug.Log("Detect Blue Hostile");
                    shootBlueNPC = false;
                    shootRedNPC = false;
                }
            }
            // Detect BlueNonHostileNPC in the killzone
            else if (npc.name.Contains("GreenNonHostileNPC") && !npc.name.Contains("Infected"))
            {
                //Debug.Log("Detect Blue Hostile");
                shootBlueNPC = false;
                shootRedNPC = false;
            }
        }
        //true if the tile only has blue npc 
        if (shootBlueNPC || shootRedNPC)
        {
            insertAdvise(AdvisorAdvice.Shoot);
            updateAdviseClientRpc(AdvisorAdvice.Shoot);
        }
        //tile contain green npc or only red npc 
        else
        {
            insertAdvise(AdvisorAdvice.Pass);
            updateAdviseClientRpc(AdvisorAdvice.Pass);
        }
    }

    //Only shoot when red is alone
    private void RedTriggerAdvice()
    {
        bool shootRedNPC = true;

        if (SpawnManager.Instance.Infected == 0 || SpawnManager.Instance.Infected == 1)
        {
            insertAdvise(AdvisorAdvice.NoAdvice);
            return;
        }

        foreach (GameObject npc in TargetController.Instance.highlightedNPCs)
        {
            if (npc == null)
                continue;

            // Detect BlueNonHostileNPC in the killzone
            if (npc.name.Contains("BlueNonHostileNPC") && !npc.name.Contains("Infected"))
            {
                //Debug.Log("Detect Blue Hostile");
                shootRedNPC = false;
            }
            // Detect GreenNonHostileNPC in the killzone
            else if (npc.name.Contains("GreenNonHostileNPC") && !npc.name.Contains("Infected"))
            {
                //Debug.Log("Detect Green Hostile");
                shootRedNPC = false;
            }
        }
        //true if the tile only has red npc
        if (shootRedNPC)
        {
            insertAdvise(AdvisorAdvice.Shoot);
            updateAdviseClientRpc(AdvisorAdvice.Shoot);
        }
        //if the tile contain green or blue npc
        else
        {
            insertAdvise(AdvisorAdvice.Pass);
            updateAdviseClientRpc(AdvisorAdvice.Pass);
        }
    }

    public string AdviceText(AdvisorAdvice advisorAdvice)
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
    GreenBias = 0,
    BlueBias = 1,
    RedTrigger = 2,
    GreenOnly = 3,
    BlueOnly = 4,
    AgentAuto
}
public enum AdvisorAdvice
{
    Pass,
    Shoot,
    NoAdvice
}
public enum Roleplay
{
    FreePlay,
    GreenBias,
    BlueBias,
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

/*
//Only shot when there is green in it
private void GreenTriggerAdvice()
{
    bool shootGreenNPC = false;

    if (SpawnManager.Instance.Infected == 0)
    {
        insertAdvise(AdvisorAdvice.NoAdvice);
        return;
    }

    foreach (GameObject npc in TargetController.Instance.highlightedNPCs)
    {
        if (npc == null)
            continue;

        // Detect GreenNonHostileNPC in the killzone
        if (npc.name.Contains("GreenNonHostileNPC") && !npc.name.Contains("Infected"))
        {
            //Debug.Log("Detect Green Hostile");
            shootGreenNPC = true;
        }
        // Detect BlueNonHostileNPC in the killzone
        else if (npc.name.Contains("BlueNonHostileNPC") && !npc.name.Contains("Infected"))
        {
            //Debug.Log("Detect Blue Hostile");
            shootGreenNPC = false;
        }
    }
    //true if the tile has green npc in it
    if (shootGreenNPC)
    {
        insertAdvise(AdvisorAdvice.Shoot);
        updateAdviseClientRpc(AdvisorAdvice.Shoot);
    }
    //tile does not contain any green npc or only red npc
    else
    {
        insertAdvise(AdvisorAdvice.Pass);
        updateAdviseClientRpc(AdvisorAdvice.Pass);
    }
}

//shot only when there is blue in it
private void BlueTriggerAdvice()
{
    bool shootBlueNPC = false;

    if (SpawnManager.Instance.Infected == 0)
    {
        insertAdvise(AdvisorAdvice.NoAdvice);
        return;
    }

    foreach (GameObject npc in TargetController.Instance.highlightedNPCs)
    {
        if (npc == null)
            continue;

        // Detect BlueNonHostileNPC in the killzone
        if (npc.name.Contains("BlueNonHostileNPC") && !npc.name.Contains("Infected"))
        {
            //Debug.Log("Detect Blue Hostile");
            shootBlueNPC = true;
        }
        // Detect GreenNonHostileNPC in the killzone
        else if (npc.name.Contains("GreenNonHostileNPC") && !npc.name.Contains("Infected"))
        {
            //Debug.Log("Detect Green Hostile");
            shootBlueNPC = false;
        }
    }
    //true if the tile has blue npc
    if (shootBlueNPC)
    {
        insertAdvise(AdvisorAdvice.Shoot);
        updateAdviseClientRpc(AdvisorAdvice.Shoot);
    }
    //if the tile does not contain any blue npc or only red npc
    else
    {
        insertAdvise(AdvisorAdvice.Pass);
        updateAdviseClientRpc(AdvisorAdvice.Pass);
    }
}
*/
//Shoot when there is only green in it
