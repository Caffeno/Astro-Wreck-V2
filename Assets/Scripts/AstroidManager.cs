using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float spawnDelay = 5;
    public float spawnRadiusMin = 15f;
    public float spawnRadiusThickness = 5;

    public float launchSpeedMin = 1;
    public float launchSpeedRange = 3;
    [SerializeField] private GameObject astroidPrefab;

    private Player ship;
    private Rigidbody2D shipRB;

    private IEnumerator astroidSpawnCoroutine;
    void Start()
    {
        ship = FindObjectOfType<Player>();
        shipRB = ship.GetComponent<Rigidbody2D>();

        astroidSpawnCoroutine = SpawnAsteroids();
        StartCoroutine(astroidSpawnCoroutine);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnAsteroids()
    {
        yield return new WaitForSeconds(spawnDelay);
        do
        {
            Vector2 spawnPosition = new Vector2(Random.value - 0.5f, Random.value - 0.5f).normalized * (spawnRadiusMin + spawnRadiusThickness * Random.value);
            GameObject newAstroid = Instantiate(astroidPrefab, spawnPosition + shipRB.position, Quaternion.identity, transform);
            Rigidbody2D RB = newAstroid.GetComponent<Rigidbody2D>();

            Vector2 aimSpot = new Vector2(Random.value - 0.5f, Random.value - 0.5f).normalized * 6 + shipRB.position;
            Vector2 launchDirection = aimSpot - RB.position;

            RB.velocity = (launchDirection.normalized * (launchSpeedMin + launchSpeedRange * Random.value));
            yield return new WaitForSeconds(spawnDelay);

        } while (true);

    }
}
