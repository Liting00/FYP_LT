using System;
using System.Collections.Generic;
using Newtonsoft.Json;

class ParticipantExpInfo
{
    public string ID;
    public string Advisor;
    public string HARoleplay;
    public string DateTime;
    public string joinCode;

    public Game[] game = new Game[GameSettings.NUMBEROFGAMES];

    public class Game
    {
        public bool HumanAdvisor;
        public bool Interrupted;
        public List<Choice> choices = new List<Choice>();
        
    }
    public class Choice
    {
        public string Decision;
        public string Advise;
        public int RedNPC;
        public int GREENNPC;
        public int BLUENPC;
    }
}