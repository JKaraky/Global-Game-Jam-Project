using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private AvatarController avatarControllerOne, avatarControllerTwo;

    private int defaultPoints = 3;
    private int maxPoints;
    private int currentPointsOne, currentPointsTwo, maxNormalAvatar, maxSpecialAvatar;

    private int[] avatarOne, avatarTwo, avatarThree;
    #endregion

    private void Start()
    {
        maxPoints = (defaultPoints * 2) + 1;
        currentPointsOne = defaultPoints;
        currentPointsTwo = defaultPoints;
        maxNormalAvatar = defaultPoints;
        maxSpecialAvatar = defaultPoints / 2;
        avatarOne = new int[2] { currentPointsOne, 0 };
        avatarTwo = new int[2] { 0, currentPointsTwo };
        avatarThree = new int[1] { 0 };
    }

    #region Points Management Methods
    public void IncrementPoint(int playerPoint)
    {
        if(playerPoint == 1)
        {
            //Start here when coding next time
        }
    }

    public void DecrementPoint(int currentPoints)
    {
        currentPoints--;

        if (currentPoints == 0)
        {
            Debug.Log("You have Lost");
        }
    }
    #endregion

    #region Comparison Method
    private int GiveBiggestPoints()
    {
        if (currentPointsOne == currentPointsTwo)
        {
            return 0;
        }
        else if(currentPointsOne < currentPointsTwo)
        {
            return currentPointsTwo;
        }
        else
        {
            return currentPointsOne;
        }
    }
    #endregion

    #region Collision Handling
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CollectibleOne"))
        {
            IncrementPoint(1);
        }
        else if (collision.gameObject.CompareTag("CollectibleTwo"))
        {
            IncrementPoint(2);
        }
        else if (collision.gameObject.CompareTag("CollectibleThree"))
        {
            int biggestPoints = GiveBiggestPoints();
            DecrementPoint(biggestPoints);
        }
    }
    #endregion
}
