using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{

    [Range(0, 100)] public float explosionForce = 25f;
    [Range(0,100)][SerializeField] private float rotationSpeed = 50f;
    [Range(0,100)][SerializeField] private float thrustStrength = 10f;


    private Rigidbody2D rb;

    private float xin = 0f;
    private float yin = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.contacts[0].otherCollider;

        BreakApart();
        
    }
    
    private void Update()
    {
        yin = Input.GetAxisRaw("Vertical");
        xin = Input.GetAxisRaw("Horizontal");
        rb.AddForce(Time.deltaTime * yin * transform.up * thrustStrength);
        rb.rotation -= xin * Time.deltaTime * rotationSpeed;
    }

    private void FixedUpdate()
    {


    }

    private void BreakApart()
    {
        ExplodingCore core = null;
        foreach(Transform child in transform)
        {
            Rigidbody2D RB = child.gameObject.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
            RB.gravityScale = 0f;
            RB.drag = 3f;
            RB.angularDrag = 3f;
            if (core == null)
            {
                core = child.GetComponent<ExplodingCore>();
                if(core != null)
                {
                    StartCoroutine(FrictionDrop(RB));

                }
            }
        }
        transform.DetachChildren();
        if (core != null)
        {
            core.Explode(explosionForce);

        }
        //gameObject.SetActive(false);
    }

    private IEnumerator FrictionDrop(Rigidbody2D body)
    {
        Debug.Log("Lowering Drag for " + body.gameObject.name);
        float vMax = 0;
        float vMin = 0;
        float aMax = 0;
        float aMin = 0;
        while (vMax == 0)
        {
            vMax = body.velocity.magnitude;
            vMin = vMax * 0.1f;
            aMax = body.angularVelocity;
            aMin = aMax * 0.1f;

            yield return new WaitForEndOfFrame();
        }
        float initalDrag = body.drag;
        float initalaDrag = body.angularDrag;
        while (body.drag != 0 || body.angularDrag != 0)
        {
           
            float drag = initalaDrag * (body.velocity.magnitude - vMin) / (vMax - vMin);
            float adrag = initalaDrag * (body.angularVelocity - aMin) / (aMax - aMin);

            drag = drag < 0.01 ? 0 : drag;
            adrag = adrag < 0.01 ? 0 : adrag;

            body.drag = drag;
            body.angularDrag = adrag;
            
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Drag Gone for " + body.gameObject.name);

    }
}
