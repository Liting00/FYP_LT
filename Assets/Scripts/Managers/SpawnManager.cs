using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //[SerializeField] private GameObject npc_civilian;

    public static SpawnManager Instance;

    [SerializeField] private float _width, _height;

    public int NonhostileNPCSpawn;
    public int hostileNPCSpawn;
    public List<GameObject> spawnPool;

    public List<ScriptableNPC> _npcs;


    private void Awake()
    {
        Instance = this;

        //_npcs = Resources.LoadAll<ScriptableNPC>("NPCS").ToList();
    }

    void Start()
    {
        //spawnObject();
    }
    public void spawnObject()
    {
        float screenX, screenZ, screenY;
        Vector3 pos;

        for(int i = 0; i < NonhostileNPCSpawn; i++)
        {
            //offset -3 to move to the right
            screenX = Random.Range((float)0.0000, _width - 3 - 1);
            screenZ = Random.Range((float)0.0000, _height - 1);
            pos = new Vector3(screenX, 0.1f, screenZ);

            //add collison
            if (!Physics.CheckSphere(pos, 0.6f, LayerMask.GetMask("NPC")))
            {
                //Debug.Log("Collision");
                spawnPool[1].name = $"NonHostileNPC {i}";
                Instantiate(spawnPool[0], pos, Quaternion.identity);
            }
        }
        for (int i = 0; i < hostileNPCSpawn; i++)
        {
            screenX = Random.Range((float)0.0000, _width - 3 - 1);
            screenZ = Random.Range((float)0.0000, _height - 1);
            screenY = 0.1f;
            pos = new Vector3(screenX, screenY, screenZ);

            if (!Physics.CheckSphere(pos, 0.6f, LayerMask.GetMask("NPC")))
            {
                //Debug.Log("Collision");
                spawnPool[1].name = $"HostileNPC {i}";
                Instantiate(spawnPool[1], pos, Quaternion.identity);
            }
        }
    }
}