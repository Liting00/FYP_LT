using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //[SerializeField] internal Color _baseColor, _offsetColor;
    [SerializeField] protected MeshRenderer _renderer;
    [SerializeField] internal GameObject _highlight;

    public Material[] material;
    //script
    //[SerializeField] internal TileCollision tileCollisionScript;

    private void Start()
    {
        GetComponent<Renderer>().material = material[GameSettings.TILE_SELECT];
    }
    public void changeMaterial()
    {
        GetComponent<Renderer>().material = material[GameSettings.TILE_SELECT];
    }
    public void Init(bool isOffset)
    {
        //not used
        //_renderer.material.color = isOffset ? _offsetColor : _baseColor;
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
    public void onHighlight(bool onHighlight)
    {
        if (onHighlight == true)
            _highlight.SetActive(true);
        else
            _highlight.SetActive(false);
    }
    public void redHighlight(bool onHighlight)
    {
        if (onHighlight == true)
            GetComponent<Renderer>().material.color = Color.red;
        else
            GetComponent<Renderer>().material = material[GameSettings.TILE_SELECT];

    }

}
