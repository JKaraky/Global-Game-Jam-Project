using System;
using UnityEngine;

public class TutorialPrompt : TutorialObjectives
{
    #region Variables
    [SerializeField]
    private GameObject[] playerOnePrompts;
    [SerializeField]
    private GameObject[] playerTwoPrompts;
    private bool turnOff;   // to turn off prompt holder after taking action with player continue input
    #endregion

    #region Awake
    private void Awake()
    {
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
            CompleteObjective();
        }
        else
        {
            turnOff = true;
        }
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
