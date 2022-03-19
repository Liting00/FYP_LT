using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    private string target = "HostileNPC";
    // Start is called before the first frame update
    void UpdateTarget()
    {
        GameObject[] hostileNPCS = GameObject.FindGameObjectsWithTag(target);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject npc in hostileNPCS)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, npc.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = npc;
                Debug.Log(nearestEnemy);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTarget();
    }
}
