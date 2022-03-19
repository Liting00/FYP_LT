using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] protected MeshRenderer _renderer;
    [SerializeField] GameObject _highlight;

    //[SerializeField] GameObject greyNPC;

    private void Awake()
    {
    }

    public void Init(bool isOffset)
    {
        _renderer.material.color = isOffset ? _offsetColor : _baseColor;
    }

    public void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }
    public void OnMouseExit()
    {
        _highlight.SetActive(false);
    }
    public void OnMouseDown()
    {
        float currentX = _highlight.transform.position.x - 0.5f;
        float currentZ = _highlight.transform.position.z - 0.5f;

        //Debug.Log("Vector X: " + currentX + " Vector Z: " + currentZ);
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("npc"))
        {
            //Debug.Log("Vector X: " + o.transform.position.x);
            //Debug.Log("Vector Y: " + o.transform.position.y);
            if (o.transform.position.x >= currentX && o.transform.position.x <= currentX + 1f && o.transform.position.z >= currentZ && o.transform.position.z <= currentZ + 1f)
            {
                Destroy(o);
            }

        }
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("npc"))
        {
            //Debug.Log("Vector X: " + o.transform.position.x);
            //Debug.Log("Vector Y: " + o.transform.position.y);
            if (o.transform.position.x >= currentX && o.transform.position.x <= currentX + 1f && o.transform.position.z >= currentZ && o.transform.position.z <= currentZ + 1f)
            {
                Destroy(o);
            }

        }
        //Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(p);



        //Vector2 mousePos = Input.mousePosition;
        //Vector2 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
        //RaycastHit2D hit = Physics2D.Raycast(screenPos, Vector2.zero);
        //if (hit.collider.tag == "GreyNPC")
        //{
        //    Debug.Log("Destroyed");
        //    Destroy(hit.collider.gameObject);
        //}

        //Vector2 mousePos = Input.mousePosition;
        //Vector2 screenPos = Camera.main.ScreenToWorldPoint(mousePos);
        //RaycastHit2D hit = Physics2D.Raycast(screenPos, Vector2.zero);
        //float targetX = hit.collider.transform.position.x;
        //float targetY = hit.collider.transform.position.y;
        //Debug.Log(targetX + " "+ targetY);

        //Debug.Log(currentX.ToString() + ',' + currentY.ToString());

        //get object that is under these x and y


        //destroy objects that meet this certain position
        //if (transform.position.x >= currentX && transform.position.x <= currentX + 1f && transform.position.y >= currentY && transform.position.y <= currentY + 1f)
        //{
        //    //Debug.Log("Left Click is pressed");
        //    Destroy(GameObject.FindGameObjectWithTag("GreyNPC"));
        //}
    }
    private void Update()
    {

    }

}
