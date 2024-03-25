using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    #region Variables
    [Header("Pools to Draw From")]
    [SerializeField] 
    private CollectiblePooling collectibleOnePool;
    [SerializeField] 
    private CollectiblePooling collectibleTwoPool;
    [SerializeField] 
    private CollectiblePooling collectibleThreePool;

    [SerializeField]
    private GameObject[] spawnPoints;

    [Header("Spawn Settings")]
    [SerializeField]
    private GameObject spawnTarget;
    [SerializeField] 
    private float spawnRate = 5;

    private bool readyToSpawn = true;
    private int specialSpawnTurn; // How many turns until collectible three spawns
    private int currentSpawnTurn = 0;
    #endregion

    #region Start and Update
    // Start is called before the first frame update
    void Start()
    {
        if(collectibleOnePool == null || collectibleTwoPool == null || collectibleThreePool == null)
        {
            Debug.LogError("One of the pools in the spawner is not assigned");
        }

        if(spawnPoints.Length <= 0 )
        {
            Debug.LogError("Must add spawn points");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(readyToSpawn)
        {
            if (currentSpawnTurn < specialSpawnTurn)
            {
                StartCoroutine(NormalSpawn());
            }
            else
            {
                StartCoroutine(SpecialSpawn());
            }
        }
    }
    #endregion

    #region Spawn Handling Methods
    private IEnumerator NormalSpawn()
    {
        readyToSpawn = false;
        currentSpawnTurn++;

        // Choose random normal pool
        int poolNumber = Random.Range(0, 2);
        Collectible spawnedCollectible = null;

        if(poolNumber == 0)
        {
            spawnedCollectible = collectibleOnePool.pool.Get();
        }
        else if(poolNumber == 1)
        {
            spawnedCollectible = collectibleTwoPool.pool.Get();
        }
        else
        {
            Debug.LogError("Pool Number in the Spawner script is out of range");
        }

        SpawnedEntityProcessing(spawnedCollectible);

        yield return new WaitForSeconds(spawnRate);
        readyToSpawn = true;
    }

    private IEnumerator SpecialSpawn()
    {
        readyToSpawn = false;
        currentSpawnTurn = 0;

        Collectible spawnedColledctible = collectibleThreePool.pool.Get();
        SpawnedEntityProcessing(spawnedColledctible);

        yield return new WaitForSeconds(spawnRate);
        readyToSpawn = true;
    }

    private void SpawnedEntityProcessing(Collectible spawnedObject)
    {
        // Assign position
        int randomPosition = Random.Range(0, spawnPoints.Length);
        spawnedObject.transform.position = spawnPoints[randomPosition].transform.position;

        // Assign target to spawn agent, agent only works if you turn it off then on again
        NavMeshAgent spawnAgent = spawnedObject.GetComponent<NavMeshAgent>();
        spawnAgent.enabled = false;
        spawnedObject.GetComponent<FollowPlayer>().Target = spawnTarget;
        spawnAgent.enabled = true;
    }

    private void UpdateSpecialSpawn(int turn)
    {
        if(turn == 0)
        {
            Debug.Log("Turn to special spawn was attempted to be 0, current turn is: " + specialSpawnTurn);
            return;
        }
        else
        {
            specialSpawnTurn = turn;
        }
    }
    #endregion

    #region OnEnable and OnDisable
    private void OnEnable()
    {
        Points.TurnsToSpecialSpawn += UpdateSpecialSpawn;
    }

    private void OnDisable()
    {
        Points.TurnsToSpecialSpawn -= UpdateSpecialSpawn;
    }
    #endregion
}
