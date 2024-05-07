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
    [SerializeField]
    private GameObject[] objectsToActivate;

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
    private bool jam;
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

    public bool JamCheck
    {
        get
        {
            return jam;
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
        ActivateGameObjects();
    }

    private void SetUpInputs()
    {
        // Reset Inputs
        tutorialInputOne.transform.parent.gameObject.SetActive(false);
        tutorialInputOne.gameObject.SetActive(false);
        tutorialInputTwo.gameObject.SetActive(false);

        tutorialInputOne.moveListen = tutorialInputTwo.moveListen = false;
        tutorialInputOne.destroyListen = tutorialInputTwo.destroyListen = false;
        tutorialInputOne.jamListen = tutorialInputTwo.jamListen = false;
        tutorialInputOne.continueListen = tutorialInputTwo.continueListen = false;

        // Reconfigure
        if (playerOne)
        {
            tutorialInputOne.moveListen = move;
            tutorialInputOne.destroyListen = destroy;
            tutorialInputOne.jamListen = jam;
            tutorialInputOne.continueListen = continueTutorial;
        }

        if (playerTwo)
        {
            tutorialInputTwo.moveListen = move;
            tutorialInputTwo.destroyListen = destroy;
            tutorialInputTwo.jamListen = jam;
            tutorialInputTwo.continueListen = continueTutorial;
        }


        tutorialInputOne.gameObject.SetActive(true);
        tutorialInputTwo.gameObject.SetActive(true);
        tutorialInputOne.transform.parent.gameObject.SetActive(true);
    }

    private void ActivateGameObjects()
    {
        if(objectsToActivate.Length > 0)
        {
            foreach (GameObject obj in objectsToActivate)
            {
                obj.SetActive(true);
            }
        }
    }
}
