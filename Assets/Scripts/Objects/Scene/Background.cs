using System.Collections;
using UnityEngine;


public class Background : MonoBehaviour
{

    public Material[] material;

    public GameObject backgroundPlane;

    // Use this for initialization
    void Awake()
    {
        backgroundPlane.GetComponent<Renderer>().material = material[GameSettings.BACKGROUND_SELECT];
    }
}