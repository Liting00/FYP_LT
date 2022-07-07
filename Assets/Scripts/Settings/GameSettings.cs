using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO: NPC Size, Tile Size, Number of NPCs, Infection Radius, Texts
public static class GameSettings
{
    //Width and Height of the Environment
    public static int WIDTH = 19;
    public static int HEIGHT = 10;

    //Number of Spawns
    public static int NONHOSTILE_GREEN_NPC = 100;
    public static int NONHOSTILE_BLUE_NPC = 100;
    public static int HOSTILE_NPC = 4;

    //Advisor Text
    public static string RED_BUTTON_TEXT = "Shoot";
    public static string GREEN_BUTTON_TEXT = "Pass";
    public static string NO_ADVISE_TEXT = "No Advise";

    //Change Target Advise Update Time
    public static float ADVISOR_UPDATE_TIME = 0.1f;
    

    //(No Advise Message) Delay when Change Target
    public static float CHANGE_TARGET_TARGET_DELAY = 0.5f;
    // On/Off artifical No Advise delay
    public static bool ENABLE_CHANGE_TARGET_DEAY = true;
    //Recommended from 2f to 15f
    public static float INFECTION_RADIUS = 8f;
    //Max is 1f
    public static float INFECTION_RATE = 0.20f;
    public static float INFECTION_INTERVAL_MIN = 5f;
    public static float INFECTION_INTERVAL_MAX = 10f;

    //Max 1f
    public static float TILE_SIZE = 1f;

    //Min: 0f to Max: 0.4f
    public static float NPC_SIZE = 0.18f;

    public static int BACKGROUND_SELECT = 0;
    public static int TILE_SELECT = 2;

    public static float ENDGAME_DELAY = 1.2f;

    public static int NUMBEROFGAMES = 5;
}