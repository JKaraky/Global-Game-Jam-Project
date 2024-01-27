using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class ControlPoints : MonoBehaviour
{
    #region Variables
    private int player;
    private Avatars destination;
    private int currentPoints;

    public int Player
    {
        get 
        { 
            return player; 
        }
    }
    public int CurrentPoints { 
        get 
        { 
            return currentPoints;
        }
        set
        {
            currentPoints = value;
        }
    }

    public Avatars Destination
    {
        get
        {
            return destination;
        }
        set
        {
            destination = value;
        }
    }
    #endregion

    private void Start()
    {
        player = GetComponent<AvatarController>().PlayerNbr;
    }

    public Avatars PointDestination()
    {
        int destinationofPoint = GetComponent<AvatarController>().ControlPointSlot;
        switch(destinationofPoint)
        {
            case 0:
                destination = Avatars.AvatarOne;
                return destination;
            case 1:
                destination = Avatars.AvatarTwo;
                return destination;
            case 2:
                destination = Avatars.AvatarThree;
                return destination;
            default:
                destination = Avatars.AvatarThree;
                return destination;
        }
    }
}
