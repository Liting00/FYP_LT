using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TerminateTimer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TerminateTimer()
    {
        yield return new WaitForSeconds(3f);
        Application.Quit();
    }
}
