using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private float spawnRate = 5;
    [SerializeField] private CollectiblePooling[] pools;

    private int spawnWave = 0;
    private int specialSpawnWave;

    public int SpecialSpawnWave
    {
        get
        {
            return specialSpawnWave;
        }
        set
        {
            specialSpawnWave = value;
        }
    }

    private bool normalSpawn = true;
    private bool specialSpawn = false;
    #endregion
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (normalSpawn)
        {
            StartCoroutine(NormalSpawn());
        }
        else if(specialSpawn)
        {
            StartCoroutine(SpecialSpawn());
        }
        else
        {
            return;
        }
    }

    #region Types of Spawning
    private IEnumerator NormalSpawn()
    {
        normalSpawn = false;
        int poolToSpawnOne = Random.Range(0, 2);
        int poolToSpawnTwo = Random.Range(0, 2);
        pools[poolToSpawnOne].pool.Get();
        pools[poolToSpawnTwo].pool.Get();
        yield return new WaitForSeconds(spawnRate);
        KeepTrackSpawning();
    }

    private IEnumerator SpecialSpawn()
    {
        Debug.Log("Spawned power 3");
        specialSpawn = false;
        pools[pools.Length - 1].pool.Get();
        yield return new WaitForSeconds(spawnRate);
        NormalSpawning();
    }
    #endregion

    #region To Set and check for Spawn Mode
    private void KeepTrackSpawning()
    {
        Debug.Log("This is wave " + spawnWave + " special spawning is every " + specialSpawnWave);
        spawnWave++;
        if(spawnWave >= specialSpawnWave)
        {
            SpecialSpawning();
            spawnWave = 0;
        }
        else
        {
            NormalSpawning();
        }
    }
    public void NormalSpawning()
    {
        normalSpawn = true;
        specialSpawn = false;
    }

    public void SpecialSpawning()
    {
        normalSpawn = false;
        specialSpawn = true;
    }
    #endregion

    #region To Change Special Spawn Rate
    private void ChangeSpecialSpawn(int wave)
    {
        specialSpawnWave = wave;
    }
    #endregion

    #region OnEnable and OnDisable
    private void OnEnable()
    {
        PointsManager.SpecialSpawnWave += ChangeSpecialSpawn;
    }

    private void OnDisable()
    {
        PointsManager.SpecialSpawnWave -= ChangeSpecialSpawn;
    }
    #endregion

}
