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
    public static int NUMBEROFGAMES = 5;


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
    public static string RED_BUTTON_TEXT = "Shoot";
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
    public static float INFECTION_INTERVAL_MIN = 5f;
    public static float INFECTION_INTERVAL_MAX = 10f;
    

    //Text when the game end or session end
    public static string WINGAMETEXT = "All Hostile are gone";
    public static string LOSEGAMETEXT = "Everyone has been infected";
    public static string GAMEOVERTEXT = "Game Has Ended";
    public static string ADVISORWAITTEXT = "Please Wait for Game to Start";

    //Docker HTTP
    public static string BASE = "http://127.0.0.1:5000";

    //Enable KeyPress
    public static bool ENABLE_SHOOT_PASS_KEYPRESSED = true;
}