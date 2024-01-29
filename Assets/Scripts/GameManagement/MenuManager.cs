using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject learnMenu;

    public void StartGame()
    {
        SceneManager.LoadScene("Arena", LoadSceneMode.Single);
    }

    public void GoToLearn()
    {
        learnMenu.SetActive(true);
    }

    public void GoBackFromLearn()
    {
        learnMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
