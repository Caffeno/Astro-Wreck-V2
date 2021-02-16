using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gold : MonoBehaviour
{
    // Start is called before the first frame update

    public static Action scoreChangeEvent;
    //public static Dictionary<GameObject, int> goldCollected = new Dictionary<GameObject, int>();
    //public static int goldCollected = -1;
    private GameObject player;

    void Start()
    {

        player = FindObjectOfType<Player>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LootTracker loot = collision.gameObject.GetComponentInParent<LootTracker>();

        if (loot != null)
        {
            loot.goldCollected += 1;
            if (loot.isPlayer)
            {
                GameEvents.instance.UpdateScore();
            }
            GameObject.Destroy(gameObject);

        }

    }
}
