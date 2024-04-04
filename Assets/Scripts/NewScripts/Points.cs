using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public enum Players { Nobody, PlayerOne, PlayerTwo };

[RequireComponent(typeof(Collider))]
public class Points : MonoBehaviour
{
    #region Variables
    private int playerOnePoints;
    private int playerTwoPoints;
    private int winningScore;

    [SerializeField]
    [Tooltip("Points the players start with. Player must have twice this value to win the game.")]
    private int startingPoints = 3;

    public static event Action<Players> PlayerWon;
    public static event Action<Players> PointForPlayer;
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

    public int StartingPoints
    {
        get
        {
            return startingPoints;
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
    #endregion

    #region Points Handling
    private void PointForPlayerOne()
    {
        playerOnePoints++;
        playerTwoPoints--;
        PointForPlayer?.Invoke(Players.PlayerOne);

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
        PointForPlayer?.Invoke(Players.PlayerTwo);

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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CollectibleOne"))
        {
            PointForPlayerOne();
        }

        if (other.gameObject.CompareTag("CollectibleTwo"))
        {
            PointForPlayerTwo();
        }

        if (other.gameObject.CompareTag("CollectibleThree"))
        {
            RemoveFromWinner();
        }
    }
    #endregion
}
