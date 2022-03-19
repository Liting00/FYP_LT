using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollision : MonoBehaviour
{
    [SerializeField] Tile tileScript;
    public void OnMouseEnter()
    {
        tileScript.highlight.SetActive(true);
    }
    public void OnMouseExit()
    {
        tileScript.highlight.SetActive(false);
    }
    public void OnMouseDown()
    {
        float currentX = tileScript.highlight.transform.position.x - 0.5f;
        float currentZ = tileScript.highlight.transform.position.z - 0.5f;

        //Debug.Log("Vector X: " + currentX + " Vector Z: " + currentZ);
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("HostileNPC"))
        {
            //Debug.Log("Vector X: " + o.transform.position.x);
            //Debug.Log("Vector Y: " + o.transform.position.y);
            if (o.transform.position.x >= currentX && o.transform.position.x <= currentX + 1f && o.transform.position.z >= currentZ && o.transform.position.z <= currentZ + 1f)
            {
                Destroy(o);
            }

        }
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("NonHostileNPC"))
        {
            //Debug.Log("Vector X: " + o.transform.position.x);
            //Debug.Log("Vector Y: " + o.transform.position.y);
            if (o.transform.position.x >= currentX && o.transform.position.x <= currentX + 1f && o.transform.position.z >= currentZ && o.transform.position.z <= currentZ + 1f)
            {
                Destroy(o);
            }

        }
    }
}
