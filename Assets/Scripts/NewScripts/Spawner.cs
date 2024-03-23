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
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        if(collectibleOnePool == null || collectibleTwoPool == null || collectibleThreePool == null)
        {
            Debug.LogError("One of the pools in the spawner is not assigned");
        }

        if(spawnPoints.Length >= 0 )
        {
            Debug.LogError("Must add spawn points");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (readyToSpawn)
        {
            StartCoroutine(Spawn());
        }
    }

    #region Enumerator
    private IEnumerator Spawn()
    {
        readyToSpawn = false;
        Collectible spawnedCollectible = collectibleOnePool.pool.Get();
        
        // Assign position
        int randomPosition = Random.Range(0, spawnPoints.Length);
        spawnedCollectible.transform.position = spawnPoints[randomPosition].transform.position;

        // Assign target to spawn agent, agent only works if you turn it off then on again
        NavMeshAgent spawnAgent = spawnedCollectible.GetComponent<NavMeshAgent>();
        spawnAgent.enabled = false;
        spawnedCollectible.GetComponent<FollowPlayer>().Target = spawnTarget;
        spawnAgent.enabled = true;

        yield return new WaitForSeconds(spawnRate);
        readyToSpawn = true;
    }

    #endregion
}
