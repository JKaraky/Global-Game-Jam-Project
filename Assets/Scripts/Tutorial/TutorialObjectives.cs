using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SectionActions))]
public class TutorialObjectives : MonoBehaviour
{
    #region Variables
    protected TutorialInput tutorialInputOne;
    protected TutorialInput tutorialInputTwo;
    protected SectionActions sectionActions;

    public static event Action ObjectiveComplete;
    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        sectionActions = gameObject.GetComponent<SectionActions>();
        tutorialInputOne = sectionActions.TutorialInputOne;
        tutorialInputTwo = sectionActions.TutorialInputTwo;
    }

    protected void CompleteObjective()
    {
        ObjectiveComplete?.Invoke();
    }
}
