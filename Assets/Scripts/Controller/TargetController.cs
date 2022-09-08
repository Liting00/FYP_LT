using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network.Singletons;
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

    [SerializeField]
    Tile tileScript;

    internal GameObject selectedTile;

    internal bool changeTarget = false;
    internal bool destroy = false;

    //Update interval
    private float update;
    private float nextUpdatedTime = 0.1f;
    private bool EnableTargetController { get; set; }
   
    public Material[] material;

    void Update()
    {
        if (EnableTargetController == false) return;

        if (PlayerManager.Instance.allowQuickJoin)
        {
            updateClientRpc();
        }
            
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
        EnableTargetController = true;
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
        //Don't Run if your Player
        if (IsOwner) return;

        //colour all the tiles to black
        foreach (GameObject tile in tiles)
        {
            if (tile == null)
                continue;
            tile.GetComponent<Tile>().redHighlight(false);
        }

        //highlight selected Tile
        foreach (GameObject tile in collidedTile)
        {
            if (tile == null)
                continue;

            if (tile.transform.position.x >= npcPosX && tile.transform.position.x <= npcPosX + 1f
                && tile.transform.position.z >= npcPosZ && tile.transform.position.z <= npcPosZ + 1f)
            {
                tile.GetComponent<Tile>().redHighlight(true);
                selectedTile = tile;
                return;
            }
        }
    }
    private void updateTarget()
    {
        //don't run if your advisor
        if (!IsOwner) return;

        //colour all the tiles back to normal
        foreach (GameObject tile in tiles)
        {
            if (tile == null)
                continue;

            //tile.GetComponent<Renderer>().material = material[GameSettings.TILE_SELECT];
            tile.GetComponent<Tile>().redHighlight(false);
        }

        hostileNPCS = GameObject.FindGameObjectsWithTag(npc);

        if (hostileNPCS.Length == 0)
        {
            changeTarget = false;
            highlightedNPCs.Clear();
            selectedNPC = null;
            return;
        }

        //Start for changing target
        if (changeTarget == true)
        {
            Debug.Log("Change Target");

            // Artificial Advisor No Advise Message Delay
            if (GameSettings.ENABLE_CHANGE_TARGET_DELAY)
                StartCoroutine(changeTargetDelay());

            string previousNpcName = "";

            if (selectedNPC != null)
                previousNpcName = selectedNPC.name;

            do
            {
                selectedNPC = hostileNPCS[Random.Range(0, hostileNPCS.Length)];
                if (hostileNPCS.Length == 1)
                    break;
            }
            while (string.Compare(previousNpcName, selectedNPC.name) == 0);

            changeTarget = false;     
        }

        float npcPosX = selectedNPC.transform.position.x - 0.5f;
        float npcPosZ = selectedNPC.transform.position.z - 0.5f;

        //highlight selectedNPC tile
        foreach (GameObject tile in collidedTile)
        {
            if (tile == null)
                continue;

            //Debug.Log("Tiles " + tile.name);
            //Debug.Log("Selected NPC " + npcPosX + ", " + npcPosZ);
            if (tile.transform.position.x >= npcPosX && tile.transform.position.x <= npcPosX + 1f 
                && tile.transform.position.z >= npcPosZ && tile.transform.position.z <= npcPosZ + 1f)
            {
                //tile.GetComponent<Renderer>().material.color = Color.red;
                tile.GetComponent<Tile>().redHighlight(true);
                selectedTile = tile;
                if(PlayerManager.Instance.PlayerInGame == 2)
                    targetClientRpc(npcPosX, npcPosZ);
                break;
            }
        }
    }
    IEnumerator changeTargetDelay()
    {
        AdvisorManager.Instance.changeTargetDelay = true;
        yield return new WaitForSeconds(GameSettings.CHANGE_TARGET_TARGET_DELAY);
        Debug.Log("Processed Advise");
        AdvisorManager.Instance.changeTargetDelay = false;
    }
    public void recordNPCs()
    {
        if (hostileNPCS.Length == 0)
            return;

        foreach (GameObject npc in highlightedNPCs)
        {
            if (npc == null)
                continue;

            if (npc.name.Contains("GreenNonHostile") && !npc.name.Contains("Infected"))
            {
                Logger.Instance.GreenNPC++;
            }
            else if (npc.name.Contains("BlueNonHostile") && !npc.name.Contains("Infected"))
            {
                Logger.Instance.BlueNPC++;
            }
            else
            {
                Logger.Instance.RedNPC++;
            }
        }
    }
    public void destroyTarget()
    {
        if (hostileNPCS.Length == 0)
            return;

        foreach (GameObject npc in highlightedNPCs)
        {
            if (npc == null)
                continue;

            if (npc.name.Contains("GreenNonHostile") && !npc.name.Contains("Infected"))
            {
                Logger.Instance.GreenRemove++;
                SpawnManager.Instance.NonInfected--;
                Logger.Instance.GreenNPC++;

            }
            else if(npc.name.Contains("BlueNonHostile") && !npc.name.Contains("Infected"))
            {
                Logger.Instance.BlueRemove++;
                SpawnManager.Instance.NonInfected--;
                Logger.Instance.BlueNPC++;
            }
            else
            {
                Logger.Instance.RedRemove++;
                SpawnManager.Instance.Infected--;
                Logger.Instance.RedNPC++;
                Debug.Log("Red removed: " + Logger.Instance.RedNPC);
            }
            Destroy(npc.gameObject);
        }
    }
}
