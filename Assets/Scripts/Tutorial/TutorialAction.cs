using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAction : TutorialObjectives
{
    private bool move;
    private bool destroy;
    private bool hamper;

    [SerializeField]
    [Tooltip("How much time to wait after right button was pressed to move on to next section")]
    private float timeToComplete = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        move = sectionActions.MoveCheck;
        destroy = sectionActions.DestroyCheck;
        hamper = sectionActions.JamCheck;
    }

    private void ObjectiveButtonPressed()
    {
        StartCoroutine(ToNextSection());
    }

    private IEnumerator ToNextSection()
    {
        Debug.Log("entered enumerrtiti");
        yield return new WaitForSeconds(timeToComplete);
        CompleteObjective();
        Debug.Log("Finsihed enuer=mrier");
    }

    private void OnEnable()
    {
        if(move)
        {
            TutorialInput.Moved += ObjectiveButtonPressed;
        }
        if (destroy)
        {
            TutorialInput.Destroyed += ObjectiveButtonPressed;
        }
        if (hamper)
        {
            TutorialInput.Jammed += ObjectiveButtonPressed;
        }
    }

    private void OnDisable()
    {
        if (move)
        {
            TutorialInput.Moved -= ObjectiveButtonPressed;
        }
        if (destroy)
        {
            TutorialInput.Destroyed -= ObjectiveButtonPressed;
        }
        if (hamper)
        {
            TutorialInput.Jammed -= ObjectiveButtonPressed;
        }
    }
}
