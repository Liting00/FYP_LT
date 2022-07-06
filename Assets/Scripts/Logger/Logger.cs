using System.Collections;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public static Logger Instance;

    // Total accumulate
    private int totalGreenRemove = 0;
    private int totalBlueRemove = 0;
    private int totalRedRemove = 0;
    private int totalInfected = 0;

    // For a game
    public int GreenRemove { get; set; }
    public int BlueRemove { get; set; }
    public int RedRemove { get; set; }
    public int Infected { get; set; }
    public void Awake()
    {
        Instance = this;
    }
    public void accumlateScore()
    {
        totalGreenRemove += GreenRemove;
        totalBlueRemove += BlueRemove;
        totalRedRemove += RedRemove;
        totalInfected += Infected;
    }
    public void resetScore()
    {
        GreenRemove = 0;
        BlueRemove = 0;
        RedRemove = 0;
        Infected = 0;
    }
}
