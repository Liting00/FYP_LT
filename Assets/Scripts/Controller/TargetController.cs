using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DilmerGames.Core.Singletons;
using UnityEngine.Networking;
using Unity.Netcode;

public class TargetController : NetworkSingleton<TargetController>
{
    private string npc = "HostileNPC";
    private string tile = "Tile";
    private string previousNPCName = "";

    private GameObject[] hostileNPCS;
    private GameObject[] tiles;
    private GameObject selectedNPC;

    internal List<GameObject> collidedTile = new List<GameObject>();
    internal bool changeTarget = false;
    internal bool destroy = false;

    private float update;
    private float nextUpdatedTime = 0.1f;

    private bool enableUpdate = false;
    /*void UpdateTarget()
    {
        //Debug.Log("Start Target");
        hostileNPCS = GameObject.FindGameObjectsWithTag(npc);
        tiles = GameObject.FindGameObjectsWithTag(tile);

        float npcPosX, npcPosZ;

        foreach (GameObject npc in hostileNPCS)
        {
            npcPosX = npc.transform.position.x -0.5f;
            npcPosZ = npc.transform.position.z -0.5f;
            //Debug.Log(fx + " " + fz);

            //find tiles with hostile NPCs
            //findTiles(npcPosX, npcPosZ, tileIndex);
            
            //target tile
            foreach (GameObject tile in tiles)
            {
                //render same color back, i can't seem to get same color from the tile
                tile.GetComponent<Renderer>().material.color = new Color(38 / 255.0f, 38 / 255.0f, 38 / 255.0f);
                if (tile.transform.position.x >= npcPosX && tile.transform.position.x <= npcPosX + 1f && tile.transform.position.z >= npcPosZ && tile.transform.position.z <= npcPosZ + 1f)
                {
                    //Debug.Log("Active");
                    tile.GetComponent<Renderer>().material.color = Color.grey;
                }
            }   
        }
    }*/
    /*private void Awake()
    {
        Debug.Log("TargetController awake was called");
    }*/
    /*void Start()
    {
        tiles = GameObject.FindGameObjectsWithTag(tile);
        hostileNPCS = GameObject.FindGameObjectsWithTag(npc);
        selectedNPC = hostileNPCS[Random.Range(0, hostileNPCS.Length)];

        GameObject SManager = GameObject.Find("Spawn Manager");
        spawnManager = SManager.GetComponent<SpawnManager>();
    }*/
    // Update is called once per frame
    void Update()
    {
        if (enableUpdate == false) return;

        update += Time.deltaTime;
        if (update > nextUpdatedTime)
        {
            update = 0f;
            updateTarget();
        }
    }
    public void targetInit()
    {
        Init();
        updateClientRpc();
    }
    private void Init()
    {
        Debug.Log("Target Initalize");
        tiles = GameObject.FindGameObjectsWithTag(tile);
        hostileNPCS = GameObject.FindGameObjectsWithTag(npc);
        selectedNPC = hostileNPCS[Random.Range(0, hostileNPCS.Length)];
        enableUpdate = true;
    }
    [ClientRpc]
    private void updateClientRpc()
    {
        if (IsOwner) return;

        Init();
    }
    [ClientRpc]
    void targetClientRpc(float npcPosX, float npcPosZ)
    {
        if (IsOwner) return;

        //colour all the tiles to black
        foreach (GameObject tile in tiles)
        {
            tile.GetComponent<Renderer>().material.color = Color.black;
        }

        foreach (GameObject tile in collidedTile)
        {
            if (tile.transform.position.x >= npcPosX && tile.transform.position.x <= npcPosX + 1f
                && tile.transform.position.z >= npcPosZ && tile.transform.position.z <= npcPosZ + 1f)
            {
                tile.GetComponent<Renderer>().material.color = Color.gray;
                return;
            }
        }
    }
    private void updateTarget()
    {
        //don't run if your advisor
        if (!IsOwner) return;

        //colour all the tiles to black
        foreach (GameObject tile in tiles)
        {
            tile.GetComponent<Renderer>().material.color = Color.black;
        }
        hostileNPCS = GameObject.FindGameObjectsWithTag(npc);
        if (hostileNPCS.Length == 0)
            return;

        //for changing target
        if (changeTarget == true)
        {
            Debug.Log("Change Target");
            int timeout = 5;
            do
            {
                selectedNPC = hostileNPCS[Random.Range(0, hostileNPCS.Length)];
                //** need to optimize this
                timeout--;
                if (timeout == 0) break;
            }
            while (string.Compare(previousNPCName, selectedNPC.name) == 0);
            previousNPCName = selectedNPC.name;
            changeTarget = false;
        }
        //highlight selectedNPC tile
        foreach (GameObject tile in collidedTile)
        {
            //Debug.Log("Tiles " + tile.name);
            float npcPosX = selectedNPC.transform.position.x - 0.5f;
            float npcPosZ = selectedNPC.transform.position.z - 0.5f;
            //Debug.Log("Selected NPC " + npcPosX + ", " + npcPosZ);
            if (tile.transform.position.x >= npcPosX && tile.transform.position.x <= npcPosX + 1f 
                && tile.transform.position.z >= npcPosZ && tile.transform.position.z <= npcPosZ + 1f)
            {
                tile.GetComponent<Renderer>().material.color = Color.gray;
                targetClientRpc(npcPosX, npcPosZ);
                return;
            }
        }
    }
    public void destroyTarget()
    {
        if (hostileNPCS.Length == 0)
            return;
        float npcPosX = selectedNPC.transform.position.x - 0.5f;
        float npcPosZ = selectedNPC.transform.position.z - 0.5f;

        foreach (GameObject o in GameObject.FindGameObjectsWithTag("HostileNPC"))
        {
            //Debug.Log("Vector X: " + o.transform.position.x);
            //Debug.Log("Vector Y: " + o.transform.position.y);
            if (o.transform.position.x >= npcPosX && o.transform.position.x <= npcPosX + 1f 
                && o.transform.position.z >= npcPosZ && o.transform.position.z <= npcPosZ + 1f)
            {
                Destroy(o);
                SpawnManager.Instance.addInfected(-1);
            }

        }
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("NonHostileNPC"))
        {
            //Debug.Log("Vector X: " + o.transform.position.x);
            //Debug.Log("Vector Y: " + o.transform.position.y);
            if (o.transform.position.x >= npcPosX && o.transform.position.x <= npcPosX + 1f 
                && o.transform.position.z >= npcPosZ && o.transform.position.z <= npcPosZ + 1f)
            {
                Destroy(o);
                SpawnManager.Instance.addNonInfected(-1);
            }

        }
    }
}
