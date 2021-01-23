using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    private Player ship;
    private Rigidbody2D rb;
    private Rigidbody2D shiprb;
    private float weakThrust = 1000;
    private float strongThrust = 30000;
    private float thrust = 10;
    private float thrustAngleTarget = 10;
    private float rotationSpeed = 50;

    void Start()
    {
        ship = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
        shiprb = ship.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       



        //transform.forward
    }

    private void FixedUpdate()
    {
        Vector2 offset = shiprb.position - rb.position;
        Vector2 forward = new Vector2(transform.up.x, transform.up.y);



        
        float targetRotation = -Mathf.Atan2(offset.x, offset.y) * 180 / Mathf.PI;

        thrust = Mathf.Abs(Mathf.DeltaAngle(targetRotation, rb.rotation)) < thrustAngleTarget ? strongThrust: weakThrust;

        rb.MoveRotation(Mathf.MoveTowardsAngle(rb.rotation, targetRotation, Time.deltaTime * rotationSpeed));
        
        rb.AddForce(forward * thrust * Time.deltaTime);
    }
}
