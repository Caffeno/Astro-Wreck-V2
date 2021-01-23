using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float teatherCastRadius = .5f;
    public float teatherCastRange = 5;
    public LayerMask teatherableMask;
    [Range(0, 100)] public float explosionForce = 25f;
    [Range(0,100)][SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float thrustStrength = 10f;

    [SerializeField] private GameObject playerCamera;


    private CenterOfMassTracker centerOfMass;

    private Rigidbody2D rb;
    private FixedJoint2D teather;
    private float xin = 0f;
    private float yin = 0f;
    private bool alive = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        teather = GetComponent<FixedJoint2D>();
        centerOfMass = FindObjectOfType<CenterOfMassTracker>();
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 impact = collision.GetImpactForce();

        Collider2D collider = collision.contacts[0].otherCollider;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            ExplodingEngine engine = collider.GetComponent<ExplodingEngine>();
            if (engine != null) { BreakApart(); return; }
        }

        Debug.Log(collision.collider);

        Astroid asteroid = collision.collider.gameObject.GetComponent<Astroid>();
        Debug.Log(asteroid);

        if (asteroid != null)
        {
            asteroid.Hit(-impact);
        }
        //GameObject.Destroy(collision.contacts[0].rigidbody.gameObject);
    }


    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        yin = Input.GetAxisRaw("Vertical");
        xin = Input.GetAxisRaw("Horizontal");
        if (yin != 0f)
        {
            rb.AddForce(Time.deltaTime * yin * transform.up * thrustStrength * rb.mass);
        }

        if(xin != 0 && !teather.enabled)
        {
            AttemptTeather(xin);
        }
        else if (teather.enabled && xin == 0)
        {
            Detach();
        }

        //rb.rotation -= xin * Time.deltaTime * rotationSpeed;  Manual rotation

    }

    private void Detach()
    {
        teather.enabled = false;
        //rb.freezeRotation = true;
        centerOfMass.Detach();
    }

    private void BreakApart()
    {
        alive = false;
        playerCamera.SetActive(false);
        Detach();
        ExplodingCore core = null;
        foreach(Transform child in transform)
        {
            Rigidbody2D RB = child.gameObject.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
            RB.gravityScale = 0f;
            RB.drag = 3f;
            RB.angularDrag = 3f;
            RB.interpolation = RigidbodyInterpolation2D.Interpolate;
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
    }

    private void AttemptTeather(float direction)
    {
        Vector2 castDir = (transform.right * direction).normalized;
        RaycastHit2D castResult = Physics2D.CircleCast(rb.position + castDir * teatherCastRadius, teatherCastRadius, castDir, teatherCastRange, teatherableMask);
        if (castResult)
        {
            teather.enabled = true;
            teather.connectedBody = castResult.rigidbody;
            centerOfMass.Attach(castResult.rigidbody);
            rb.freezeRotation = false;
        }
    }
}
