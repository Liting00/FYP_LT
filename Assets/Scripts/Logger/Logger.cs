using Network.Singletons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.Netcode;
using UnityEngine;

public class Logger : NetworkSingleton<Logger>
{
    public static new Logger Instance;

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
    public int BlueNPC { get; set; }
    public int RedNPC { get; set; }

    public string decision { get; set; }
    public string advice { get; set; }

    //JSON Logger
    ParticipantExpInfo participantExpInfo = new ParticipantExpInfo();

    //JSON Filepath
    private string filepath = GameSettings.FILEPATH;

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

        participantExpInfo.Advisor = AdvisorManager.Instance.selectedAgent.ToString();
        participantExpInfo.ID = generateID();
        participantExpInfo.DateTime = DateTime.Now.ToString();
        participantExpInfo.HARoleplay = "nill";

        //Create instances of class list
        for (int i = 0; i < GameSettings.NUMBEROFGAMES; i++)
        {
            participantExpInfo.game[i] = new ParticipantExpInfo.Game();
        }

        Debug.Log("Init Log");
    }
    public void LogRolePlay(string roleplay)
    {
        participantExpInfo.HARoleplay = roleplay.ToString();
    }
    public void resetLogger()
    {
        RedNPC = 0;
        BlueNPC = 0;
        GreenNPC = 0;
        decision = "nill";
        advice = "nill";
    }
    public void LogInterrupt(int GameNum)
    {
        participantExpInfo.game[GameNum - 1].Interrupted = true;
    }
    public void startGameLog(int GameNum)
    {
        Debug.Log("Game:" + GameNum);

        participantExpInfo.game[GameNum - 1].Interrupted = false;
        if (PlayerManager.Instance.PlayerInGame == 2)
            participantExpInfo.game[GameNum - 1].HumanAdvisor = true;
        else
            participantExpInfo.game[GameNum - 1].HumanAdvisor = true;
    }
    public void LogGame(int GameNum)
    {
        Debug.Log("Log Game");
        ParticipantExpInfo.Choice choice = new ParticipantExpInfo.Choice();
        choice.Decision = decision;
        choice.Advise = advice;
        choice.BLUENPC = BlueRemove;
        choice.RedNPC = RedRemove;
        choice.GREENNPC = GreenRemove;

        participantExpInfo.game[GameNum - 1].choices.Add(choice);
    }
    public void writeToTextFile()
    {
        List<ParticipantExpInfo> HostData = new List<ParticipantExpInfo>();

        string jsonData;

        if (File.Exists(filepath)) 
        {
            jsonData = File.ReadAllText(filepath);
            HostData = JsonConvert.DeserializeObject<List<ParticipantExpInfo>>(jsonData)
                      ?? new List<ParticipantExpInfo>();
            
        }

        HostData.Add(participantExpInfo);
        jsonData = JsonConvert.SerializeObject(HostData);

        Debug.Log(jsonData);

        File.WriteAllText(filepath, jsonData);
    }

    //public void writeToCSV()
    //{
    //    string filePath = "HostData.csv";

    //    bool fileExist = File.Exists(filePath);
    //    if (!fileExist)
    //    {
    //        string header = $"\"ID\",\"Advisor\",\"HARoleplay\",\"DateTime\",\"Game01\",\"Game02\",\"Game03\",\"Game04\",\"Game05\"" +
    //            $",\"Game06\",\"Game07\",\"Game08\"{Environment.NewLine}";
    //        File.AppendAllText(filePath, header);
    //    }

    //    var csv = new StringBuilder();

    //    var ID = participantExpInfo.ID.ToString();
    //    var Advisor = participantExpInfo.Advisor.ToString();
    //    var HARoleplay = participantExpInfo.HARoleplay.ToString();
    //    var DateTime = participantExpInfo.DateTime.ToString();

    //    var newLine = string.Format("{0}, {1}, {2}, {3}", ID, Advisor, HARoleplay, DateTime);
    //    csv.AppendLine(newLine);

    //    for (int i = 0; i < GameSettings.NUMBEROFGAMES; i++)
    //    {
    //        var game = participantExpInfo.game[i];
    //        newLine = string.Format("{0}", game.ToString());
    //        csv.AppendLine(newLine);
    //    }
    //    File.AppendAllText(filePath, csv.ToString());
    //}
}
