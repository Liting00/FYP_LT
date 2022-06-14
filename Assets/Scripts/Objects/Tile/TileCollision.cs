using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollision : MonoBehaviour
{
    //script
    [SerializeField] Tile tileScript;

    /*private void OnMouseEnter()
    {
        tileScript.onHighlight(true);
    }
    private void OnMouseExit()
    {
        tileScript.onHighlight(false);
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "HostileNPC")
        {
            //GameObject tController = GameObject.Find("Target Controller");
            TargetController.Instance.collidedTile.Add(gameObject);
            //Debug.Log(gameObject.name + " hit!");
        }
        //TODO: Cannot make Collision Work
        /*if(TargetController.Instance.selectedTile == this.gameObject && (other.gameObject.tag == "NonHostileNPC" || other.gameObject.tag == "HostileNPC"))
        {
            Debug.Log(other.gameObject);
            TargetController.Instance.highlightedNPCs.Add(other.gameObject);
        }*/
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "HostileNPC")
        {
            //GameObject tController = GameObject.Find("Target Controller");
            TargetController.Instance.collidedTile.Remove(gameObject);
            //Debug.Log(gameObject.name + " Remove!");
        }
        //TODO: Cannot make Collision Work
        /*if (TargetController.Instance.selectedTile == this.gameObject && (other.gameObject.tag == "NonHostileNPC" || other.gameObject.tag == "HostileNPC"))
        {
            TargetController.Instance.highlightedNPCs.Remove(other.gameObject);
        }*/
    }
    /*public void OnMouseDown()
    {
        float currentX = tileScript._highlight.transform.position.x - 0.5f;
        float currentZ = tileScript._highlight.transform.position.z - 0.5f;

        Debug.Log("Vector X: " + currentX + " Vector Z: " + currentZ);
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
    }*/
}
