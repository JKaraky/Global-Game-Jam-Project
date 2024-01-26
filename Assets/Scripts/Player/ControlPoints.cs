using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPoints : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private int defaultPoints = 3;

    [SerializeField]
    private int maxPoints = 7;

    [SerializeField]
    private int currentPoints;
    #endregion

    #region To Increment and Decrement Points
    public void IncrementPoint()
    {
        currentPoints++;

        if(currentPoints == maxPoints)
        {
            Debug.Log("You have won");
        }
    }

    public void DecrementPoint()
    {
        currentPoints--;

        if(currentPoints == 0)
        {
            Debug.Log("You have Lost");
        }
    }
    #endregion
}
