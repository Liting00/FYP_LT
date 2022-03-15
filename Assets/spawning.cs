using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawning : MonoBehaviour
{
    public GameObject[] npcs;
    public Transform npc_transform;

    public int count;

    private float xPos;
    private float zPos;

    private void Start()
    {
        StartCoroutine(SpawnCount());
    }

    IEnumerator SpawnCount()
    {
        while (count < 20)
        {
            xPos = Random.Range(-10f, 10f);
            zPos = Random.Range(-10f, 10f);
            int npcs_Index = Random.Range(0, npcs.Length);
            Collider[] Collision = Physics.OverlapSphere(npc_transform.position, 1f, LayerMask.GetMask("npc"));
            if (Collision.Length == 0)
            {
                Instantiate(npcs[npcs_Index], new Vector3(xPos, 0f, zPos), Quaternion.identity);
                Debug.Log("No Collision");
                count++;
            }
            yield return new WaitForSeconds(1f);
            Debug.Log("Collision happened");
        }
    }

}

