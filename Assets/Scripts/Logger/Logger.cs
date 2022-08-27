using Network.Singletons;
using System.Collections;
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
    public int BlueNPC{ get; set; }
    public int RedNPC { get; set; }

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

    public void NPC()
    {
        GreenNPC = 0;
        BlueNPC = 0;
        RedNPC = 0;
    }
}
