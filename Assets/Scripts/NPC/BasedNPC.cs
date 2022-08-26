using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Network.Singletons;
public class BasedNPC : NetworkSingleton<BasedNPC>
{
    public float moveSpeed = 0.25f;
    public float rotSpeed = 100f;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;

    internal List<GameObject> influenceAreaNpcs = new List<GameObject>();

    void Update()
    {
        if (isWandering == false)
        {
            StartCoroutine(Wander());
        }
        if (isRotatingRight == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * rotSpeed);
        }
        if (isRotatingLeft == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * -rotSpeed);
        }
        if (isWalking == true)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    IEnumerator Wander()
    {
        // TODO: Adjust this
        int rotTime = Random.Range(1, 3);
        float rotateWait = Random.Range(1f, 4f);
        int rotateLorR = Random.Range(1, 2);
        float walkWait = Random.Range(1f, 5f);
        float walkTime = Random.Range(1f, 5f);

        isWandering = true;

        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;
        yield return new WaitForSeconds(rotateWait);
        if (rotateLorR == 1)
        {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingRight = false;
        }
        if (rotateLorR == 2)
        {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingLeft = false;
        }
        isWandering = false;
    }
}
