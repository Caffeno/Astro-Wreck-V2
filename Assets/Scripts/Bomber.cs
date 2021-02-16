using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    [SerializeField] private GameObject goldPrefab;


    private Player ship;
    private Rigidbody2D rb;
    private Rigidbody2D shiprb;
    private LootTracker loot;
    private float weakThrust = 1000;
    private float strongThrust = 30000;
    private float thrust = 10;
    private float thrustAngleTarget = 10;
    private float rotationSpeed = 80;

    private void Awake()
    {
        loot = GetComponent<LootTracker>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        ship = Player.instance;
        shiprb = ship.rb;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private bool colliding = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider);
        Debug.Log(Time.realtimeSinceStartup);
        if (colliding) { return; }

        StartCoroutine("CollideReady");
        if (collision.gameObject.GetComponentInParent<Player>() == ship)
        {
            if (collision.gameObject.GetComponent<ExplodingEngine>() == null)
            {
                colliding = true;

                Hit(collision.GetImpactForce());
            }
        }
    }

    private IEnumerator CollideReady()
    {
        yield return new WaitForEndOfFrame();

        colliding = false;
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

    private void Hit(Vector2 impact)
    {
        float impactStrength = impact.magnitude;

        float impactAngle = Mathf.Atan2(impact.y, impact.x) * Mathf.Rad2Deg;


        for (int i = 0; i < loot.goldCollected + 5; i++)
        {
            float goldLaunchAngle = impactAngle + (Random.value - 0.5f) * 30f;

            Vector3 goldLaunchVector = new Vector3(Mathf.Cos(goldLaunchAngle * Mathf.Deg2Rad), Mathf.Sin(goldLaunchAngle * Mathf.Deg2Rad), 0f);


            GameObject gold = Instantiate(goldPrefab, transform.position, Quaternion.identity, GoldManager.instance.transform);
            Rigidbody2D goldRB = gold.GetComponent<Rigidbody2D>();
            Vector2 goldVelocity = Time.deltaTime * goldLaunchVector * impact.magnitude / goldRB.mass;
            goldRB.velocity = goldVelocity * (Random.value * 0.2f + 0.9f);

            //spawn a gold with some ofset from impact
        }
        GameObject.Destroy(gameObject);
    }
}
