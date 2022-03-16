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
            xPos = Random.Range(-5f, 5f);
            zPos = Random.Range(-5f, 5f);
            int npcs_Index = Random.Range(0, npcs.Length);
            if ((!Physics.CheckSphere(new Vector3(xPos, 0.5f, zPos), 1f, LayerMask.GetMask("npc"))))
            {
                Instantiate(npcs[npcs_Index], new Vector3(xPos, 0.5f, zPos), Quaternion.identity);
                Debug.Log("No Collision");
                count++;
            }
            yield return new WaitForSeconds(1f);
            Debug.Log("Collision happened");
        }
    }

}

