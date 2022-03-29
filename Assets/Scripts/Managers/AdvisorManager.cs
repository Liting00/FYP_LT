using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvisorManager : MonoBehaviour
{
    string[] advise = { "Destroy", "Don't Destroy", "Destroy when alone" };
    [SerializeField] internal GameObject advisorBox;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void getAdvise()
    {
        advisorBox.GetComponent<UnityEngine.UI.Text>().text = advise[Random.Range(0, advise.Length)].ToString();
    }


}
