using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    #region Singleton Setup
    // To setup Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                SetupInstance();
            }

            return instance;
        }
    }
    #endregion

    #region Variables
    [SerializeField] private GameObject player;
    [SerializeField] private SpawnManager spawnManager;

    public GameObject Player
    {
        get
        {
            return player;
        }
    }

    [SerializeField] private CollectiblePooling pool;
    private ControlPoints[] controlArray;
    #endregion

    #region Awake Methods
    private void Awake()
    {
        if(player == null)
        {
            Debug.Log("Player in Game Manager is empty");
        }

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        controlArray = player.GetComponentsInChildren<ControlPoints>();
    }

    #region Method to Create Singleton Instance
    private static void SetupInstance()
    {
        instance = FindObjectOfType<GameManager>();

        if(instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = "GameManager";
            instance = gameObj.AddComponent<GameManager>();
        }
    }
    #endregion
}
