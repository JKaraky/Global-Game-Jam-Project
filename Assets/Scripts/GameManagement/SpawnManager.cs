using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private float intervalBetweenSpawns = 5;
    [SerializeField] private CollectiblePooling[] pools;

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
            SpecialSpawn();
        }
        else
        {
            return;
        }
    }

    private IEnumerator NormalSpawn()
    {
        normalSpawn = false;
        int poolToSpawnOne = Random.Range(0, 2);
        int poolToSpawnTwo = Random.Range(0, 2);
        pools[poolToSpawnOne].pool.Get();
        pools[poolToSpawnTwo].pool.Get();
        yield return new WaitForSeconds(intervalBetweenSpawns);
        normalSpawn = true;
    }

    private void SpecialSpawn()
    {
        specialSpawn = false;
        pools[pools.Length - 1].pool.Get();
    }

    #region To Set Spawn Mode
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

}
