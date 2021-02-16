using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GenericLootDropGameObjectTable enemyTable;
    Player ship;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.instance.waveStartTrigger += SpawnWave;
        ship = Player.instance;
    }

    private void OnValidate()
    {
        enemyTable.ValidateTable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnWave(int waveNumber)
    {
        //logic should change based on wave number at some point (spawn types and such)

        float pointsOfEnemies = 0;
        int unitsSpawned = 0;
        while (pointsOfEnemies < waveNumber)
        {
            GenericLootDropGameObject enemyToSpawn = enemyTable.GetRandomItem();
            pointsOfEnemies += enemyToSpawn.probabilityWeight;
            Instantiate(enemyToSpawn.item, new Vector3(Random.value - 0.5f, Random.value - 0.5f, 0).normalized * Random.Range(20f, 30f) + ship.transform.position, Quaternion.identity, gameObject.transform);
        }

        Debug.Log("Units Spawned: " + unitsSpawned);


    }
}
