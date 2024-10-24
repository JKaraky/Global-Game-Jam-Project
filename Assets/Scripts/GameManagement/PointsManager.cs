using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Avatars { AvatarOne, AvatarTwo, AvatarThree};
public class PointsManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private ControlPoints pointsOne, pointsTwo;
    private List<ControlPoints> playersPoints;

    [SerializeField]
    private Avatar avatarOne, avatarTwo, avatarThree;
    private List<Avatar> allAvatars;

    private int defaultPoints = 3;
    private int losingPlayer;

    private List<Avatar> avatarsControlledByOne;
    private List<Avatar> avatarsControlledByTwo;
    private List<List<Avatar>> playersAvatars;

    public static event Action<int> SpecialSpawnWave;
    public static event Action<int> GainedPoint;
    public static event Action<int> GameOver;
    public static event Action<int> PlayerAvatarsChanged;

    public List<List<Avatar>> PlayersAvatars
    {
        get
        {
            return playersAvatars;
        }
    }
    #endregion

    #region Points Setup
    private void Awake()
    {
        // Setup starting points
        pointsOne.CurrentPoints = defaultPoints;
        pointsTwo.CurrentPoints = defaultPoints;

        // Setup avatar one
        avatarOne.MaxStorage = defaultPoints;
        avatarOne.PlayersPoints = new int[2] { defaultPoints, 0 };
        avatarOne.ControllingPlayer = 0;

        // Setup avatar two
        avatarTwo.MaxStorage = defaultPoints;
        avatarTwo.PlayersPoints = new int[2] { 0, defaultPoints };
        avatarTwo.ControllingPlayer = 1;

        // Setup avatar three
        avatarThree.MaxStorage = defaultPoints / 2;
        avatarThree.PlayersPoints = new int[2] { 0, 0 };
        avatarThree.ControllingPlayer = 3;

        // Setup controlled avatars lists
        avatarsControlledByOne = new List<Avatar> { avatarOne };
        avatarsControlledByTwo = new List<Avatar> { avatarTwo };

        playersPoints = new List<ControlPoints> { pointsOne, pointsTwo };
        allAvatars = new List<Avatar> { avatarOne, avatarTwo, avatarThree };
        playersAvatars = new List<List<Avatar>> { avatarsControlledByOne, avatarsControlledByTwo };
    }
    #endregion

    #region Points Management Methods
    public void IncrementPoint(ControlPoints controlPoints)
    {
        Avatars destination = controlPoints.Destination;

        if(destination == Avatars.AvatarOne)
        {
            avatarOne.AddPoint(controlPoints.Player);
        }
        else if(destination == Avatars.AvatarTwo)
        {
            avatarTwo.AddPoint(controlPoints.Player);
        }
        else
        {
            avatarThree.AddPoint(controlPoints.Player);
        }
    }

    public void DecrementPoint(int playerToDecrease)
    {
        foreach (Avatar avatar in allAvatars)
        {
            if (!playersAvatars[playerToDecrease].Contains(avatar))
            {
                if (avatar.PlayersPoints[playerToDecrease] != 0)
                {
                    avatar.AddPoint(losingPlayer);
                    return;
                }
            }
            else if (avatar.AvatarNumber == 2)
            {
                avatarThree.SubtractPoint(playerToDecrease);
                return;
            }
        }
        playersAvatars[playerToDecrease][0].SubtractPoint(playerToDecrease);
    }
    #endregion

    #region Comparison Checks
    private int PlayerWithMostPoints()
    {
        if (pointsOne.CurrentPoints == pointsTwo.CurrentPoints)
        {
            losingPlayer = -1;
            return -1;
        }
        else if(pointsOne.CurrentPoints < pointsTwo.CurrentPoints)
        {
            losingPlayer = 0;
            return 1;
        }
        else
        {
            losingPlayer = 1;
            return 0;
        }
    }
    #endregion

    #region Adding and Removing Player Points and Controlling Players

    private void IncreasePoint(int avatar, int player)
    {
        ControlPoints controlPointsScript = playersPoints[player];
        controlPointsScript.CurrentPoints++;
        GainedPoint?.Invoke(player);

        PlayerWithMostPoints();

        if(losingPlayer == -1)
        {
            SpecialSpawnWave?.Invoke(playersPoints[player].CurrentPoints);
        }
        else
        {
            SpecialSpawnWave?.Invoke(playersPoints[losingPlayer].CurrentPoints);
        }
    }

    private void DecreasePoint(int avatar, int player)
    {
        ControlPoints controlPointsScript = playersPoints[player];
        controlPointsScript.CurrentPoints--;
        if (controlPointsScript.CurrentPoints == 0)
        {
            GameOver?.Invoke(player);
            Debug.Log("Congratulations " + (controlPointsScript.Player==0 ? "Duck" : "Worm") + " you showcased why your father left!");
        }

        PlayerWithMostPoints();

        if (losingPlayer == -1)
        {
            SpecialSpawnWave?.Invoke(playersPoints[player].CurrentPoints);
        }
        else
        {
            SpecialSpawnWave?.Invoke(playersPoints[losingPlayer].CurrentPoints);
        }
    }

    private void AddControl(int avatar, int player)
    {
        playersAvatars[player].Add(allAvatars[avatar]);

        PlayerAvatarsChanged?.Invoke(player);
    }

    private void RemoveControl(int avatar, int player)
    {
        playersAvatars[player].Remove(allAvatars[avatar]);

        PlayerAvatarsChanged?.Invoke(player);
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
            int playerToDecrease = PlayerWithMostPoints();
            if(playerToDecrease != -1)
            {
                DecrementPoint(playerToDecrease);
            }
        }
    }
    #endregion

    #region OnEnable and OnDisable
    private void OnEnable()
    {
        Avatar.IncreasePoint += IncreasePoint;
        Avatar.DecreasePoint += DecreasePoint;
        Avatar.GainedControl += AddControl;
        Avatar.LostControl += RemoveControl;
    }

    private void OnDisable()
    {
        Avatar.IncreasePoint -= IncreasePoint;
        Avatar.DecreasePoint -= DecreasePoint;
        Avatar.GainedControl -= AddControl;
        Avatar.LostControl -= RemoveControl;
    }
    #endregion
}
