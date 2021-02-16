using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidManager : MonoBehaviour
{
    public static AstroidManager instance;
    // Start is called before the first frame update
    public float spawnDelay = 2f;
    public float spawnRadiusMin = 15f;
    public float spawnRadiusThickness = 5;

    public float launchSpeedMin = 1;
    public float launchSpeedRange = 3;

    private Player ship;
    private Rigidbody2D shipRB;

    private IEnumerator astroidSpawnCoroutine;

    [SerializeField] private GenericLootDropGameObjectTable astroidSpawnTable;

    private void OnValidate()
    {
        astroidSpawnTable.ValidateTable();
    }

    private void Awake()
    {

        astroidSpawnCoroutine = PassiveAstroidSpawns();
        StartCoroutine(astroidSpawnCoroutine);
        instance = this;
    }
    void Start()
    {
        GameEvents.instance.waveStartTrigger += SpawnAstroids;
        ship = FindObjectOfType<Player>();
        shipRB = ship.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SpawnAstroids(int waveNumber)
    {
        int clusterCount = waveNumber % 5 + Random.Range(0, 3);
        int clusterAstroids = Random.Range(5, 10);
        float clusterSize = 3 + .75f * clusterAstroids;
        int failedPlacementAttempts = 0;
        int placedClusters = 0;
        
        while (failedPlacementAttempts < 5 && placedClusters < clusterCount)
        {
            float distance = Random.Range(20f, 30f);
            Vector2 location = Random.insideUnitCircle.normalized * distance + shipRB.position;
            Collider2D spawnArea = Physics2D.OverlapCircle(location, clusterSize, LayerMask.GetMask("astroids"));
            if(spawnArea == false)
            {
                placedClusters += 1;
                Vector2 velocity = Random.insideUnitCircle.normalized * Random.Range(3f, 10f);
                Debug.Log("Cluster Placed At " + location + Random.insideUnitCircle.normalized * distance  + ":Relitive to player");

                int astroidAttempts = 0;
                int astroidsPlaced = 0;
                while (astroidAttempts < 3 && astroidsPlaced < clusterAstroids)
                {
                    Vector2 astroidSpawnLocation = Random.insideUnitCircle * clusterSize + location;
                    GenericLootDropGameObject astroidType = astroidSpawnTable.GetRandomItem();
                    GameObject astroidPrefab = astroidType.item;
                    Collider2D astroidSpace = Physics2D.OverlapCircle(astroidSpawnLocation, astroidPrefab.transform.localScale.magnitude * astroidPrefab.GetComponent<CircleCollider2D>().radius, LayerMask.GetMask("Astroids"));
                    if (astroidSpace == false)
                    {
                        GameObject astroid = Instantiate(astroidPrefab, astroidSpawnLocation, Quaternion.identity, gameObject.transform);
                        Rigidbody2D astroidRB = astroid.GetComponent<Rigidbody2D>();
                        astroidRB.velocity = velocity;
                        astroidsPlaced += 1;
                    }
                    else { astroidAttempts += 1; }

                }
            }
            else
            {
                failedPlacementAttempts += 1;
                Debug.Log("Failed Because Of " + spawnArea.attachedRigidbody.gameObject.name);
            }
        }
    }

    
    private IEnumerator PassiveAstroidSpawns()
    {
        yield return new WaitForSeconds(spawnDelay);
        do
        {
            GenericLootDropGameObject astroidType = astroidSpawnTable.GetRandomItem();
            Vector2 spawnPosition = new Vector2(Random.value - 0.5f, Random.value - 0.5f).normalized * (spawnRadiusMin + spawnRadiusThickness * Random.value);
            GameObject newAstroid = Instantiate(astroidType.item, spawnPosition + shipRB.position, Quaternion.identity, transform);
            Rigidbody2D RB = newAstroid.GetComponent<Rigidbody2D>();

            Vector2 aimSpot = new Vector2(Random.value - 0.5f, Random.value - 0.5f).normalized * 6 + shipRB.position;
            Vector2 launchDirection = aimSpot - RB.position;

            RB.velocity = (launchDirection.normalized * (launchSpeedMin + launchSpeedRange * Random.value));
            yield return new WaitForSeconds(spawnDelay + Random.value);

        } while (true);

    }
    
}
