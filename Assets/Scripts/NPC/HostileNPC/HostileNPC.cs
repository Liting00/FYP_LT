using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileNPC : BasedNPC
{
    [SerializeField] GameObject objToSpawn;

    private float update;

    private float nextUpdatedTime;

    // Start is called before the first frame update
    void Start()
    {
        nextUpdatedTime = nextUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        update += Time.deltaTime;
        if (update > nextUpdatedTime)
        {
            Debug.Log("Start Infection");
            nextUpdatedTime = nextUpdate();
            update = 0.0f;
            Vector3 pos = transform.position;
            //Debug.Log(pos);

            foreach (GameObject o in GameObject.FindGameObjectsWithTag("NonHostileNPC"))
            {
                if (o.transform.position.x >= pos.x && o.transform.position.x <= pos.x + 1f && o.transform.position.z >= pos.z && o.transform.position.z <= pos.z + 1f)
                {
                    //chances of infection
                    float chances = Random.Range(0.0000f, 1f);
                    if (chances <= 0.110f)
                    {
                        Vector3 spanLoc = o.transform.position;
                        Quaternion spawnRot = o.transform.rotation;
                        //Debug.Log(spanLoc);
                        Destroy(o);
                        Instantiate(objToSpawn, spanLoc, spawnRot);
                    }

                }
            }
        }
    }
    //time for it takes to do the next infection
    float nextUpdate()
    {
        return Random.Range(5.0f, 15.0f);
    }
}