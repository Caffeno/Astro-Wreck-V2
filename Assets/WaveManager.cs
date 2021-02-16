using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private int waveNumber = 0;
    private IEnumerator waveSpawnerCoroutine;

    private AstroidManager astroidManager;

    private GameEvents eventManager;
    // Start is called before the first frame update
    private void Awake()
    {

    }

    void Start()
    {
        eventManager = GameEvents.instance;
        eventManager.onPlayerDeathEnter += Death;
        astroidManager = AstroidManager.instance;
        waveSpawnerCoroutine = SpawnWave();
        StartCoroutine(waveSpawnerCoroutine);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnWave()
    {
        float waveDelay = 15; //Probablly not going to just be static
        do
        {
            waveNumber += 1;
            eventManager.StartWave(waveNumber);
            //astroidManager.SpawnAstroids(waveNumber);
            //Display wave number (possibly vary it on milestones)
            //Spawn all the stuff from the wave (Deciding what to spawn, how much of each to spawn, where, and what direction)(how to decide?)
            yield return new WaitForSeconds(waveDelay);

        } while (true);
    }

    void Death()
    {
        StopCoroutine(waveSpawnerCoroutine);
    }
}
