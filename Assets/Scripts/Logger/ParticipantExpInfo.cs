using System;
using System.Collections.Generic;
using Newtonsoft.Json;

class ParticipantExpInfo
{
    public string ID;
    public string Advisor;

    public Game game1;
    public Game game2;
    public Game game3;
    public Game game4;
    public Game game5;
    public Game game6;
    public Game game7;
    public Game game8;

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