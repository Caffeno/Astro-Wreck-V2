using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTracker : MonoBehaviour
{


    public bool isPlayer = false;
    public int goldCollected = 0;

    private void Start()
    {
        if (GetComponent<Player>() != null)
        {
            isPlayer = true;
        }
    }

    public int GetScore()
    {
        int total = 0;
        total += goldCollected;
        return total;
    }
}
