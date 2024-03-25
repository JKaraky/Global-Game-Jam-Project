using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Points : MonoBehaviour
{
    #region Variables
    private int playerOnePoints;
    private int playerTwoPoints;
    [SerializeField]
    [Tooltip("Points the players start with. Player must have twice this value to win the game")]
    private int startingPoints = 3;
    private int winningScore;
    public enum Players { Nobody, PlayerOne, PlayerTwo };
    public static event Action<Players> PlayerWon;
    public static event Action<int> TurnsToSpecialSpawn;

    public int PlayerOnePoints
    {
        get
        {
            return playerOnePoints;
        }
        set
        {
            playerOnePoints = value;
        }
    }

    public int PlayerTwoPoints
    {
        get
        {
            return playerTwoPoints;
        }
        set
        {
            playerTwoPoints = value;
        }
    }
    #endregion

    #region Start
    private void Start()
    {
        playerOnePoints = playerTwoPoints = startingPoints;
        winningScore = startingPoints * 2;
        TurnsToSpecialSpawn?.Invoke(startingPoints);
    }

    //private void Update()
    //{
    //    Debug.Log(playerOnePoints + " " + playerTwoPoints);
    //}
    #endregion

    #region Points Handling
    private void PointForPlayerOne()
    {
        playerOnePoints++;
        playerTwoPoints--;

        if(playerOnePoints == winningScore)
        {
            PlayerWon?.Invoke(Players.PlayerOne);
        }

        TurnsToSpecialSpawn?.Invoke(GetLoserPoints());
    }

    private void PointForPlayerTwo()
    {
        playerTwoPoints++;
        playerOnePoints--;

        if (playerTwoPoints == winningScore)
        {
            PlayerWon?.Invoke(Players.PlayerTwo);
        }

        TurnsToSpecialSpawn?.Invoke(GetLoserPoints());
    }

    private void RemoveFromWinner()
    {
        if(playerOnePoints == playerTwoPoints)
        {
            return;
        }
        else if(playerOnePoints < playerTwoPoints)
        {
            PointForPlayerOne();
        }
        else
        {
            PointForPlayerTwo();
        }

        TurnsToSpecialSpawn?.Invoke(GetLoserPoints());
    }

    private int GetLoserPoints()
    {
        if (playerOnePoints == playerTwoPoints)
        {
            return playerOnePoints;
        }
        else if (playerOnePoints < playerTwoPoints)
        {
            return playerOnePoints;
        }
        else
        {
            return playerTwoPoints;
        }
    }
    #endregion

    #region Collision Handling
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CollectibleOne"))
        {
            PointForPlayerOne();
        }

        if (collision.gameObject.CompareTag("CollectibleTwo"))
        {
            PointForPlayerTwo();
        }

        if (collision.gameObject.CompareTag("CollectibleThree"))
        {
            RemoveFromWinner();
        }
    }
    #endregion
}
