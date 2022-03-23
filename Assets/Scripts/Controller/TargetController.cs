using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    private string npc = "HostileNPC";
    private string tile = "Tile";
    private GameObject[] hostileNPCS;
    private GameObject[] tiles;
    private GameObject[] highlighted;

    internal List<GameObject> collidedTile = new List<GameObject>();

    public float range = 15f;

    private float update;
    private float nextUpdatedTime = 0.01f;
    void UpdateTarget()
    {
        //Debug.Log("Start Target");
        update = 0.0f;
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
    void findTiles(float npcPosX, float npcPosZ, int index)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].transform.position.x >= npcPosX && tiles[i].transform.position.x <= npcPosX + 1f && tiles[i].transform.position.z >= npcPosZ
                && tiles[i].transform.position.z <= npcPosZ + 1f)
            {
                highlighted[index] = tiles[i];
                index++;
            }
        }
    }
    void Start()
    {
        Debug.ClearDeveloperConsole();

        //objColor = tiles[0].GetComponent<MeshRenderer>().material.color;
        //print(objColor.r + " " + objColor.g + " " + objColor.b + " " + objColor.a + " ");
    }
    // Update is called once per frame
    void Update()
    {
        update += Time.deltaTime;
        if (update > nextUpdatedTime)
        {
            //Debug.Log(collidedTile.Count);
            /*if (!hostileNPCs.Count.Equals(0))
            {
                for (int i=0; i< hostileNPCS.Length; i++)
                {
                    Debug.Log(hostileNPCS[i].name);
                }
            }*/
            //UpdateTarget();
        }
        
    }
}
