using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HostileNPC : BasedNPC
{
    [SerializeField] GameObject objToSpawn;

    private float update;

    private float nextUpdatedTime;

    [SerializeField]
    private float infectionRate;

    void Start()
    {
        //** need to optimize this
        nextUpdatedTime = nextUpdate();
    }
    void Update()
    {
        if (!IsServer) return;

        update += Time.deltaTime;
        if (update > nextUpdatedTime)
        {
            infection();
        }
    }
    private void infection()
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
                if (Random.Range(0.0000f, 1f) <= infectionRate)
                {
                    Vector3 spanLoc = o.transform.position;
                    Quaternion spawnRot = o.transform.rotation;
                    SpawnManager.Instance.addInfected(1);
                    SpawnManager.Instance.addNonInfected(-1);
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
    private float nextUpdate()
    {
        return Random.Range(5.0f, 10.0f);
    }

}
