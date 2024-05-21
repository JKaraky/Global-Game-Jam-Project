using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToArena : MonoBehaviour
{
    public void StartGame()
    {
        string sceneToLoad = "Arena";

        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }
    private void OnEnable()
    {
        StartGame();
    }
}
