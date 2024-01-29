using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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

    public GameObject Player
    {
        get
        {
            return player;
        }
    }

    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField]
    private ParticleSystem particleEffect;
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

    #region Game Over Method
    private void GameOver(int player)
    {
        text.text = "Congratulations player " + (player + 1) + ", you have proved why your father left!";
        Time.timeScale = 0f;
        gameOverScreen.SetActive(true);
    }
    #endregion

    #region Button Events

    public void RestartLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    #region Particle Effect PLay
    private void PlayParticleEffect(Vector3 position)
    {
        Instantiate(particleEffect, position, Quaternion.identity);
        particleEffect.Emit(100);
    }
    #endregion

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

    private void OnEnable()
    {
        PointsManager.GameOver += GameOver;
        Collectible.PlayDestroyEffect += PlayParticleEffect;
    }

    private void OnDisable()
    {
        PointsManager.GameOver -= GameOver;
        Collectible.PlayDestroyEffect -= PlayParticleEffect;
    }
}
