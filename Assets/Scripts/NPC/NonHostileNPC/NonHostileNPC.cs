using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonHostileNPC : BasedNPC
{
    // Start is called before the first frame update
    void Start()
    {
        CapsuleCollider myCollider = transform.GetComponent<CapsuleCollider>();
        myCollider.radius = GameSettings.NPC_SIZE;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
