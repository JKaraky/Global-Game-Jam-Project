using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Avatars { AvatarOne, AvatarTwo, AvatarThree};
public class PointsManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private ControlPoints pointsOne, pointsTwo;
    [SerializeField]
    private Avatar avatarOne, avatarTwo, avatarThree;

    private int defaultPoints = 3;
    private int maxPoints;

    private List<Avatar> avatarsControlledByOne;
    private List<Avatar> avatarsControlledByTwo;
    private List<List<Avatar>> playersAvatars;
    #endregion

    private void Start()
    {
        maxPoints = (defaultPoints * 2) + (defaultPoints / 2);

        // Setup starting points
        pointsOne.CurrentPoints = defaultPoints;
        pointsTwo.CurrentPoints = defaultPoints;

        // Setup avatar one
        avatarOne.MaxStorage = defaultPoints;
        avatarOne.PlayersPoints = new int[2] { defaultPoints, 0 };

        // Setup avatar two
        avatarTwo.MaxStorage = defaultPoints;
        avatarTwo.PlayersPoints = new int[2] { 0, defaultPoints };

        // Setup avatar three
        avatarThree.MaxStorage = defaultPoints / 2;
        avatarThree.PlayersPoints = new int[2] { 0, 0 };

        // Setup controlled avatars lists
        avatarsControlledByOne = new List<Avatar> { avatarOne };
        avatarsControlledByTwo = new List<Avatar> { avatarTwo };
    }

    #region Points Management Methods
    public void IncrementPoint(ControlPoints controlPoints)
    {
        IncreasePoint(controlPoints);

        Avatars destination = controlPoints.Destination;

        if(destination == Avatars.AvatarOne)
        {
            ManageAvatar(avatarOne, controlPoints.Player);
        }
        else if(destination == Avatars.AvatarTwo)
        {
            ManageAvatar(avatarTwo, controlPoints.Player);
        }
        else
        {
            ManageAvatar(avatarThree, controlPoints.Player);
        }
    }

    public void DecrementPoint(ControlPoints controlPoints)
    {

    }
    #endregion

    #region Comparison Checks
    private ControlPoints GiveBiggestPoints()
    {
        if (pointsOne.CurrentPoints == pointsTwo.CurrentPoints)
        {
            return null;
        }
        else if(pointsOne.CurrentPoints < pointsTwo.CurrentPoints)
        {
            return pointsTwo;
        }
        else
        {
            return pointsOne;
        }
    }
    #endregion

    #region Managing Avatars and Points Algorithms

    private void IncreasePoint(ControlPoints controlPoints)
    {
        controlPoints.CurrentPoints++;
        if (controlPoints.CurrentPoints == maxPoints)
        {
            Debug.Log("Player " + controlPoints.Player + " took full control!");
        }
    }
    private void ManageAvatar(Avatar avatar, int playerNumber)
    {
        IncreasePointsInAvatar(avatar, playerNumber);
        CheckControlOfAvatar(avatar, playerNumber);
    }

    private void IncreasePointsInAvatar(Avatar avatar, int playerNumber)
    {
        if (avatar.CurrentStorage == avatar.MaxStorage)
        {
            foreach (int i in avatar.PlayersPoints)
            {
                if (i == playerNumber)
                {
                    avatar.PlayersPoints[i]++;
                }
                else
                {
                    avatar.PlayersPoints[i]--;
                }
            }
        }
        else
        {
            avatar.PlayersPoints[playerNumber]++;
        }
    }

    private void CheckControlOfAvatar(Avatar avatar, int playerNumber)
    {
        if (avatar.PlayersPoints[playerNumber] == avatar.MaxStorage)
        {
            for (int i = 0; i < playersAvatars.Count; i++)
            {
                if (i == playerNumber)
                {
                    playersAvatars[i].Add(avatar);
                }
                else
                {
                    if (playersAvatars[i].Contains(avatar))
                    {
                        playersAvatars[i].Remove(avatar);
                    }
                }
            }
        }
    }
    #endregion

    #region Collision Handling
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CollectibleOne"))
        {
            IncrementPoint(pointsOne);
        }
        else if (collision.gameObject.CompareTag("CollectibleTwo"))
        {
            IncrementPoint(pointsTwo);
        }
        else if (collision.gameObject.CompareTag("CollectibleThree"))
        {
            ControlPoints playerToDecrease = GiveBiggestPoints();
            DecrementPoint(playerToDecrease);
        }
    }
    #endregion
}
