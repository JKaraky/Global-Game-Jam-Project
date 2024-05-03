using System;
using UnityEngine;

[RequireComponent(typeof(TutorialObjectives))]
public class SectionActions : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private TutorialInput tutorialInputOne;
    [SerializeField]
    private TutorialInput tutorialInputTwo;

    public TutorialInput TutorialInputOne
    {
        get
        {
            return tutorialInputOne;
        }
    }

    public TutorialInput TutorialInputTwo
    {
        get
        {
            return tutorialInputTwo;
        }
    }

    [Header("Actions Player Can Do")]
    [SerializeField]
    private bool move;
    [SerializeField]
    private bool destroy;
    [SerializeField]
    private bool hamper;
    [SerializeField]
    private bool continueTutorial;

    public bool MoveCheck
    {
        get
        {
            return move;
        }
    }

    public bool DestroyCheck
    {
        get
        {
            return destroy;
        }
    }

    public bool HamperCheck
    {
        get
        {
            return hamper;
        }
    }

    [Header("Who Can Do The Actions")]
    [SerializeField]
    private bool playerOne;
    [SerializeField]
    private bool playerTwo;
    #endregion

    private void Awake()
    {
        SetUpInputs();
    }

    private void SetUpInputs()
    {
        // Reset Inputs
        tutorialInputOne.gameObject.SetActive(false);
        tutorialInputTwo.gameObject.SetActive(false);

        tutorialInputOne.moveListen = tutorialInputTwo.moveListen = false;
        tutorialInputOne.destroyListen = tutorialInputTwo.destroyListen = false;
        tutorialInputOne.hamperListen = tutorialInputTwo.hamperListen = false;
        tutorialInputOne.continueListen = tutorialInputTwo.continueListen = false;

        // Reconfigure
        if (playerOne)
        {
            tutorialInputOne.moveListen = move;
            tutorialInputOne.destroyListen = destroy;
            tutorialInputOne.hamperListen = hamper;
            tutorialInputOne.continueListen = continueTutorial;
        }

        if (playerTwo)
        {
            tutorialInputTwo.moveListen = move;
            tutorialInputTwo.destroyListen = destroy;
            tutorialInputTwo.hamperListen = hamper;
            tutorialInputTwo.continueListen = continueTutorial;
        }


        tutorialInputOne.gameObject.SetActive(true);
        tutorialInputTwo.gameObject.SetActive(true);
    }
}
