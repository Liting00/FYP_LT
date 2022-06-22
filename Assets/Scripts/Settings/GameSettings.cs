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
    public static int NONHOSTILE_GREEN_NPC = 30;
    public static int NONHOSTILE_BLUE_NPC = 30;
    public static int HOSTILE_NPC = 5;

    //Advisor Text
    public static string RED_BUTTON_TEXT = "Shoot";
    public static string GREEN_BUTTON_TEXT = "Pass";
    public static string NO_ADVISE_TEXT = "No Advise";

    //Advisor Update Interval
    public static float ADVISOR_UPDATE_TIME = 0.5f;

    //Recommended from 2f to 10f
    public static float INFECTION_RADIUS = 5f;
    //Max is 1f
    public static float INFECTION_RATE = 0.01f;
    public static float INFECTION_INTERVAL_MIN = 5f;
    public static float INFECTION_INTERVAL_MAX = 10f;

    public static float TILE_SIZE = 1f;

    //Min: 0f to Max: 0.4f
    public static float NPC_SIZE = 0.2f;
}