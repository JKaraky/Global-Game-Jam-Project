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

    public void PointDestination(int slot)
    {
        switch(slot)
        {
            case 0:
                destination = Avatars.AvatarOne;
                break;
            case 1:
                destination = Avatars.AvatarTwo;
                break;
            case 2:
                destination = Avatars.AvatarThree;
                break;
            default:
                destination = Avatars.AvatarOne;
                break;
        }
    }
}
