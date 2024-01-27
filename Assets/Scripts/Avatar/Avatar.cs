using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private int avatarNumber;
    private int maxStorage;
    private int currentStorage;
    private int[] playersPoints;
    private int controllingPlayer;

    public int AvatarNumber
    {
        get
        {
            return avatarNumber;
        }
        set
        {
            avatarNumber = value;
        }
    }
    public int MaxStorage
    {
        get
        {
            return maxStorage;
        }
        set
        {
            maxStorage = value;
        }
    }

    public int CurrentStorage
    {
        get
        {
            return GetStorage();
        }
    }

    public int[] PlayersPoints
    {
        get
        {
            return playersPoints;
        }
        set
        {
            playersPoints = value;
        }
    }

    public int ControllingPlayer
    {
        get
        {
            return controllingPlayer;
        }
        set
        {
            controllingPlayer = value;
        }
    }

    public static event Action<int, int> IncreasePoint;
    public static event Action<int, int> DecreasePoint;

    public static event Action<int, int> GainedControl;
    public static event Action<int, int> LostControl;
    #endregion
    private void Start()
    {
        GetStorage();
    }

    #region Avatar Methods
    private int GetStorage()
    {
        int sum = 0;
        foreach (int i in playersPoints)
        {
            sum += i;
        }

        currentStorage = sum;
        return currentStorage;
    }

    public void AddPoint(int player)
    {
        if(currentStorage == maxStorage)
        {
            for (int i = 0; i < playersPoints.Length; i++)
            {
                if (i == player)
                {
                    playersPoints[i]++;
                    IncreasePoint?.Invoke(avatarNumber, i);
                }
                else
                {
                    SubtractPoint(i);
                }
            }
        }
        else
        {
            playersPoints[player]++;
            GetStorage();
            IncreasePoint?.Invoke(avatarNumber, player);
        }

        if (playersPoints[player] == MaxStorage)
        {
            controllingPlayer = player;
            GainedControl?.Invoke(avatarNumber, controllingPlayer);
        }
    }

    public void SubtractPoint(int player)
    {
        playersPoints[player]--;

        if (playersPoints[player] == 0)
        {
            if(controllingPlayer == player)
            {
                LostControl?.Invoke(avatarNumber, controllingPlayer);
                controllingPlayer = -1;
            }
        }

        DecreasePoint?.Invoke(avatarNumber, player);
    }

    public bool CheckPickableAvatar(int playerNumber)
    {
        if (playerNumber == controllingPlayer)
        {
            if (playersPoints[playerNumber] == maxStorage)
                return false;
        }
        return true;
    }
    #endregion
}
