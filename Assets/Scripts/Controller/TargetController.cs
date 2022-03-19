using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    private string target = "HostileNPC";

    public Transform aim;
    public float range = 15f;

    void UpdateTarget()
    {
        GameObject[] hostileNPCS = GameObject.FindGameObjectsWithTag(target);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy;
        float npcPosX, npcPosZ;

        foreach (GameObject npc in hostileNPCS)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, npc.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                npcPosX = npc.transform.position.x;
                npcPosZ = npc.transform.position.z;
            }
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTarget();
    }
}
