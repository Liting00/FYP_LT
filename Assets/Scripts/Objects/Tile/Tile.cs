using UnityEngine;

public class Tile : MonoBehaviour
{
    //[SerializeField] internal Color _baseColor, _offsetColor;
    [SerializeField] protected MeshRenderer _renderer;
    [SerializeField] internal GameObject _highlight;

    public Material[] material;
    //script
    //[SerializeField] internal TileCollision tileCollisionScript;
    private void Awake()
    {
        GetComponent<Renderer>().material = material[GameSettings.TILE_SELECT];
    }
    public void changeMaterial()
    {
        GetComponent<Renderer>().material = material[GameSettings.TILE_SELECT];
    }
    private void Init(bool isOffset)
    {
        //not used
        //_renderer.material.color = isOffset ? _offsetColor : _baseColor;
    }
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