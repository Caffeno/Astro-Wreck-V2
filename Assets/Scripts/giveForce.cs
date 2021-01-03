using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giveForce : MonoBehaviour
{
    [Range(0, 1000)] public float force = 10;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D RB = GetComponent<Rigidbody2D>();
        RB.AddForce(new Vector2(0, -1) * force);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
