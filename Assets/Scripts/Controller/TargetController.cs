using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    private string npc = "HostileNPC";
    private string tile = "Tile";


    private GameObject[] hostileNPCS;
    private GameObject[] tiles;

    internal List<GameObject> collidedTile = new List<GameObject>();
    private GameObject selectedNPC;
    private int index = 0;
    internal bool changeTarget = true;

    public float range = 15f;

    private float update;
    private float nextUpdatedTime = 0.1f;
    void UpdateTarget()
    {
        //Debug.Log("Start Target");
        hostileNPCS = GameObject.FindGameObjectsWithTag(npc);
        tiles = GameObject.FindGameObjectsWithTag(tile);
        
        float npcPosX, npcPosZ;
        int tileIndex = 0;

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
    }
    int SelectAnHostileNPC(GameObject[] hostileNPCS)
    {
        return Random.Range(0, hostileNPCS.Length-1);
    }
    void updateTarget2()
    {
        tiles = GameObject.FindGameObjectsWithTag(tile);
        foreach (GameObject tile in tiles)
        {
            tile.GetComponent<Renderer>().material.color = new Color(38 / 255.0f, 38 / 255.0f, 38 / 255.0f);
        }

        hostileNPCS = GameObject.FindGameObjectsWithTag(npc);
        //Debug.Log(hostileNPCS.Length);

        //for changing target
        if (changeTarget == true)
        {
            int timeout = 5;
            while (timeout > 0)
            {
                int previousIndex = index;
                timeout--;
                index = SelectAnHostileNPC(hostileNPCS);
                if (index != previousIndex)
                    break;
            }
            
            selectedNPC = hostileNPCS[index];
            changeTarget = false;
            //Debug.Log(selectedNPC.name);
        }
        //Debug.Log("Collided Tile: " + collidedTile.Count);
        foreach (GameObject tile in collidedTile)
        {
            //Debug.Log("Tiles " + tile.transform.position);
            float npcPosX = selectedNPC.transform.position.x - 0.5f;
            float npcPosZ = selectedNPC.transform.position.z - 0.5f;
            //Debug.Log("Selected NPC " + npcPosX + ", " + npcPosZ);
            if (tile.transform.position.x >= npcPosX && tile.transform.position.x <= npcPosX + 1f && tile.transform.position.z >= npcPosZ && tile.transform.position.z <= npcPosZ + 1f)
                tile.GetComponent<Renderer>().material.color = Color.grey;
        }
    }
    void Start()
    {
        //selectedNPC = SelectAnHostileNPC(hostileNPCS);
        //objColor = tiles[0].GetComponent<MeshRenderer>().material.color;
        //print(objColor.r + " " + objColor.g + " " + objColor.b + " " + objColor.a + " ");
    }
    // Update is called once per frame
    void Update()
    {
        update += Time.deltaTime;
        if (update > nextUpdatedTime)
        {
            update = 0f;
            updateTarget2();
        }
        
    }
}
