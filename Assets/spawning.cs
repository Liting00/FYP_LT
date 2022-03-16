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
    private float time;

    private void Start()
    {
        StartCoroutine(SpawnCount());
    }

    IEnumerator SpawnCount()
    {
        while (count < 20)
        {
            xPos = Random.Range(-1f, 1f);
            zPos = Random.Range(-1f, 1f);
            time = Random.Range(1f, 10f);
            int npcs_Index = Random.Range(0, npcs.Length);
            if ((!Physics.CheckCapsule(new Vector3(xPos, 0.5f, zPos), new Vector3(xPos, 0.5f, zPos), 1f, LayerMask.GetMask("npc"))))
            {
                Instantiate(npcs[npcs_Index], new Vector3(xPos, 0.5f, zPos), Quaternion.identity);
                Debug.Log("No Collision");
                count++;
            }
            yield return new WaitForSeconds(time);
            Debug.Log("Collision happened");
        }
    }

}

