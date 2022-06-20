using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO: NPC Size, Tile Size, Number of NPCs, Infection Radius, Texts
public static class GameSettings
{
    public static int WIDTH = 19;
    public static int HEIGHT = 10;

    public static int NONHOSTILE_GREEN_NPC = 30;
    public static int NONHOSTILE_BLUE_NPC = 30;
    public static int HOSTILE_NPC = 5;

    public static string RED_BUTTON_TEXT = "Shoot";
    public static string GREEN_BUTTON_TEXT = "Pass";
    public static string NO_ADVISE_TEXT = "No Advise";

    public static int INFECTION_RADIUS = 5;
    public static float INFECTION_RATE = 0.1f;
    public static float TILE_SIZE = 5f;

    public static float NPC_SIZE = 1f;
}