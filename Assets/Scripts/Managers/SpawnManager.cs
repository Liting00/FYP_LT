using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DilmerGames.Core.Singletons;
using Unity.Netcode;

public class SpawnManager : NetworkSingleton<SpawnManager>
{
    //[SerializeField] private GameObject npc_civilian;

    public static SpawnManager Instance;

    [SerializeField]
    private float _width, _height;
    private int greenNonhostileNPCSpawn = 0 ;
    private int blueNonhostileNPCSpawn = 0;
    private int hostileNPCSpawn = 0;

    [SerializeField]
    private List<GameObject> spawnPool;

    private NetworkVariable<int> infected = new NetworkVariable<int>();
    private NetworkVariable<int> nonInfected = new NetworkVariable<int>();

    //public List<ScriptableNPC> _npcs;

    public int Infected
    {
        get
        {
            return infected.Value;
        }
    }
    public int NonInfected
    {
        get
        {
            return nonInfected.Value;
        }
    }
    public void addInfected(int value)
    {
        infected.Value = infected.Value + value;
    }
    public void addNonInfected(int value)
    {
        nonInfected.Value = nonInfected.Value + value;
    }
    private void Awake()
    {
        Instance = this;
        //_npcs = Resources.LoadAll<ScriptableNPC>("NPCS").ToList();
    }

    public void generateNPCs()
    {
        if (NetworkManager.IsServer)
            spawnNPCs();
        GameManager.Instance.ChangeState(GameState.Targeter);
    }
    private void spawnNPCs()
    {
        //Config Variables. Number of Spawns
        hostileNPCSpawn = GameSettings.HOSTILE_NPC;
        greenNonhostileNPCSpawn = GameSettings.NONHOSTILE_GREEN_NPC;
        blueNonhostileNPCSpawn = GameSettings.NONHOSTILE_BLUE_NPC;
        
        //Max Npc Size
        float npcSize = GameSettings.NPC_SIZE;

        if (npcSize > 0.4f)
            npcSize = 0.4f;
        
        float screenX, screenZ, screenY;
        Vector3 pos;

        infected.Value = hostileNPCSpawn;
        nonInfected.Value = greenNonhostileNPCSpawn +  blueNonhostileNPCSpawn;

        for(int i = 0; i < blueNonhostileNPCSpawn; i++)
        {
            screenX = Random.Range((float)0.0000, _width  - 1);
            screenZ = Random.Range((float)0.0000, _height - 1);
            pos = new Vector3(screenX, 0.1f, screenZ);

            //add collison
            if (!Physics.CheckSphere(pos, 0.6f, LayerMask.GetMask("NPC")))
            {
                //Debug.Log("Collision");
                spawnPool[0].name = $"BlueNonHostileNPC {i}";
                spawnPool[0].transform.localScale = new Vector3(npcSize, npcSize, npcSize);
                GameObject go = Instantiate(spawnPool[0], pos, Quaternion.identity);
                go.GetComponent<NetworkObject>().Spawn();
            }
            else
                i--;
        }
        for (int i = 0; i < greenNonhostileNPCSpawn; i++)
        {
            screenX = Random.Range((float)0.0000, _width  - 1 );
            screenZ = Random.Range((float)0.0000, _height - 1);
            pos = new Vector3(screenX, 0.1f, screenZ);

            //add collison
            if (!Physics.CheckSphere(pos, 0.6f, LayerMask.GetMask("NPC")))
            {
                //Debug.Log("Collision");
                spawnPool[2].name = $"GreenNonHostileNPC {i}";
                spawnPool[2].transform.localScale = new Vector3(npcSize, npcSize, npcSize);
                GameObject go = Instantiate(spawnPool[2], pos, Quaternion.identity);
                go.GetComponent<NetworkObject>().Spawn();
            }
            else
                i--;
        }
        for (int i = 0; i < hostileNPCSpawn; i++)
        {
            screenX = Random.Range((float)0.0000, _width  - 1);
            screenZ = Random.Range((float)0.0000, _height - 1);
            screenY = 0.1f;
            pos = new Vector3(screenX, screenY, screenZ);

            if (!Physics.CheckSphere(pos, 0.6f, LayerMask.GetMask("NPC")))
            {
                //Debug.Log("Collision");
                spawnPool[1].name = $"HostileNPC {i}";
                spawnPool[1].transform.localScale = new Vector3(npcSize, npcSize, npcSize);
                GameObject go = Instantiate(spawnPool[1], pos, Quaternion.identity);
                go.GetComponent<NetworkObject>().Spawn();
            }
            else
                i--;
        }
    }
}