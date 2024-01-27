using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    private int maxStorage;
    private int[] playersPoints;

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

    private int GetStorage()
    {
        int sum = 0;
        foreach (int i in playersPoints)
        {
            sum += i;
        }
        return sum;
    }
}
