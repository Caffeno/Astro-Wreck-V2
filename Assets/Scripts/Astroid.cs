using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{

    [SerializeField] private GameObject goldPrefab;
    private Rigidbody2D rb;
    private Rigidbody2D goldrb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        goldrb = goldPrefab.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool colliding = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (colliding) { return; }

        colliding = true;
        //Hit should probably be called here instead of on the player or whatever else
        Vector2 impact = collision.GetImpactForce();
        Hit(impact);

    }
    
    public void Hit(Vector2 impact)
    {
        //if Threshold to destroy reached?

        Vector3 goldSpawnLocation = rb.position + impact.normalized * 0.02f;
        Vector2 goldLaunchVelocity = Time.fixedDeltaTime * impact / goldrb.mass;

        GameObject newGold = Instantiate(goldPrefab, goldSpawnLocation, Quaternion.identity, GoldManager.instance.transform);
        Rigidbody2D newGoldrb = newGold.GetComponent<Rigidbody2D>();
        newGoldrb.velocity = goldLaunchVelocity / 5;
        GameObject.Destroy(gameObject);
        //spawn gold some random offset vector from impact
        //spawn position should be far enough away that the gold isn't just collected immediatly
        //destry astroid
    }
}
