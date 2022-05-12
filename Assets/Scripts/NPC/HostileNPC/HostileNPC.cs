using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HostileNPC : BasedNPC
{
    [SerializeField] GameObject objToSpawn;

    private float update;

    private float nextUpdatedTime;
    SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        //** need to optimize this
        GameObject SManager = GameObject.Find("Spawn Manager");
        spawnManager = SManager.GetComponent<SpawnManager>();
        nextUpdatedTime = nextUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer) return;

        update += Time.deltaTime;
        if (update > nextUpdatedTime)
        {
            infection();
        }
    }
    void infection()
    {
        //Debug.Log("Start Infection");
        nextUpdatedTime = nextUpdate();
        update = 0.0f;
        Vector3 pos = transform.position;
        //Debug.Log(pos);

        foreach (GameObject o in GameObject.FindGameObjectsWithTag("NonHostileNPC"))
        {
            if (o.transform.position.x >= pos.x -0.5f && o.transform.position.x <= pos.x + 0.5f 
                && o.transform.position.z >= pos.z - 0.5f && o.transform.position.z <= pos.z + 0.5f)
            {
                //chances of infection
                if (Random.Range(0.0000f, 1f) <= 1f)
                {
                    Vector3 spanLoc = o.transform.position;
                    Quaternion spawnRot = o.transform.rotation;
                    int infected = spawnManager.getInfected();
                    spawnManager.setInfected(infected - 1);
                    int nonInfected = spawnManager.getNonInfected();
                    spawnManager.setInfected(nonInfected + 1);
                    //spawnManager.nonInfected--;
                    //spawnManager.infected++;
                    objToSpawn.name = $"Infected {o.name}";
                    Destroy(o);
                    //Debug.Log(o.name + " is Destroy!");
                    GameObject infectedNPC = Instantiate(objToSpawn, spanLoc, spawnRot) as GameObject;
                    infectedNPC.GetComponent<NetworkObject>().Spawn();
                    infectedNPC.name = infectedNPC.name.Replace("(Clone)", "");
                }
            }
        }
    }

    //time for it takes to do the next infection
    float nextUpdate()
    {
        return Random.Range(5.0f, 10.0f);
    }

}
