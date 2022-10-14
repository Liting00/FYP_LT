using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO: NPC Size, Tile Size, Number of NPCs, Infection Radius, Texts
public static class GameSettings
{
    /* Environment */
    //Width and Height of the Environment
    public static int WIDTH = 19;
    public static int HEIGHT = 10;

    //Max 1f
    public static float TILE_SIZE = 1f;

    //Background Colour Select
    public static int BACKGROUND_SELECT = 0;

    //Tile Colour Select
    public static int TILE_SELECT = 2;

    //Delay for Game Over screen to appear after all red npcs is removed
    public static float ENDGAME_SCENE_DELAY = 1.2f;

    //Number of Game per Session
    public static int NUMBEROFGAMES = 8;


    /* NPC */
    //Min: 0f to Max: 0.4f
    public static float NPC_SIZE = 0.18f;

    //Number of Spawns
    public static int NONHOSTILE_GREEN_NPC = 100;
    public static int NONHOSTILE_BLUE_NPC = 100;
    public static int HOSTILE_NPC = 4;

    //Range: 0.1f to 0.5f
    public static float NPC_SPEED = 0.2F;
    //Range: 10 to 100
    public static float NPC_WALKRADIUS = 30;


    /* Advisor */
    //Advisor Text
    public static string RED_BUTTON_TEXT = "REMOVE";
    public static string GREEN_BUTTON_TEXT = "Pass";
    public static string NO_ADVISE_TEXT = "No Advice";

    //Change Target Advise Update Time
    public static float ADVISOR_UPDATE_TIME = 0.2f;
    
    //(No Advise Message) Artifical Delay when Target is Changed
    public static float CHANGE_TARGET_TARGET_DELAY = 0.5f;

    //Artificial Delay For Advisor No Advise Message
    public static bool ENABLE_CHANGE_TARGET_DELAY = true;
    
    /* Infection */
    //Recommended from 2f to 15f
    public static float INFECTION_RADIUS = 8f;

    //Max is 1f
    public static float INFECTION_RATE = 0.20f;

    //Rate of Infection Interval Min to Max
    public static float INFECTION_INTERVAL_MIN = 4f;
    public static float INFECTION_INTERVAL_MAX = 12f;

    //Human Advisor additional Goal
    public static string ADDITIONALGOALTEXT = $"Additional Instructions: ";
    public static string BIAS_AGAINST_GREEN = "\n1. You do not mind green people getting infected.\n2. You do not mind green people getting remove.";
    public static string BIAS_AGAINST_BLUE = "\n1. You do not mind blue people getting infected.\n2. You do not mind blue people getting remove.";

    //Text when the game end or session end
    public static string WINGAMETEXT = "All Hostile are Removed. \nPlease Start the Next Round.";
    public static string LOSEGAMETEXT = "All NPCs has been Infected. \nPlease Start the Next Round.";
    public static string GAMEOVERTEXT = "You Have Completed the Experiment. \nThank You for Your Participation. \nPlease Wait for the Survey.";
    public static string ADVISORWAITTEXT = "Please Wait for Shooter to Start Experiment";
    public static string INTERRUPTEDTEXT = "Game has been Interrupted. \nPlease Start The Next Round.";

    //Digital Ocean HTTP
    public static string BASE = "http://159.223.65.52:5000";

    //Enable KeyPress
    public static bool ENABLE_SHOOT_PASS_KEYPRESSED = true;

    //JSON Save Path
    public static string FILEPATH = "HostData.json";

    //Random Role enable
    public static bool randRole = true;
}