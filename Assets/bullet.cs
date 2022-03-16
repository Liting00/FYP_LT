using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject, 3);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
