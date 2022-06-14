using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DilmerGames.Core.Singletons;
using UnityEngine.Networking;
using Unity.Netcode;

public class TargetController : NetworkSingleton<TargetController>
{
    private static string npc = "HostileNPC";
    private static string tile = "Tile";


    private GameObject[] hostileNPCS;
    private GameObject[] tiles;

    internal GameObject selectedNPC;
    internal List<GameObject> highlightedNPCs = new List<GameObject>();
    internal List<GameObject> collidedTile = new List<GameObject>();

    internal GameObject selectedTile;

    private GameObject previousTile;

    internal bool changeTarget = false;
    internal bool destroy = false;

    //Update interval
    private float update;
    private float nextUpdatedTime = 0.1f;
    private bool enableUpdate = false;
   
    public Material material;
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
            tile.GetComponent<Renderer>().material = material;
        }

        foreach (GameObject tile in collidedTile)
        {
            if (tile.transform.position.x >= npcPosX && tile.transform.position.x <= npcPosX + 1f
                && tile.transform.position.z >= npcPosZ && tile.transform.position.z <= npcPosZ + 1f)
            {
                tile.GetComponent<Renderer>().material.color = Color.gray;
                selectedTile = tile;
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
            tile.GetComponent<Renderer>().material = material;
        }

        hostileNPCS = GameObject.FindGameObjectsWithTag(npc);

        if (hostileNPCS.Length == 0)
        {
            changeTarget = false;
            return;
        }

        //Start for changing target
        if (changeTarget == true)
        {
            Debug.Log("Change Target");
            string previousNPCName = "";

            if (selectedNPC != null)
                previousNPCName = selectedNPC.name;

            do
            {
                selectedNPC = hostileNPCS[Random.Range(0, hostileNPCS.Length)];
                if (hostileNPCS.Length == 1)
                    break;
            }
            while (string.Compare(previousNPCName, selectedNPC.name) == 0);

            changeTarget = false;
        }

        float npcPosX = selectedNPC.transform.position.x - 0.5f;
        float npcPosZ = selectedNPC.transform.position.z - 0.5f;

        //highlight selectedNPC tile
        foreach (GameObject tile in collidedTile)
        {
            //Debug.Log("Tiles " + tile.name);
            //Debug.Log("Selected NPC " + npcPosX + ", " + npcPosZ);
            if (tile.transform.position.x >= npcPosX && tile.transform.position.x <= npcPosX + 1f 
                && tile.transform.position.z >= npcPosZ && tile.transform.position.z <= npcPosZ + 1f)
            {
                tile.GetComponent<Renderer>().material.color = Color.red;
                selectedTile = tile;
                targetClientRpc(npcPosX, npcPosZ);
            }
        }
        //TODO: Cannot make Collision Work
        if (previousTile != selectedTile)
        {
            highlightedNPCs.Clear();
            previousTile = selectedTile;
            Debug.Log("Clear");
        }
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("NonHostileNPC"))
        {
            if (o.transform.position.x >= npcPosX && o.transform.position.x <= npcPosX + 1.05f
                && o.transform.position.z >= npcPosZ && o.transform.position.z <= npcPosZ + 1.05f)
            {
                highlightedNPCs.Add(o.gameObject);
            }
        }
    }
    public void destroyTarget()
    {
        if (hostileNPCS.Length == 0)
            return;

        //TODO: Cannot make Collision Work
        /*Destroy(selectedNPC);
        SpawnManager.Instance.addInfected(-1);

        foreach (GameObject npc in highlightedNPCs)
        {
            Debug.Log(npc.name);
            if (npc.name.Contains("Non"))
                SpawnManager.Instance.addNonInfected(-1);
            else
                SpawnManager.Instance.addInfected(-1);
            
            Destroy(npc);
        }*/

        float npcPosX = selectedNPC.transform.position.x - 0.5f;
        float npcPosZ = selectedNPC.transform.position.z - 0.5f;

        foreach (GameObject o in GameObject.FindGameObjectsWithTag("HostileNPC"))
        {
            //Debug.Log("Vector X: " + o.transform.position.x);
            //Debug.Log("Vector Y: " + o.transform.position.y);
            if (o.transform.position.x >= npcPosX && o.transform.position.x <= npcPosX + 1f 
                && o.transform.position.z >= npcPosZ && o.transform.position.z <= npcPosZ + 1f)
            {
                o.SetActive(false);
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
                o.SetActive(false);
                Destroy(o);
                SpawnManager.Instance.addNonInfected(-1);
            }
        }
        return;
    }
}
