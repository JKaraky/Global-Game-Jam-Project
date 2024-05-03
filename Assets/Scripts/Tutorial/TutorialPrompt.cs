using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPrompt : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private TutorialInput tutorialInputOne;
    [SerializeField]
    private TutorialInput tutorialInputTwo;
    [SerializeField]
    private GameObject[] playerOnePrompts;
    [SerializeField]
    private GameObject[] playerTwoPrompts;

    [Header("Actions Player Can Do")]
    [SerializeField]
    private bool move;
    [SerializeField]
    private bool destroy;
    [SerializeField]
    private bool hamper;

    private bool turnOff;   // to turn off prompt holder after taking action with player continue input

    public static event Action NextSection;
    #endregion

    #region Awake
    private void Awake()
    {
        SetUpInputs();

        if(playerOnePrompts.Length > 0 && playerTwoPrompts.Length > 0)
        {
            turnOff = false;
        }
        else
        {
            turnOff= true;
        }
    }
    #endregion

    #region Prompt Logic
    private void ManagePrompts(int player)
    {
        if(player == 0)
        {
            if(playerOnePrompts.Length > 0)
            {
                foreach (GameObject prompt in playerOnePrompts)
                {
                    prompt.SetActive(false);
                }

                CheckTurnOff();
            }
        }
        else
        {
            if (playerTwoPrompts.Length > 0)
            {
                foreach (GameObject prompt in playerTwoPrompts)
                {
                    prompt.SetActive(false);
                }

                CheckTurnOff();
            }
        }
    }

    private void CheckTurnOff()
    {
        if (turnOff)
        {
            NextSection?.Invoke();
            this.gameObject.SetActive(false);
        }
        else
        {
            turnOff = true;
        }
    }

    private void SetUpInputs()
    {
        tutorialInputOne.moveListen = tutorialInputTwo.moveListen = move;
        tutorialInputOne.destroyListen = tutorialInputTwo.destroyListen = destroy;
        tutorialInputOne.hamperListen = tutorialInputTwo.hamperListen = hamper;
    }
    #endregion

    #region OnEnable and OnDisable
    private void OnEnable()
    {
        TutorialInput.ContinuePressed += ManagePrompts;
    }

    private void OnDisable()
    {
        TutorialInput.ContinuePressed -= ManagePrompts;
    }
    #endregion
}
