using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSections : MonoBehaviour
{
    [SerializeField]
    private GameObject[] sections;

    private LinkedList<GameObject> sectionsList;
    private LinkedListNode<GameObject> currentSection;

    private void Start()
    {
        sectionsList = new LinkedList<GameObject>(sections);
        currentSection = sectionsList.First;
    }

    private void MoveToNextSection()
    {
        if(currentSection == sectionsList.Last)
        {
            return;
        }
        else
        {
            currentSection.Value.SetActive(false);
            currentSection = currentSection.Next;
            currentSection.Value.SetActive(true);
        }
    }

    private void OnEnable()
    {
        TutorialObjectives.ObjectiveComplete += MoveToNextSection;
    }

    private void OnDisable()
    {
        TutorialObjectives.ObjectiveComplete -= MoveToNextSection;
    }
}
