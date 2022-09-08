using Network.Singletons;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Logger : NetworkSingleton<Logger>
{
    public static new Logger Instance;

    //JSON Logger
    ParticipantExpInfo participantExpInfo = null;

    // Total accumulate
    private NetworkVariable<int> totalGreenRemove = new NetworkVariable<int>();
    private NetworkVariable<int> totalBlueRemove = new NetworkVariable<int>();
    private NetworkVariable<int> totalRedRemove = new NetworkVariable<int>();
    private NetworkVariable<int> totalInfected = new NetworkVariable<int>();

    // For a game
    public int GreenRemove { get; set; }
    public int BlueRemove { get; set; }
    public int RedRemove { get; set; }
    public int Infected { get; set; }

    public int GreenNPC { get; set; }
    public int BlueNPC{ get; set; }
    public int RedNPC { get; set; }

    public string decision { get; set; }
    public string advice { get; set; }

    public void Awake()
    {
        Instance = this;
    }
    public void accumlateScore()
    {
        totalGreenRemove.Value += GreenRemove;
        totalBlueRemove.Value += BlueRemove;
        totalRedRemove.Value += RedRemove;
        totalInfected.Value += Infected;
    }
    public void resetScore()
    {
        GreenRemove = 0;
        BlueRemove = 0;
        RedRemove = 0;
        Infected = 0;
    }
    public string generateID()
    {
        return Guid.NewGuid().ToString("N");
    }
    public void initLogging()
    {
        participantExpInfo = new ParticipantExpInfo();
        participantExpInfo.Advisor = AdvisorManager.Instance.selectedAgent.ToString();
        participantExpInfo.ID = generateID();

        Debug.Log("Init Log");
    }
    public void resetLogger()
    {
        RedNPC = 0;
        BlueNPC = 0;
        GreenNPC = 0;
        decision = "";
        advice = "";
    }
    public void startGameLog(int game)
    {
        switch (game)
        {
            case 1:
                participantExpInfo.game1 = new ParticipantExpInfo.Game();
                if (PlayerManager.Instance.PlayerInGame == 2)
                    participantExpInfo.game1.HumanAdvisor = true;
                else
                    participantExpInfo.game1.HumanAdvisor = false;

                //TODO
                participantExpInfo.game1.Interrupted = false;
                break;
            case 2:
                participantExpInfo.game2 = new ParticipantExpInfo.Game();
                if (PlayerManager.Instance.PlayerInGame == 2)
                    participantExpInfo.game2.HumanAdvisor = true;
                else
                    participantExpInfo.game2.HumanAdvisor = false;

                //TODO
                participantExpInfo.game2.Interrupted = false;
                break;
            case 3:
                participantExpInfo.game3 = new ParticipantExpInfo.Game();
                if (PlayerManager.Instance.PlayerInGame == 2)
                    participantExpInfo.game3.HumanAdvisor = true;
                else
                    participantExpInfo.game3.HumanAdvisor = false;

                //TODO
                participantExpInfo.game3.Interrupted = false;
                break;
            case 4:
                participantExpInfo.game4 = new ParticipantExpInfo.Game();
                if (PlayerManager.Instance.PlayerInGame == 2)
                    participantExpInfo.game4.HumanAdvisor = true;
                else
                    participantExpInfo.game4.HumanAdvisor = false;

                //TODO
                participantExpInfo.game4.Interrupted = false;
                break;
            case 5:
                participantExpInfo.game5 = new ParticipantExpInfo.Game();
                if (PlayerManager.Instance.PlayerInGame == 2)
                    participantExpInfo.game5.HumanAdvisor = true;
                else
                    participantExpInfo.game5.HumanAdvisor = false;

                //TODO
                participantExpInfo.game5.Interrupted = false;
                break;
            case 6:
                participantExpInfo.game6 = new ParticipantExpInfo.Game();
                if (PlayerManager.Instance.PlayerInGame == 2)
                    participantExpInfo.game6.HumanAdvisor = true;
                else
                    participantExpInfo.game6.HumanAdvisor = false;

                //TODO
                participantExpInfo.game6.Interrupted = false;
                break;
            case 7:
                participantExpInfo.game7 = new ParticipantExpInfo.Game();
                if (PlayerManager.Instance.PlayerInGame == 2)
                    participantExpInfo.game7.HumanAdvisor = true;
                else
                    participantExpInfo.game7.HumanAdvisor = false;

                //TODO
                participantExpInfo.game7.Interrupted = false;
                break;
            case 8:
                participantExpInfo.game8 = new ParticipantExpInfo.Game();
                if (PlayerManager.Instance.PlayerInGame == 2)
                    participantExpInfo.game8.HumanAdvisor = true;
                else
                    participantExpInfo.game8.HumanAdvisor = false;

                //TODO
                participantExpInfo.game8.Interrupted = false;
                break;

        }
    }
    public void LogGame(int game)
    {
        ParticipantExpInfo.Choice choice = new ParticipantExpInfo.Choice();
        choice.Decision = decision;
        choice.Advise = advice;
        choice.BLUENPC = BlueRemove;
        choice.RedNPC = RedRemove;
        choice.GREENNPC = GreenRemove;

        //add switch case
        switch (game)
        {
            case 1:
                participantExpInfo.game1.choices.Add(choice);
                break;
            case 2:
                participantExpInfo.game2.choices.Add(choice);
                break;
            case 3:
                participantExpInfo.game3.choices.Add(choice);
                break;
            case 4:
                participantExpInfo.game4.choices.Add(choice);
                break;
            case 5:
                participantExpInfo.game5.choices.Add(choice);
                break;
            case 6:
                participantExpInfo.game6.choices.Add(choice);
                break;
            case 7:
                participantExpInfo.game7.choices.Add(choice);
                break;
            case 8:
                participantExpInfo.game8.choices.Add(choice);
                break;
        }
    }
    public void writeToJSON()
    {
        string stringjson = JsonConvert.SerializeObject(participantExpInfo);
        Debug.Log(stringjson);
    }
}
