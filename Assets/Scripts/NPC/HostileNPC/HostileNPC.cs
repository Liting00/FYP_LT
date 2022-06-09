using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HostileNPC : BasedNPC
{
    [SerializeField] 
    GameObject objToSpawn;

    private float update;

    private float nextUpdatedTime;

    [SerializeField]
    private float infectionRate;

    void Start()
    {
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

        /*foreach (GameObject o in GameObject.FindGameObjectsWithTag("NonHostileNPC"))
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
        }*/
        foreach (GameObject npc in influenceAreaNpcs)
        {
            //chances of infection

            if (Random.Range(0.0000f, 1f) <= infectionRate && npc != null) { 
                Debug.Log("Infect!");
                Vector3 spanLoc = npc.transform.position;
                Quaternion spawnRot = npc.transform.rotation;

                SpawnManager.Instance.addInfected(1);
                SpawnManager.Instance.addNonInfected(-1);

                objToSpawn.name = $"Infected {npc.name}";
                Destroy(npc.gameObject);
                //Debug.Log(npc.name + " is Destroy!");

                GameObject infectedNPC = Instantiate(objToSpawn, spanLoc, spawnRot) as GameObject;
                infectedNPC.GetComponent<NetworkObject>().Spawn();
                infectedNPC.name = infectedNPC.name.Replace("(Clone)", "");
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NonHostileNPC"))
        {
            influenceAreaNpcs.Add(other.gameObject);
            if(gameObject == TargetController.Instance.selectedNPC)
                AdvisorManager.Instance.highlightedNPCs.Add(other.gameObject);
            //Debug.Log("NPC Collision");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NonHostileNPC"))
        {
            influenceAreaNpcs.Remove(other.gameObject);
            if (gameObject == TargetController.Instance.selectedNPC)
                AdvisorManager.Instance.highlightedNPCs.Remove(other.gameObject);
            //Debug.Log("NPC Exit Collision");
        }
    }

    //time for it takes to do the next infection
    private float nextUpdate()
    {
        return Random.Range(5.0f, 10.0f);
    }

}
